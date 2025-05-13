using MI.Dal.IDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Order.ViewModal;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Utility;
using System.Globalization;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Way2GoWEB.Payment;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using PayPalCheckoutSdk.Orders;
using System.Collections.Generic;
using PayPalCheckoutSdk.Core;
using MI.Entity.Models;
using Remotion.Linq.Clauses.ResultOperators;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PageOrderController : ControllerBase
    {
        private IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;

        public PageOrderController(IOrderRepository orderRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
        }

        private async Task<ResponseCreateMultipleItemOrder> _CreateMultipleItemOrderManualy(RequestCreateMultipleItemOrder reqest)
        {
            var cultureCode = "";
            switch (reqest.i18Code)
            {
                case "en":
                    cultureCode = "en-US";
                    break;
                case "vi":
                    cultureCode = "vi-VN";
                    break;
            }

            // Truoc khi tao thi can tien hanh kiem tra don hang truoc
            //Kiem tra OnepayRefs voi request.orderCode

            


            var response = await _orderRepository.CreateMultipleItemOrderAsync(reqest);
            if (response != null)
            {

                if (response.auth.isNewUser)
                {
                    var requestCreateAccount = new RequestSendNewUserEmail();
                    requestCreateAccount.culture_code = cultureCode;
                    requestCreateAccount.customerEmail = response.auth.email;
                    requestCreateAccount.username = response.auth.email;
                    requestCreateAccount.password = response.auth.pcname;
                    requestCreateAccount.customerName = response.auth.firstName + " " + response.auth.lastName;
                    //Gui Email den khach hang
                    var sendMailStatus = await _orderRepository.SendNewUserEmail(requestCreateAccount);

                }

                if (!string.IsNullOrEmpty(response.orderCode))
                {
                    var requestCreateOrder = new RequestSendNewOrderEmail();
                    requestCreateOrder.customerId = response.auth.id;
                    requestCreateOrder.customerName = response.auth.firstName + " " + response.auth.lastName;
                    requestCreateOrder.customerEmail = response.auth.email;
                    requestCreateOrder.orderCode = response.orderCode;
                    requestCreateOrder.culture_code = cultureCode;
                    requestCreateOrder.orderNotes = reqest.orderNotes;
                    var sendMailOrders = await _orderRepository.SendNewOrderEmailToCustomer(requestCreateOrder);
                    var sendHelpDeskMaiOrder = await _orderRepository.SendNewOrderEmailToHelpDesk(requestCreateOrder);
                }

                return response;
            }
            else
            {
                return null;
            }
        }

        private async Task<ResponseCreateMultipleItemOrder> _CreateMultipleItemOrderManualyForApp(RequestCreateMultipleItemOrder reqest)
        {
            var cultureCode = "";
            switch (reqest.i18Code)
            {
                case "en":
                    cultureCode = "en-US";
                    break;
                case "vi":
                    cultureCode = "vi-VN";
                    break;
            }

            // Truoc khi tao thi can tien hanh kiem tra don hang truoc
            //Kiem tra OnepayRefs voi request.orderCode

            var isOrderCreated = false;
            using (IDbContext _context = new IDbContext())
            {
                var query = await _context.Orders.Where(r => r.OnepayRef.Equals(reqest.orderCode)).FirstOrDefaultAsync();
                if(query != null)
                {
                    isOrderCreated = true;
                    var _response = new ResponseCreateMultipleItemOrder();
                    //trar ve thong tin khach hang
                    var customer = await _context.Customer.FindAsync(query.CustomerId);
                    if(customer != null)
                    {
                        var _responseCustomer = new CustomerAuth();
                        _responseCustomer = reqest.auth;
                        _responseCustomer.id = customer.Id;
                        _responseCustomer.pcname = customer.Pcname;
                        _response.auth = _responseCustomer;
                        _response.orderCode = query.OrderCode;
                        

                        return _response;
                    }
                }
            }

            if(isOrderCreated == false)
            {
                var response = await _orderRepository.CreateMultipleItemOrderAsync(reqest);
                if (response != null)
                {

                    if (response.auth.isNewUser)
                    {
                        var requestCreateAccount = new RequestSendNewUserEmail();
                        requestCreateAccount.culture_code = cultureCode;
                        requestCreateAccount.customerEmail = response.auth.email;
                        requestCreateAccount.username = response.auth.email;
                        requestCreateAccount.password = response.auth.pcname;
                        requestCreateAccount.customerName = response.auth.firstName + " " + response.auth.lastName;
                        //Gui Email den khach hang
                        var sendMailStatus = await _orderRepository.SendNewUserEmail(requestCreateAccount);

                    }

                    if (!string.IsNullOrEmpty(response.orderCode))
                    {
                        var requestCreateOrder = new RequestSendNewOrderEmail();
                        requestCreateOrder.customerId = response.auth.id;
                        requestCreateOrder.customerName = response.auth.firstName + " " + response.auth.lastName;
                        requestCreateOrder.customerEmail = response.auth.email;
                        requestCreateOrder.orderCode = response.orderCode;
                        requestCreateOrder.culture_code = cultureCode;
                        requestCreateOrder.orderNotes = reqest.orderNotes;
                        var sendMailOrders = await _orderRepository.SendNewOrderEmailToCustomer(requestCreateOrder);
                        var sendHelpDeskMaiOrder = await _orderRepository.SendNewOrderEmailToHelpDesk(requestCreateOrder);
                    }

                    return response;
                }
            }
            return null;
        }


        [HttpPost]
        [Route("CreateMultipleItemOrderAsync")]
        public async Task<IActionResult> CreateMultipleItemOrderAsync(RequestCreateMultipleItemOrder reqest)
        {
            

            var response = await _CreateMultipleItemOrderManualy(reqest);
            if(response != null)
            {
                return Ok(response);

            }
            else
            {
                return BadRequest("ERROR");
            }

        }

        [HttpPost]
        [Route("ProcessPaymentOnePay")]
        public async Task<IActionResult> ProcessPaymentOnePay(RequestCreateMultipleItemOrder request)
        {
            //ExcuteGetMethod(string orderCode, string amount, string returnUrl, string customerEmail = "", string customerPhone = "", string customerId = "")
            if (request != null)
            {
                var mainAPI = _configuration["MainAPI"];
                var returnUrl = $"{mainAPI}/api/PageOrder/ResultPaymentOnePay";
                returnUrl += "/" + request.i18Code;



                var orderCode = request.orderCode;

                var customerEmail = request.auth.email;

                //Tinh toan lai gia tien o day
                decimal totalPrice = 0;
                foreach (var pay in request.pays)
                {
                    decimal _total = 0;
                    var choosenDate = pay.choosenDate; // Convert tu dinh dang dd/mm/yyyy => yyyy-mm-dd

                    DateTime parsedDate = DateTime.TryParseExact(pay.choosenDate, new[] { "d/M/yyyy", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : throw new FormatException("Invalid date format");

                    var combinationId = pay.combination.id;
                    var productChildId = pay.productChildId;
                    var productParentId = pay.productId;
                    var discountCoupon = "";
                    var numberOfAdut = pay.numberOfAldut;
                    var numberOfChild = pay.numberOfChildrend;
                    if (pay.discountSelected != null)
                    {
                        if (!string.IsNullOrEmpty(pay.discountSelected.couponCode))
                        {
                            discountCoupon = pay.discountSelected.couponCode;
                        }
                    }
                    //Bat dau goi vao database de lay ra gia thuc te
                    using (IDbContext context = new IDbContext())
                    {
                        var combinationDetail = await context.ProductPriceInZoneList.FindAsync(combinationId);
                        if (combinationDetail != null)
                        {
                            var zoneList = combinationDetail.ZoneList;
                            if (!string.IsNullOrEmpty(zoneList))
                            {
                                var productPriceDetail = await context.ProductPriceInZoneListByDate.FirstOrDefaultAsync(r => r.ProductId == productChildId && r.Date.Date == parsedDate.Date && r.ZoneList.Equals(zoneList));
                                if (productPriceDetail != null)
                                {
                                    _total = productPriceDetail.PriceEachNguoiLon * numberOfAdut + productPriceDetail.PriceEachTreEm * numberOfChild;
                                    //Tinh khuyen mai va tru di

                                }
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(discountCoupon))
                    {
                        var requestCoupon = new RequestCheckCouponCode();
                        requestCoupon.productId = productParentId;
                        requestCoupon.couponCode = discountCoupon;
                        requestCoupon.customerEmail = request.auth.email;
                        requestCoupon.culture_code = "vi-VN";

                        var resultCoupon = _orderRepository.CheckCouponCode(requestCoupon);
                        if(resultCoupon != null)
                        {
                            var discountOption = resultCoupon.DiscountOption;
                            var discountValue = resultCoupon.ValueDiscount;

                            if(discountOption == 1) // 1 la loai khuyen mai theo phan tram
                            {
                                _total = _total * (100 - discountValue) / 100;
                            }
                            if(discountOption == 2) // 2 la khuuyen mai theo gia tien
                            {
                                _total = _total - discountValue;
                            }
                        }
                    }
                    totalPrice += _total;

                }

                //decimal totalPrice = request.pays.Sum(r => r.totalPrice - r.discountSelected.couponPrice);


                totalPrice = totalPrice * 100;
                var priceInt = (int)totalPrice;
                var result = Onepay.ExcuteGetMethod(orderCode, priceInt.ToString(), returnUrl, customerEmail, requestBody: request);
                return Ok(result);
            }

            return BadRequest(request);
        }

       

        [HttpGet]
        [Route("ResultPaymentOnePay/{i18code}/{merchantEncriped}")]
        public async Task<IActionResult> ResultPaymentOnePay(string i18Code, string merchantEncriped)
        {
            // Get the vpc_TxnResponseCode parameter from the query string
            var txnResponseCode = Request.Query["vpc_TxnResponseCode"].ToString();
            //vpc_MerchTxnRef
            var vpc_MerchTxnRef = Request.Query["vpc_MerchTxnRef"].ToString();
            var baseUrl = _configuration["MainUrl"];
            if (!string.IsNullOrEmpty(txnResponseCode) && !string.IsNullOrEmpty(vpc_MerchTxnRef) && txnResponseCode.Equals("0"))
            {

                var checker = Onepay.Decrypt(merchantEncriped);
                if (checker.Equals(vpc_MerchTxnRef))
                {
                    //Doc vao database lay ra object tu merchantRef
                    using (IDbContext context = new IDbContext())
                    {
                        var paymentDetail = await context.OnepayRefs.FirstOrDefaultAsync(r => r.EncryptMerchantTxtCode.Equals(merchantEncriped));
                        if (paymentDetail != null)
                        {
                            var paymentObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestCreateMultipleItemOrder>(paymentDetail.Object);

                            //Kiem tra xem don hang da ton tai chua
                            var checkCreatedOrder = await context.Orders.FirstOrDefaultAsync(r => r.OnepayRef == paymentObject.orderCode);
                            if (checkCreatedOrder == null)
                            {
                                //Tao don hang
                                var response = await _CreateMultipleItemOrderManualy(paymentObject);
                                if (response != null)
                                {
                                    if (i18Code.Equals("en"))
                                    {

                                        var url = $"{baseUrl}/confirm/payment/success";
                                        return Redirect(url);
                                    }
                                    else
                                    {
                                        var url = $"{baseUrl}/{i18Code}/confirm/payment/success";
                                        return Redirect(url);
                                    }
                                }
                            }
                            else
                            {
                                if (i18Code.Equals("en"))
                                {

                                    var fail_url = $"{baseUrl}/confirm/payment/fail";
                                    return Redirect(fail_url);
                                }
                                else
                                {
                                    var fail_url = $"{baseUrl}/{i18Code}/confirm/payment/fail";
                                    return Redirect(fail_url);
                                }
                            }
                            
                            //return ve success
                        }
                    }

                    

                }
            }
            else
            {
                if (i18Code.Equals("en"))
                {

                    var fail_url = $"{baseUrl}/confirm/payment/fail";
                    return Redirect(fail_url);
                }
                else
                {
                    var fail_url = $"{baseUrl}/{i18Code}/confirm/payment/fail";
                    return Redirect(fail_url);
                }
            }
            return BadRequest();
        }




        [HttpPost]
        [Route("ProcessPaymentOnePayApp")]
        public async Task<IActionResult> ProcessPaymentOnePayApp(RequestCreateMultipleItemOrder request)
        {
            //ExcuteGetMethod(string orderCode, string amount, string returnUrl, string customerEmail = "", string customerPhone = "", string customerId = "")
            if (request != null)
            {
                var mainAPI = _configuration["MainAPI"];
                var returnUrl = $"{mainAPI}/api/PageOrder/ResultPaymentOnePayApp";
                returnUrl += "/" + request.i18Code;



                var orderCode = request.orderCode;

                var customerEmail = request.auth.email;

                //Tinh toan lai gia tien o day
                decimal totalPrice = 0;
                foreach (var pay in request.pays)
                {
                    decimal _total = 0;
                    var choosenDate = pay.choosenDate; // Convert tu dinh dang dd/mm/yyyy => yyyy-mm-dd

                    DateTime parsedDate = DateTime.TryParseExact(pay.choosenDate, new[] { "d/M/yyyy", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : throw new FormatException("Invalid date format");

                    var combinationId = pay.combination.id;
                    var productChildId = pay.productChildId;
                    var productParentId = pay.productId;
                    var discountCoupon = "";
                    var numberOfAdut = pay.numberOfAldut;
                    var numberOfChild = pay.numberOfChildrend;
                    if (pay.discountSelected != null)
                    {
                        if (!string.IsNullOrEmpty(pay.discountSelected.couponCode))
                        {
                            discountCoupon = pay.discountSelected.couponCode;
                        }
                    }
                    //Bat dau goi vao database de lay ra gia thuc te
                    using (IDbContext context = new IDbContext())
                    {
                        var combinationDetail = await context.ProductPriceInZoneList.FindAsync(combinationId);
                        if (combinationDetail != null)
                        {
                            var zoneList = combinationDetail.ZoneList;
                            if (!string.IsNullOrEmpty(zoneList))
                            {
                                var productPriceDetail = await context.ProductPriceInZoneListByDate.FirstOrDefaultAsync(r => r.ProductId == productChildId && r.Date.Date == parsedDate.Date && r.ZoneList.Equals(zoneList));
                                if (productPriceDetail != null)
                                {
                                    _total = productPriceDetail.PriceEachNguoiLon * numberOfAdut + productPriceDetail.PriceEachTreEm * numberOfChild;
                                    //Tinh khuyen mai va tru di

                                }
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(discountCoupon))
                    {
                        var requestCoupon = new RequestCheckCouponCode();
                        requestCoupon.productId = productParentId;
                        requestCoupon.couponCode = discountCoupon;
                        requestCoupon.customerEmail = request.auth.email;
                        requestCoupon.culture_code = "vi-VN";

                        var resultCoupon = _orderRepository.CheckCouponCode(requestCoupon);
                        if (resultCoupon != null)
                        {
                            var discountOption = resultCoupon.DiscountOption;
                            var discountValue = resultCoupon.ValueDiscount;

                            if (discountOption == 1) // 1 la loai khuyen mai theo phan tram
                            {
                                _total = _total * (100 - discountValue) / 100;
                            }
                            if (discountOption == 2) // 2 la khuuyen mai theo gia tien
                            {
                                _total = _total - discountValue;
                            }
                        }
                    }
                    totalPrice += _total;

                }

                //decimal totalPrice = request.pays.Sum(r => r.totalPrice - r.discountSelected.couponPrice);


                totalPrice = totalPrice * 100;
                var priceInt = (int)totalPrice;
                var result = Onepay.ExcuteGetMethod(orderCode, priceInt.ToString(), returnUrl, customerEmail, requestBody: request);
                return Ok(result);
            }

            return BadRequest(request);
        }
        [HttpGet]
        [Route("ResultPaymentOnePayApp/{i18code}/{merchantEncriped}")]
        public async Task<IActionResult> ResultPaymentOnePayApp(string i18Code, string merchantEncriped)
        {


            var deepLinkUrl = "joytime://payment-result";

            // Get the vpc_TxnResponseCode parameter from the query string
            var txnResponseCode = Request.Query["vpc_TxnResponseCode"].ToString();
            //vpc_MerchTxnRef
            var vpc_MerchTxnRef = Request.Query["vpc_MerchTxnRef"].ToString();
            var baseUrl = _configuration["MainUrl"];
            if (!string.IsNullOrEmpty(txnResponseCode) && !string.IsNullOrEmpty(vpc_MerchTxnRef) && txnResponseCode.Equals("0"))
            {

                var checker = Onepay.Decrypt(merchantEncriped);
                if (checker.Equals(vpc_MerchTxnRef))
                {
                    //Doc vao database lay ra object tu merchantRef
                    using (IDbContext context = new IDbContext())
                    {
                        var paymentDetail = await context.OnepayRefs.FirstOrDefaultAsync(r => r.EncryptMerchantTxtCode.Equals(merchantEncriped));
                        if (paymentDetail != null)
                        {
                            var paymentObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestCreateMultipleItemOrder>(paymentDetail.Object);
                            if(paymentObject != null)
                            {
                                var response = await _CreateMultipleItemOrderManualyForApp(paymentObject);
                                if (response != null)
                                {
                                    return Ok(response);
                                }
                            }
                        }
                    }
                }
            }
            return BadRequest("ERROR ORDER"); // Ở đây sẽ bắt trường hợp lỗi không xác định hoặc log
        }


        [HttpPost]
        [Route("ProcessPaymentPayPalApp")]
        public async Task<IActionResult> ProcessPaymentPayPalApp(RequestCreateMultipleItemOrder request)
        {

            //ExcuteGetMethod(string orderCode, string amount, string returnUrl, string customerEmail = "", string customerPhone = "", string customerId = "")
            if (request != null)
            {
                var clientId = _configuration["PaypalParams:clientId"];
                var clientSecret = _configuration["PaypalParams:clientSecret"];
                var mainAPI = _configuration["MainAPI"];

                long ticks = DateTime.Now.Ticks;
                string vpcMerchantTxnRef = "TEST_" + ticks.ToString();
                var encryptTxnRef = Onepay.Encrypt(vpcMerchantTxnRef);

                var returnUrl = $"{mainAPI}/api/PageOrder/PayPalCallbackAppAsync";
                returnUrl += "/" + encryptTxnRef;

                var payPalClient = new PayPalHttpClient(new SandboxEnvironment(clientId, clientSecret)); // Sử dụng môi trường sandbox

                var orderCode = request.orderCode;

                var customerEmail = request.auth.email;

                //Tinh toan lai gia tien o day
                decimal totalPrice = 0;
                foreach (var pay in request.pays)
                {
                    decimal _total = 0;
                    var choosenDate = pay.choosenDate; // Convert tu dinh dang dd/mm/yyyy => yyyy-mm-dd

                    DateTime parsedDate = DateTime.TryParseExact(pay.choosenDate, new[] { "d/M/yyyy", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : throw new FormatException("Invalid date format");

                    var combinationId = pay.combination.id;
                    var productChildId = pay.productChildId;
                    var productParentId = pay.productId;
                    var discountCoupon = "";
                    var numberOfAdut = pay.numberOfAldut;
                    var numberOfChild = pay.numberOfChildrend;
                    if (pay.discountSelected != null)
                    {
                        if (!string.IsNullOrEmpty(pay.discountSelected.couponCode))
                        {
                            discountCoupon = pay.discountSelected.couponCode;
                        }
                    }
                    //Bat dau goi vao database de lay ra gia thuc te
                    using (IDbContext context = new IDbContext())
                    {
                        var combinationDetail = await context.ProductPriceInZoneList.FindAsync(combinationId);
                        if (combinationDetail != null)
                        {
                            var zoneList = combinationDetail.ZoneList;
                            if (!string.IsNullOrEmpty(zoneList))
                            {
                                var productPriceDetail = await context.ProductPriceInZoneListByDate.FirstOrDefaultAsync(r => r.ProductId == productChildId && r.Date.Date == parsedDate.Date && r.ZoneList.Equals(zoneList));
                                if (productPriceDetail != null)
                                {
                                    _total = productPriceDetail.PriceEachNguoiLon * numberOfAdut + productPriceDetail.PriceEachTreEm * numberOfChild;
                                    //Tinh khuyen mai va tru di

                                }
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(discountCoupon))
                    {
                        var requestCoupon = new RequestCheckCouponCode();
                        requestCoupon.productId = productParentId;
                        requestCoupon.couponCode = discountCoupon;
                        requestCoupon.customerEmail = request.auth.email;
                        requestCoupon.culture_code = "vi-VN";

                        var resultCoupon = _orderRepository.CheckCouponCode(requestCoupon);
                        if (resultCoupon != null)
                        {
                            var discountOption = resultCoupon.DiscountOption;
                            var discountValue = resultCoupon.ValueDiscount;

                            if (discountOption == 1) // 1 la loai khuyen mai theo phan tram
                            {
                                _total = _total * (100 - discountValue) / 100;
                            }
                            if (discountOption == 2) // 2 la khuuyen mai theo gia tien
                            {
                                _total = _total - discountValue;
                            }
                        }
                    }
                    totalPrice += _total;

                }
                try
                {
                    using (IDbContext context = new IDbContext())
                    {
                        OnepayRef _newPaymentOrder = new OnepayRef();
                        _newPaymentOrder.EncryptMerchantTxtCode = encryptTxnRef;
                        _newPaymentOrder.Object = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                        context.OnepayRefs.Add(_newPaymentOrder);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                //decimal totalPrice = request.pays.Sum(r => r.totalPrice - r.discountSelected.couponPrice);


                var usdCulture = Math.Round((totalPrice / 25200), 2);
                var priceInt = (int)totalPrice;

                var order = new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = "USD", // Đổi sang tiền tệ của bạn nếu cần
                            Value = usdCulture.ToString().Replace(",",".")
                        },
                        Description = $"Thanh toán cho đơn hàng {request.orderCode}"
                    }
                },
                    ApplicationContext = new ApplicationContext()
                    {
                        ReturnUrl = returnUrl,
                        CancelUrl = returnUrl
                    }
                };

                // Gửi yêu cầu tạo đơn hàng đến PayPal
                var requestCreateOrder = new OrdersCreateRequest();
                requestCreateOrder.RequestBody(order);

                try
                {
                    var response = await payPalClient.Execute(requestCreateOrder);
                    var resultCreateOrder = response.Result<PayPalCheckoutSdk.Orders.Order>();

                    // Lấy URL thanh toán từ PayPal và chuyển hướng người dùng đến đó
                    foreach (var link in resultCreateOrder.Links)
                    {
                        if (link.Rel.Equals("approve", StringComparison.OrdinalIgnoreCase))
                        {
                            return Ok(link.Href);
                        }
                    }

                    return BadRequest("Không thể tạo thanh toán.");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Lỗi: {ex.Message}");
                }



            }
            return BadRequest(request);
        }

        private async Task<bool> ConfirmPaymentPayPalAsync( string token, string PayerID)
        {
            try
            {
                // Thực hiện các bước cần thiết để xác nhận thanh toán và hoàn tất đơn hàng
                // Điều này có thể bao gồm việc gửi yêu cầu xác nhận đến PayPal và cập nhật trạng thái của đơn hàng trong cơ sở dữ liệu của bạn

                // Ví dụ:
                // Tạo client PayPal
                //var _clientId = "AWgcbC5XqAmSBwBaKFjoOy3OwQWcTTMErnXatyW-ujwavCrOGNI3j0G0ZPzhHT8GoVT_TGr7LkmNwQSt";
                //var _clientSecret = "ENIsfxMqFuYRV56Wo9pbBapxRLh8H7SEmSm1EN34Ol5OTFcaNluQqLDHcaWwRyDbCjfalG1ohBfg__-F";
                var clientId = _configuration["PaypalParams:clientId"];
                var clientSecret = _configuration["PaypalParams:clientSecret"];
                var payPalClient = new PayPalHttpClient(new SandboxEnvironment(clientId, clientSecret));
                var request = new OrdersCaptureRequest(token);
                request.RequestBody(new OrderActionRequest());

                var response = await payPalClient.Execute(request);
                var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                if (result.Status == "COMPLETED")
                {
                    // Đơn hàng đã được thanh toán thành công, cập nhật trạng thái của đơn hàng trong cơ sở dữ liệu của bạn (ví dụ: đánh dấu là đã thanh toán)
                    // Do something...
                    return true;
                }
                else
                {
                    // Thanh toán không thành công, xử lý tùy thuộc vào yêu cầu của bạn
                    return false;
                }

                // Trong ví dụ này, chúng ta chỉ mô phỏng việc xác nhận thanh toán thành công
                // Bạn cần thay thế các dòng mã trên bằng các thao tác cụ thể phù hợp với ứng dụng của bạn
                return true; // Mô phỏng việc xác nhận thanh toán thành công
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ nếu có
                Console.WriteLine($"Lỗi xác nhận thanh toán: {ex.Message}");
                return false;
            }
        }
        [HttpGet]
        [Route("PayPalCallbackAppAsync/{encryptTxnRef}")]
        public async Task<IActionResult> PayPalCallbackAppAsync(string encryptTxnRef , string token, string PayerID)
        {
            var isOrderCreated = false;
            using (IDbContext context = new IDbContext())
            {
                if (!string.IsNullOrEmpty(encryptTxnRef))
                {
                    var paymentDetail = await context.OnepayRefs.FirstOrDefaultAsync(r => r.EncryptMerchantTxtCode.Equals(encryptTxnRef));
                    if (paymentDetail != null)
                    {
                        var paymentObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestCreateMultipleItemOrder>(paymentDetail.Object);

                        if (paymentObject != null)
                        {
                            var query = await context.Orders.Where(r => r.OnepayRef.Equals(paymentObject.orderCode)).FirstOrDefaultAsync();
                            if (query != null)
                            {
                                isOrderCreated = true;
                            }
                            else
                            {
                                isOrderCreated = false;
                            }

                            if(isOrderCreated == false)
                            {
                                // Kiểm tra xem token và PayerID có tồn tại không
                                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(PayerID))
                                {
                                    // Xác nhận thanh toán và hoàn tất đơn hàng
                                    if (await ConfirmPaymentPayPalAsync(token, PayerID))
                                    {
                                        // Nếu thanh toán thành công, chuyển hướng người dùng đến trang "PaymentSuccess" trong controller "Product"
                                        // Tao don hang o day
                                        // Nhuwng van de la lay dau ra don hang???

                                        //Doc vao database lay ra object tu merchantRef
                                        var response = await _CreateMultipleItemOrderManualyForApp(paymentObject);
                                        if (response != null)
                                        {

                                            return Ok(response);
                                        }

                                    }
                                    else
                                    {
                                        // Nếu xác nhận thanh toán không thành công, chuyển hướng người dùng đến trang thông báo lỗi hoặc thực hiện các thao tác khác tùy thuộc vào yêu cầu của bạn
                                        return BadRequest("Loi khi thanh toan"); // Thay thế "Loi" và "Controller" bằng tên controller và action thực tế của bạn
                                    }
                                }
                            }
                            else
                            {
                                //Doc vao database lay ra object tu merchantRef
                                var response = await _CreateMultipleItemOrderManualyForApp(paymentObject);
                                if (response != null)
                                {

                                    return Ok(response);
                                }
                            }
                        }
                    }
                }

            }
            // Nếu không có thông tin token và PayerID, có thể xảy ra lỗi, bạn có thể xử lý lỗi ở đây nếu cần
            return BadRequest("Loi khi thanh toan"); // Thay thế "Loi" và "Controller" bằng tên controller và action thực tế của bạn
        }


        [HttpPost]
        [Route("GetCouponByProductId")]
        public async Task<IActionResult> GetCouponByProductId(RequestGetCouponByProductId request)
        {
            var response = _orderRepository.GetCouponByProductId(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("GenerateOrderCode")]
        public async Task<IActionResult> GenerateOrderCode()
        {
            var response = _orderRepository.GenerateOrderCode();
            return Ok(response);
        }

        [HttpPost]
        [Route("CheckCouponCode")]
        public async Task<IActionResult> CheckCouponCode(RequestCheckCouponCode request)
        {
            var response = _orderRepository.CheckCouponCode(request);
            return Ok(response);
        }
        [HttpGet]
        [Route("CheckOrderPaidByOrderCode/{orderCode}")]
        public async Task<IActionResult> CheckOrderPaidByOrderCode(string orderCode)
        {
            using (IDbContext context = new IDbContext())
            {
                var checker = await context.Orders.FirstOrDefaultAsync(r => r.OnepayRef.Equals(orderCode));
                if (checker != null)
                {
                    var customerId = checker.CustomerId;
                    
                    dynamic response = new ExpandoObject();
                    response.customerId = 0;
                    response.customerEmail = "";
                    if (customerId > 0)
                    {
                        var customer = await context.Customer.FindAsync(customerId);
                        if(customer != null)
                        {
                            
                            response.customerId = customerId;
                            response.customerEmail = customer.Email;
                        }
                        
                    }
                    return Ok(response);
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
