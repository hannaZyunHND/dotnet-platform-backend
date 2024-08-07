using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Order.ViewModal;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Utility;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Way2GoWEB.Payment;

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

        [HttpPost]
        [Route("CreateMultipleItemOrderAsync")]
        public async Task<IActionResult> CreateMultipleItemOrderAsync(RequestCreateMultipleItemOrder reqest)
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
            if(response != null)
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
                    var sendMailOrders = _orderRepository.SendNewOrderEmailToCustomer(requestCreateOrder);
                    var sendHelpDeskMaiOrder = _orderRepository.SendNewOrderEmailToHelpDesk(requestCreateOrder);
                }
                
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
            if(request != null)
            {
                var mainAPI = _configuration["MainAPI"];
                var returnUrl = $"{mainAPI}/api/PageOrder/ResultPaymentOnePay";
                returnUrl += "/" + request.i18Code;
                


                var orderCode = request.orderCode;
                
                var customerEmail = request.auth.email;

                decimal totalPrice = request.pays.Sum(r => r.totalPrice - r.discountSelected.couponPrice);
                
                
                totalPrice = totalPrice * 100;
                var result = Onepay.ExcuteGetMethod(orderCode, totalPrice.ToString(), returnUrl, customerEmail);
                return Ok(result);
            }
            
            return Ok(request);
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
            if(!string.IsNullOrEmpty(txnResponseCode) && !string.IsNullOrEmpty(vpc_MerchTxnRef) && txnResponseCode.Equals("0"))
            {
                
                var checker = Onepay.Decrypt(merchantEncriped);
                if (checker.Equals(vpc_MerchTxnRef))
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

        
    }
}
