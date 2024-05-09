using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlatformWEB.Services.ApiJoyTel.ViewModel;
using System;
using System.Net.Http;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PlatformWEB.Services.ApiJoyTel.Repository
{
    public interface IApiJoyTelRepository
    {
        Task<OTARechargeResponse> RequestOTA(string iccid, string productCode, int days);
        Task<ESimSubmitResponse> SendEsimRequestSubmit(string customerEmail, string productCode, int quantity);
        Task<EsimOrderQueryResponse> SendEsimQueryRequest(string orderCode, string orderTid);
        Task<EsimRequestGetQrcodeResponse> SendEsimGetQrCode(string couponCode);
    }
    public class ApiJoyTelRepository : IApiJoyTelRepository
    {
        public async Task<OTARechargeResponse> RequestOTA(string iccid, string productCode, int days)
        {
            //for testing
            days = 1;
            var customerCode = "10383";
            var customerAuth = "554fC7CA55";
            var orderTid = $"{customerCode}_{DateTime.UtcNow.Ticks}";
            var timeStamp = DateTime.UtcNow.Ticks;
            var itemListString = JsonConvert.SerializeObject(new[]
            {
                new { productCode = productCode, snCode = iccid, days = days } // Giả sử sử dụng 30 ngày
            });
            Console.WriteLine(itemListString);

            var autoGraph = CalculateSHA1($"{customerCode}{customerAuth}{timeStamp}{productCode}{iccid}{days}{orderTid}");
            using (var httpClient = new HttpClient())
            {
                var requestUri = "https://api.joytelshop.com/joyRechargeApi/rechargeOrder";
                var json = JsonConvert.SerializeObject(new
                {
                    customerCode = customerCode,
                    orderTid = orderTid,
                    timestamp = timeStamp,
                    autoGraph = autoGraph,
                    itemList = new[]
                    {
                        new { productCode = productCode, snCode = iccid, days = days }
                    }
                });
                Console.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content.ToString());

                var response = await httpClient.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var _r = JsonConvert.DeserializeObject<OTARechargeResponse>(responseContent);
                    return _r;


                }
                else
                {
                    Console.WriteLine(response);
                    return null;
                    // Xử lý lỗi ở đây
                }

            }
        }

        public async Task<ESimSubmitResponse> SendEsimRequestSubmit(string customerEmail, string productCode, int quantity)
        {
            var customerCode = "10383";
            var customerAuth = "554fC7CA55";
            var orderTid = string.Format("{0}_{1}", customerCode, DateTime.UtcNow.Ticks);
            var warehouse = "";
            var type = 3;
            var receiveName = "Consortio Vietnam";
            var phone = "+84965951180";
            var timeStamp = DateTime.UtcNow.Ticks;
            var email = customerEmail;
            var replyType = 1;
            var itemListString = JsonConvert.SerializeObject(new[] {
                new {productCode = productCode, quantity = quantity}
            });
            var autoGraph = CalculateSHA1($"{customerCode}{customerAuth}{type}{orderTid}{receiveName}{phone}{timeStamp}{productCode}{quantity}");
            using (var httpClient = new HttpClient())
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
                    var _r = JsonConvert.DeserializeObject<ESimSubmitResponse>(responseContent);
                    return _r;
                }
                else
                {
                    Console.WriteLine(response);
                    return null;
                    // Xử lý lỗi ở đây
                }

            }
        }

        public async Task<EsimOrderQueryResponse> SendEsimQueryRequest(string orderCode, string orderTid)
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
                    return JsonConvert.DeserializeObject<EsimOrderQueryResponse>(responseContent);
                }
                else
                {
                    Console.WriteLine(response);
                    return null;
                    // Xử lý lỗi ở đây
                }
            }
        }

        public async Task<EsimRequestGetQrcodeResponse> SendEsimGetQrCode(string couponCode)
        {
            var AppId = "iqBfQSQim4MJ";
            var AppSecret = "C68A62072EB545E490B30EFCB25FDA2D";
            var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var TransId = Guid.NewGuid().ToString();
            var cipherText = ComputeMD5Hash($"{AppId}{TransId}{timeStamp}{AppSecret}");
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var requestUri = "https://esim.joytelecom.com/openapi/coupon/redeem";

                // Them request header vao httpClient
                // Thêm header vào HttpClient
                httpClient.DefaultRequestHeaders.Add("AppId", AppId);
                httpClient.DefaultRequestHeaders.Add("Timestamp", timeStamp.ToString());
                httpClient.DefaultRequestHeaders.Add("TransId", TransId);
                httpClient.DefaultRequestHeaders.Add("Ciphertext", cipherText);

                var json = JsonConvert.SerializeObject(new
                {
                    coupon = couponCode,
                    qrcodeType = 0
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
                    return JsonConvert.DeserializeObject<EsimRequestGetQrcodeResponse>(responseContent);
                }
                else
                {
                    Console.WriteLine(response);
                    return null;
                    // Xử lý lỗi ở đây
                }
            }


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
        private string ComputeMD5Hash(string input)
        {
            // Sử dụng MD5
            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi nhập vào thành mảng byte
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                // Tính toán hash
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte đã hash thành chuỗi hex
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
