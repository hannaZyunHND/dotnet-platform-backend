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
                    var sendMailStatus = _orderRepository.SendNewUserEmail(requestCreateAccount);

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
                    var sendMailOrders = _orderRepository.SendNewOrderEmailToCustomer(requestCreateOrder);
                    var sendHelpDeskMaiOrder = _orderRepository.SendNewOrderEmailToHelpDesk(requestCreateOrder);
                }

                return response;
            }
            else
            {
                return null;
            }
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


                            //Tao don hang
                            var response = await _CreateMultipleItemOrderManualy(paymentObject);
                            if(response != null)
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
