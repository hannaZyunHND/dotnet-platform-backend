using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayPalHttp;
using PlatformWEBAPI.Services.ApiJoyTel.Repository;
using PlatformWEBAPI.Services.ApiJoyTel.ViewModel;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Product.Repository;
using Serilog.Context;
using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    public class ApiJoyTelController : Controller
    {
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IApiJoyTelRepository _apiJoyTelRepository;
        private IHostingEnvironment _hostingEnvironment;
        private IExtraRepository _extraRepository;
        public ApiJoyTelController(IProductRepository productRepository, IOrderRepository orderRepository, IApiJoyTelRepository apiJoyTelRepository, IHostingEnvironment hostingEnvironment, IExtraRepository extraRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _apiJoyTelRepository = apiJoyTelRepository;
            _hostingEnvironment = hostingEnvironment;
            _extraRepository = extraRepository;
        }


        [Route("/api/EsimCallback/order")]
        public async Task<IActionResult> GetEsimOrderCallback()
        {
            return Ok("callback successful!");
        }

        [Route("/api/EsimCallback/notify/coupon/redeem")]
        public async Task<IActionResult> GetEsimOrderRedeemCallback()
        {
            var url = Request.GetDisplayUrl();

            // Đảm bảo body có thể đọc nhiều lần
            HttpContext.Request.EnableRewind();

            // Đọc body của request
            var body = "";
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true, bufferSize: 1024))
            {
                body = await reader.ReadToEndAsync();
                Request.Body.Position = 0;
            }
            //Ghi log
            string logFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "log.txt");
            string logContent = $"URL: {url}\nBody: {body}\n";
            await System.IO.File.AppendAllTextAsync(logFilePath, logContent + System.Environment.NewLine);

            try
            {
                var responseQR = JsonConvert.DeserializeObject<EsimQrCodeResponse>(body);
                //Ghi nho vao database
                if (responseQR != null)
                {
                    var responseQrData = responseQR.data;
                    if (responseQrData != null)
                    {
                        var coupon = responseQrData.coupon;
                        var qrCode = responseQrData.qrcode;

                        var savedOrderDetail = _orderRepository.UpdateImageQrCode(coupon, qrCode);
                        if (savedOrderDetail != null)
                        {
                            // Gui Email cho 
                            var orderDetails = _orderRepository.GetOrderByCode(savedOrderDetail.orderCode);
                            if (orderDetails != null)
                            {
                                var affected = orderDetails.Where(r => r.JoyTelOrderCoupon == coupon).FirstOrDefault();
                                if (affected != null)
                                {
                                    //SEND MAIl
                                    var productDetail = _productRepository.GetProductInfomationDetail(affected.ProductId, "en-US");
                                    _extraRepository.SendEmailEsimQRCode(productDetail.Title, qrCode, affected.CustomerEmail);
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await System.IO.File.AppendAllTextAsync(logFilePath, ex.Message + System.Environment.NewLine);
                //throw;
            }


            //// Tạo đường dẫn đến file log trong wwwroot
            //string logFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "log.txt");
            //// Ghi URL vào file log
            //await System.IO.File.AppendAllTextAsync(logFilePath, body + System.Environment.NewLine);

            dynamic response = new ExpandoObject();
            response.code = "000";
            response.mesg = "Success";
            return Ok(response);
        }

        public async Task<IActionResult> SendEsimRequestSubmit(string productCode, string quantity)
        {
            var customerCode = "10383";
            var customerAuth = "554fC7CA55";
            var orderTid = string.Format("{0}_{1}", customerCode, DateTime.UtcNow.Ticks);
            var warehouse = "";
            var type = 3;
            var receiveName = "Consortio Vietnam";
            var phone = "+84965951180";
            var timeStamp = DateTime.UtcNow.Ticks;
            var email = "huy.bc@aptechlearning.edu.vn";
            var replyType = 1;
            var itemListString = JsonConvert.SerializeObject(new[] {
                new {productCode = productCode, quantity = quantity}
            });
            var autoGraph = CalculateSHA1($"{customerCode}{customerAuth}{type}{orderTid}{receiveName}{phone}{timeStamp}{productCode}{quantity}");
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var requestUri = "https://api.joytelshop.com/customerApi/customerOrder";
                var json = JsonConvert.SerializeObject(new
                {
                    customerCode = customerCode,
                    orderTid = orderTid,
                    type = type,
                    warehouse = warehouse,
                    receiveName = receiveName,
                    phone = phone,
                    timestamp = timeStamp,
                    autoGraph = autoGraph,
                    email = email,
                    replyType = replyType,
                    itemList = new[]
                    {
                        new { productCode = productCode, quantity = quantity }
                    }
                });
                Console.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content.ToString());

                var response = await httpClient.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // Xử lý phản hồi từ JoyTel ở đây
                    Console.WriteLine(responseContent);
                    return Ok(responseContent);
                }
                else
                {
                    Console.WriteLine(response);
                    return BadRequest(response);
                    // Xử lý lỗi ở đây
                }

            }
        }

        public async Task<IActionResult> SendEsimQueryRequest(string orderCode, string orderTid)
        {
            var customerCode = "10383";
            var customerAuth = "554fC7CA55";
            var timeStamp = DateTime.UtcNow.Ticks;
            var autoGraph = CalculateSHA1($"{customerCode}{customerAuth}{orderCode}{orderTid}{timeStamp}");
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var requestUri = "https://api.joytelshop.com/customerApi/customerOrder/query";
                var json = JsonConvert.SerializeObject(new
                {
                    customerCode = customerCode,
                    orderTid = orderTid,
                    orderCode = orderCode,
                    timestamp = timeStamp,
                    autoGraph = autoGraph
                });
                Console.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content.ToString());

                var response = await httpClient.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // Xử lý phản hồi từ JoyTel ở đây
                    Console.WriteLine(responseContent);
                    return Ok(responseContent);
                }
                else
                {
                    Console.WriteLine(response);
                    return BadRequest(response);
                    // Xử lý lỗi ở đây
                }
            }
        }

        public async Task<IActionResult> SendOTARecharge(string currentIccid, string newIccid, int orderId, int productId, string orderCode = "")
        {
            var productInfo = _productRepository.GetProductInfomationDetail(productId, "en-US");
            //Send Request OTA
            var resultOTA = await _apiJoyTelRepository.RequestOTA(newIccid, productInfo.joytelProductCode, int.Parse(productInfo.Validity));
            if (resultOTA != null)
            {
                if (resultOTA.code == 0)
                {
                    //Cap nhat lai Order
                    var orderChanged = _orderRepository.UpdateOrderIccid(orderId, currentIccid, newIccid, resultOTA.data.orderTid);
                    if (orderChanged == true)
                    {
                        //SEND MAIL VAO DAY
                        _extraRepository.SendEmailUpdateIccid(currentIccid, newIccid, orderId, productId, orderCode);
                        return Ok("SUCCESS!");
                    }
                }
            }
            return BadRequest("Error while active SIM");
        }

        private string CalculateSHA1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // format as hexadecimal
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString().ToLower();
            }
        }

    }
}
