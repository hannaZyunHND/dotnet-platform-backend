using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using Way2GoWEB.Payment;
using Way2GoWEB.Services.ApiJoyTel.Repository;
using Way2GoWEB.Services.Extra.Repository;
using Way2GoWEB.Services.Locations.Repository;
using Way2GoWEB.Services.Order.Repository;
using Way2GoWEB.Services.Order.ViewModal;
using Way2GoWEB.Services.Product.Repository;
using Way2GoWEB.Services.Promotion.Repository;
using Way2GoWEB.Services.Zone.Repository;
using Way2GoWEB.Utility;

namespace Way2GoWEB.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICookieUtility _cookieUtility;
        private readonly IExtraRepository _extraRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly IHostingEnvironment _env;
        private readonly IApiJoyTelRepository _apiJoyTelRepository;

        public OrderController(IZoneRepository zoneRepository, IProductRepository productRepository, IPromotionRepository promotionRepository, IOrderRepository orderRepository, ICookieUtility cookieUtility, IHostingEnvironment env, IExtraRepository extraRepository, ILocationsRepository locationsRepository, IApiJoyTelRepository apiJoyTelRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _promotionRepository = promotionRepository;
            _orderRepository = orderRepository;
            _cookieUtility = cookieUtility;
            _env = env;
            _extraRepository = extraRepository;
            _locationsRepository = locationsRepository;
            _apiJoyTelRepository = apiJoyTelRepository;
        }
        public IActionResult Orders()
        {
            //Lay danh sach san pham theo list product_id
            //var current_location_id = int.Parse(Request.Cookies[CookieLocationId]);
            //var total = 0;
            //var model = _productRepository.GetProductInListProductsMinify(product_ids, current_location_id, CurrentLanguageCode, 1, 10, out total);
            //var list_promotion_item = new List<int>();

            ////v2: Lay tat ca khuyen mai, sau nay co the cache cai nay
            //var promotions = _promotionRepository.GetAllPromotions(CurrentLanguageCode);
            //ViewBag.Promotions = promotions;
            return View();
        }
        [HttpPost]
        public IActionResult GetQuanHuyen(string locationType, string parent)
        {
            var provinces_result = new Dictionary<string, QuanHuyen>();
            var provinces_json_part = _env.WebRootFileProvider.GetFileInfo("hanhchinhvn-master/dist/" + locationType + ".json")?.PhysicalPath;
            if (provinces_json_part != null)
            {
                var file_content = System.IO.File.ReadAllText(provinces_json_part);
                if (file_content != null)
                {
                    provinces_result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, QuanHuyen>>(file_content);

                    var result = provinces_result.Where(r => r.Value.parent_code.Equals(parent));
                    var result_cooked = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    return Ok(result_cooked);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderViewModel order)
        {
            var result = _orderRepository.CreateOrderInWebsite(order);

            //send message Telegram
            if (result > 0)
            {
                try
                {
                    string token = "5080830709:AAEIjgTv2NOlF4x00eepmFxTgLSO428Xk-U";
                    string chatId = "-634473536";
                    string text = string.Empty;
                    text = "Mã số đơn hàng: " + order.OrderCode + "%0A";
                    text += "Ngày đặt hàng: " + DateTime.Now.ToString("dd/MM/yyyy") + "%0A";
                    text += "------------------------%0A%0A";

                    text += "Thông tin khách hàng%0A";
                    text += "Anh/ Chị: " + order.Customer.Name + "%0A";
                    text += "SĐT: " + order.Customer.PhoneNumber + "%0A";
                    text += "Địa chỉ: " + order.Customer.Address + "%0A";
                    text += order.Customer.Note != null ? "Ghi chú: " + order.Customer.Note + "%0A" : "";
                    text += order.Extras != null && order.Extras.Any() ? "Thông tin khác: " + string.Join(",", order.Extras.Select(x => x)) + "%0A%0A" : "không";
                    text += "Thông tin đơn hàng%0A";
                    decimal total = 0;
                    foreach (var o in order.Products.Select((value, index) => new { value, index }))
                    {
                        text += "     " + (Convert.ToInt16(o.index) + 1) + ". ";
                        text += String.Format("{0} (SL: {1}, Giá bán: {2}, Tổng tiền: {3}, KM: {4})",
                                                    o.value.Name,
                                                    o.value.Quantity,
                                                    UIHelper.FormatNumber(o.value.LogPrice),
                                                    UIHelper.FormatNumber((o.value.LogPrice * Convert.ToDecimal(o.value.Quantity))),
                                                    o.value.Promotions != null && o.value.Promotions.Any() ? string.Join(",", o.value.Promotions.Select(x => x.LogName)) : "không"
                                                    ) + "%0A";
                        total += o.value.LogPrice * Convert.ToDecimal(o.value.Quantity);
                    }
                    text += "------------------------%0A%0A";
                    text += order.CodePromotion != null ? "Mã voucher được áp dụng: " + order.CodePromotion + "%0A" : "";
                    text += order.ValuePromotion != null ? "Trị giá voucher: " + order.ValuePromotion + "%0A" : "";
                    text += "Số tiền cần thanh toán: " + UIHelper.FormatNumber(total) + " vnd %0A";


                    string url = string.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=markdown", token, chatId, text);
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://api.telegram.org/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.GetAsync(url);
                    }
                }
                catch (Exception ex)
                {
                    return Ok(result);
                }
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateOrderStringtify(string order)
        {
            var serialized = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderViewModel>(order);
            var result = _orderRepository.CreateOrderInWebsite(serialized);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetOrderByCode(string orderCode)
        {
            //var result = _orderRepository.GetOrderByCode(orderCode);
            return ViewComponent("GetOrderByCode", new { orderCode = orderCode });
        }
        [HttpPost]
        public IActionResult LoadOrderDetail(string product_ids)
        {
            return ViewComponent("OrderDetail", new { product_ids = product_ids });

        }
        [HttpPost]
        public IActionResult LoadOrderCombo(string product_ids)
        {
            return ViewComponent("OrderCombo", new { product_ids = product_ids });

        }
        [HttpPost]
        public IActionResult LoadOrderDetailJson(string product_ids)
        {
            var total = 0;
            var result = _productRepository.GetProductInListProductsMinify(product_ids, _cookieUtility.SetCookieDefault().LocationId, CurrentLanguageCode, 1, 1000, out total);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult LoadDropdonwCart(string product_ids)
        {
            return ViewComponent("DropdownCart", new { product_ids = product_ids });

        }
        [HttpPost]
        public IActionResult GetOrderCode()
        {
            var result = _orderRepository.GetOrderCode();
            return Ok(result + 1);
            //return ViewComponent("DropdownCart", new { product_ids = product_ids });
        }
        [HttpGet]
        public IActionResult GetCouPonByCode(string code, int productId)
        {
            var result = _orderRepository.GetCouponChildByCode_v2(code, productId);
            return Ok(result);
        }

        public IActionResult KiemTraDonHang()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrderVersionMinify(OrderVersionMinifyRequest request)
        {

            
            var response = _orderRepository.CreateOrderVersionMinify(request);
            if(response.customerId > 0)
            {
                
                //Send mail confirm;
                try
                {
                    if (response.isCreatedAccount == 1 || request.isCreatedAccount)
                    {
                        _extraRepository.SendEmailRegister(new CustomerAuthViewModel() { email = request.email, password = request.customerRandomId });
                    }
                    var mailConfirmRequest = new SuccessEmailRequest();
                    mailConfirmRequest.email = request.email;
                    mailConfirmRequest.orderCode = request.orderCode;
                    mailConfirmRequest.productName = response.productName;
                    mailConfirmRequest.pickupPoint = request.orderNote;
                    //mailConfirmRequest.iccid = response.iccid;

                    _extraRepository.SendEmailSuccessOrder(mailConfirmRequest);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message); 
                   
                }

                return Ok(response.customerId);
            }
            if (!string.IsNullOrEmpty(request.orderCode))
            {

            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderVersionMinifyV2(OrderVersionMinifyRequest request)
        {
            var response = _orderRepository.CreateOrderMultipleVersionMinify(request);
            if (response.customerId > 0)
            {
                //kiem tra discount
                if (!string.IsNullOrEmpty(request.discountCode))
                {
                    bool isActiveDiscount = false;
                    if(request.productsInfo != null)
                    {
                        foreach(var item in request.productsInfo)
                        {
                            if (!isActiveDiscount)
                            {
                                var code = _orderRepository.GetCouponChildByCode_v2(request.discountCode, item.productId, true);
                                if (code != null)
                                {
                                    isActiveDiscount = true;
                                }
                            }
                            
                        }
                    }
                    
                }
                //Send mail confirm;
                try
                {
                    if (response.isCreatedAccount == 1)
                    {
                        _extraRepository.SendEmailRegister_v2(new CustomerAuthViewModel() { email = request.email, password = request.customerRandomId }, CurrentLanguageCode);
                    }
                    var mailConfirmRequest = new SuccessEmailRequest();
                    mailConfirmRequest.email = request.email;
                    mailConfirmRequest.orderCode = response.orderCode;
                    mailConfirmRequest.productName = response.productName;
                    mailConfirmRequest.pickupPoint = request.orderNote;
                    mailConfirmRequest.iccid = response.iccid;
                    mailConfirmRequest.productsInfo = request.productsInfo;

                    // Gui Email don hang thanh cong
                    _extraRepository.SendEmailSuccessOrder_v2(mailConfirmRequest, CurrentLanguageCode);
                    //Lay danh sach san pham phu thuoc vao orderCode
                    var orderDetails = _orderRepository.GetOrderByCode(response.orderCode);
                    if (orderDetails != null)
                    {
                        foreach (var item in request.productsInfo)
                        {
                            //Kich hoat SIM o day
                            if (item.type.Equals("TOPUP"))
                            {
                                var productId = item.productId;
                                var productDetail = _productRepository.GetProductInfomationDetail(productId, CurrentLanguageCode);
                                if (productDetail != null)
                                {
                                    var iccid = request.serialNumber;
                                    var days = int.Parse(productDetail.Validity);
                                    var joytelCode = productDetail.joytelProductCode;
                                    var isMultipleTime = true;
                                    if (!productDetail.DataLimit.Contains("/"))
                                    {
                                        isMultipleTime = false;
                                    }
                                    // Call o day
                                    var resultOTA = await _apiJoyTelRepository.RequestOTA(iccid, joytelCode, days, isMultipleTime);
                                    if (resultOTA.code == 0)
                                    {
                                        //Cap nhat lai Order
                                        var orderChanged = _orderRepository.UpdateTopupStatus(response.orderCode, productDetail.Id, resultOTA.data.orderTid);

                                    }
                                }
                            }
                            if (item.type.Equals("ESIM"))
                            {

                                var productId = item.productId;
                                var productDetail = _productRepository.GetProductInfomationDetail(productId, CurrentLanguageCode);
                                if (productDetail != null)
                                {
                                    var affected = orderDetails.Where(r => r.Type.Equals("ESIM") && string.IsNullOrEmpty(r.JoyTelOrderTid) && r.ProductId == item.productId).FirstOrDefault();
                                    if(affected != null)
                                    {
                                        var esimSubmitResponse = await _apiJoyTelRepository.SendEsimRequestSubmit(request.email, productDetail.joytelProductCode, item.quantity);
                                        if (esimSubmitResponse != null)
                                        {
                                            if (esimSubmitResponse.code == 0)
                                            {
                                                var orderTid = esimSubmitResponse.data.orderTid;
                                                var orderCode = esimSubmitResponse.data.orderCode;
                                                Thread.Sleep(30000);
                                                //Goi tiep den eSimQuery
                                                var esimQueryResponse = await _apiJoyTelRepository.SendEsimQueryRequest(orderCode, orderTid);
                                                if (esimQueryResponse != null)
                                                {
                                                    if(esimQueryResponse.code == 0)
                                                    {
                                                        if(esimQueryResponse.data != null)
                                                        {
                                                            //Lay ra snPin
                                                            var itemList = esimQueryResponse.data.itemList.FirstOrDefault();
                                                            if (itemList != null)
                                                            {
                                                                var sn = itemList.snList.FirstOrDefault();
                                                                if(sn != null)
                                                                {
                                                                    var snPin = sn.snPin;
                                                                    var snSerial = sn.snCode;
                                                                    // Ghi du lieu vao database
                                                                    var updatedResult = _orderRepository.UpdateOrderInfomationToEsim(orderTid, orderCode, snPin, snSerial, affected.Id);
                                                                    if (updatedResult)
                                                                    {
                                                                        //Thread.Sleep(10000);
                                                                        var esimGetQrResponse = await _apiJoyTelRepository.SendEsimGetQrCode(snPin);
                                                                    }
                                                                    
                                                                }

                                                            }
                                                        }
                                                        
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    

                                }
                            }
                        } 
                    }
                    return Ok(response.customerId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> ProcessPaypal(OrderVersionMinifyRequest rq)
        {
            // Tạo client PayPal
            //var _clientId = "AWgcbC5XqAmSBwBaKFjoOy3OwQWcTTMErnXatyW-ujwavCrOGNI3j0G0ZPzhHT8GoVT_TGr7LkmNwQSt";
            //var _clientSecret = "ENIsfxMqFuYRV56Wo9pbBapxRLh8H7SEmSm1EN34Ol5OTFcaNluQqLDHcaWwRyDbCjfalG1ohBfg__-F";
            var _clientId = "ATgWSOTnwYc-BCysxxDbX7c2JMjtVBdACrPd-J_aojA8mtByaNIlHnBwPDakTUdWuoOIzNgRPbPsXgTe";
            var _clientSecret = "EKQ6axpy_JQnEa_vchD2kWDAUQryZosdJxgqEdZRhZFGH53mgDWciBms4wXz2Prfb9tCSV14QwQ36Clv";
            var payPalClient = new PayPalHttpClient(new LiveEnvironment(_clientId, _clientSecret)); // Sử dụng môi trường sandbox
            //Khong ho tro thanh toan bang VND
            decimal totalPrice = 0;
            bool isActiveDiscount = false;
            foreach (var item in rq.productsInfo)
            {
                var prod = _productRepository.GetProductInfomationDetail(item.productId, CurrentLanguageCode);
                if (prod != null)
                {
                    totalPrice = prod.Price * item.quantity;
                }
                if (!string.IsNullOrEmpty(rq.discountCode))
                {
                    if (!isActiveDiscount)
                    {
                        var code = _orderRepository.GetCouponChildByCode_v2(rq.discountCode, prod.Id);
                        if (code != null)
                        {
                            decimal discountValue = 0;
                            decimal.TryParse(code.ValueDiscount, out discountValue);
                            if (discountValue > 0)
                            {
                                if (code.DiscountOption == 1)
                                {
                                    totalPrice = totalPrice - totalPrice * discountValue / 100;
                                    isActiveDiscount = true;
                                }
                            }

                        }
                    }
                    
                }
            }
            

            if (!string.IsNullOrEmpty(rq.serialNumber))
            {
                //Kiem tra serial number
                var serial = _orderRepository.CheckSerialNumber(rq.serialNumber);
                if (serial != null)
                {
                    totalPrice = totalPrice - 15000;
                }
            }
            var usdCulture = Math.Round((totalPrice / 25200),2);
            // Tạo đơn hàng
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
                        Description = $"Thanh toán cho sản phẩm có ID: {rq.productId}"
                    }
                },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = rq.returnUrl,
                    CancelUrl = rq.returnUrl
                }
            };

            // Gửi yêu cầu tạo đơn hàng đến PayPal
            var request = new OrdersCreateRequest();
            request.RequestBody(order);

            try
            {
                var response = await payPalClient.Execute(request);
                var result = response.Result<Order>();

                // Lấy URL thanh toán từ PayPal và chuyển hướng người dùng đến đó
                foreach (var link in result.Links)
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


        [HttpPost]
        public IActionResult ProcessOnePay(OrderVersionMinifyRequest rq)
        {
            //ExcuteGetMethod(string orderCode, string amount, string returnUrl, string customerEmail = "", string customerPhone = "", string customerId = "")
            var returnUrl = rq.returnUrl;
            var totalAmount = rq.totalPrice * 100;
            var amount = totalAmount.ToString();
            var orderCode = rq.orderCode;
            var customerEmail = rq.email;
            var customerPhone = "";
            var customerId = rq.customerRandomId;
            decimal totalPrice = 0;

            bool isActiveDiscount = false;
            foreach (var item in rq.productsInfo)
            {
                var prod = _productRepository.GetProductInfomationDetail(item.productId, CurrentLanguageCode);
                if (prod != null)
                {
                    totalPrice = prod.Price * item.quantity;
                }
                if (!string.IsNullOrEmpty(rq.discountCode))
                {
                    if (!isActiveDiscount)
                    {
                        var code = _orderRepository.GetCouponChildByCode_v2(rq.discountCode, prod.Id);
                        if (code != null)
                        {
                            decimal discountValue = 0;
                            decimal.TryParse(code.ValueDiscount, out discountValue);
                            if (discountValue > 0)
                            {
                                if (code.DiscountOption == 1)
                                {
                                    totalPrice = totalPrice - totalPrice * discountValue / 100;
                                    isActiveDiscount = true;
                                }
                            }

                        }
                    }

                }
            }

            
            if (!string.IsNullOrEmpty(rq.serialNumber))
            {
                //Kiem tra serial number
                var serial = _orderRepository.CheckSerialNumber(rq.serialNumber);
                if(serial != null)
                {
                    totalPrice = totalPrice - 15000;
                }
            }
            totalPrice = totalPrice * 100;
            var result = Onepay.ExcuteGetMethod(orderCode, totalPrice.ToString(), returnUrl, customerEmail);
            return Ok(result);

        }
        [HttpPost]
        public IActionResult ConfirmOnepay(RequestConfirmOnepay request)
        {
            var response = false;
            if(request != null)
            {
                var decrypedCode = Onepay.Decrypt(request.encrypedCode);
                if (decrypedCode.Equals(request.transactionId))
                {
                    response = true;
                }
            }
            
            return Ok(response);
        }
        // Hàm xác nhận thanh toán và hoàn tất đơn hàng
        private async Task<bool> ConfirmPaymentPayPalAsync(string token, string PayerID)
        {
            try
            {
                // Thực hiện các bước cần thiết để xác nhận thanh toán và hoàn tất đơn hàng
                // Điều này có thể bao gồm việc gửi yêu cầu xác nhận đến PayPal và cập nhật trạng thái của đơn hàng trong cơ sở dữ liệu của bạn

                // Ví dụ:
                // Tạo client PayPal
                //var _clientId = "AWgcbC5XqAmSBwBaKFjoOy3OwQWcTTMErnXatyW-ujwavCrOGNI3j0G0ZPzhHT8GoVT_TGr7LkmNwQSt";
                //var _clientSecret = "ENIsfxMqFuYRV56Wo9pbBapxRLh8H7SEmSm1EN34Ol5OTFcaNluQqLDHcaWwRyDbCjfalG1ohBfg__-F";
                var _clientId = "ATgWSOTnwYc-BCysxxDbX7c2JMjtVBdACrPd-J_aojA8mtByaNIlHnBwPDakTUdWuoOIzNgRPbPsXgTe";
                var _clientSecret = "EKQ6axpy_JQnEa_vchD2kWDAUQryZosdJxgqEdZRhZFGH53mgDWciBms4wXz2Prfb9tCSV14QwQ36Clv";
                var payPalClient = new PayPalHttpClient(new LiveEnvironment(_clientId, _clientSecret));
                var request = new OrdersCaptureRequest(token);
                request.RequestBody(new OrderActionRequest());

                var response = await payPalClient.Execute(request);
                var result = response.Result<Order>();

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
        public async Task<IActionResult> PayPalCallbackAsync(string token, string PayerID)
        {
            // Kiểm tra xem token và PayerID có tồn tại không
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(PayerID))
            {
                // Xác nhận thanh toán và hoàn tất đơn hàng
                if (await ConfirmPaymentPayPalAsync(token, PayerID))
                {
                    // Nếu thanh toán thành công, chuyển hướng người dùng đến trang "PaymentSuccess" trong controller "Product"
                    return Ok("THANH TOAN THANH CONG!");
                }
                else
                {
                    // Nếu xác nhận thanh toán không thành công, chuyển hướng người dùng đến trang thông báo lỗi hoặc thực hiện các thao tác khác tùy thuộc vào yêu cầu của bạn
                    return BadRequest("Loi khi thanh toan"); // Thay thế "Loi" và "Controller" bằng tên controller và action thực tế của bạn
                }
            }

            // Nếu không có thông tin token và PayerID, có thể xảy ra lỗi, bạn có thể xử lý lỗi ở đây nếu cần
            return BadRequest("Loi khi thanh toan"); // Thay thế "Loi" và "Controller" bằng tên controller và action thực tế của bạn
        }

        public IActionResult OrderComplete()
        {
            return View();
        }

        public IActionResult CheckSerialNumber(string serialNumber)
        {
            var result = _orderRepository.CheckSerialNumber(serialNumber);
            return Ok(result);
        }

        public IActionResult GetLocations()
        {
            var result = _locationsRepository.GetLocations(CurrentLanguageCode);
            return Ok(result);
        }

        [HttpGet]
        [Route("SuccessOrder/{orderCode}")]
        public IActionResult SuccessOrder(string orderCode)
        {
            var orderDetails = _orderRepository.GetOrderByCode(orderCode);
            var listProduct = orderDetails.Select(r => r.ProductId);
            return View(orderDetails);
        }

        [HttpGet]
        public IActionResult RemoveOrder(string orderCode)
        {
            var result = _orderRepository.RemoveOrder(orderCode);
            return Ok("Remove Success!");
        }


        [HttpGet]
        public IActionResult GetOrderDetail(string orderCode)
        {
            var result = _orderRepository.GetOrderByCode(orderCode);
            return Ok(result);
        }

        

    }
}