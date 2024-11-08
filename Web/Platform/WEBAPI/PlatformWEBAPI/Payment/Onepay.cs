using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Nest;
using System.IO;
using PlatformWEBAPI.Services.Order.ViewModal;
using MI.Dal.IDbContext;
using MI.Entity.Models;

namespace Way2GoWEB.Payment
{

    public class ResponseOnepay
    {
        public string returnUrl { get; set; }
        public string merchantTxnRefCrypted { get; set; }
    }
    public static class Onepay
    {
        private static readonly string key = "93a8b6c4d2e1f08a45c9d6e3b7f2a1d3";
        private static readonly string iv = "b1e2d3c4a5f6b7c8";

        //public static string merchantId = "TESTONEPAY";
        public static string merchantId = "OP_EVISACS2";

        //public static string accessCode = "6BEB2546";
        public static string accessCode = "B2106B90";

        //public static string merchantHashCode = "6D0870CDE5F24F34F3915FB0045120DB";
        public static string merchantHashCode = "4D68420A9B98479E9A2CF73A0FC9DF08";

        //public static string BASE_URL = "https://mtf.onepay.vn/paygate/vpcpay.op";
        public static string BASE_URL = "https://onepay.vn/paygate/vpcpay.op";



        public static string MerchantSendRequestStatic()
        {
            Dictionary<string, string> merchantParams = new Dictionary<string, string>
            {
                { "vpc_Version", "2" },
                { "vpc_Currency", "VND" },
                { "vpc_Command", "pay" },
                { "vpc_AccessCode", "6BEB2546" },
                { "vpc_Merchant", "TESTONEPAY" },
                { "vpc_Locale", "vn" },
                { "vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/" },
                { "vpc_MerchTxnRef", "TEST_1698291680035" },
                { "vpc_OrderInfo", "Ma Don Hang" },
                { "vpc_Amount", "10000000" },
                { "vpc_TicketNo", "192.168.166.149" },
                { "vpc_CardList", "VC" },
                { "AgainLink", "https://mtf.onepay.vn/client/qt/" },
                { "Title", "PHP VPC 3-Party" },
                { "vpc_Customer_Phone", "84987654321" },
                { "vpc_Customer_Email", "test@onepay.vn" },
                { "vpc_Customer_Id", "test" }
            };
            Dictionary<string, string> dictSorted = SortParamsMap(merchantParams);
            var stringToHash = GenerateStringToHash(dictSorted);
            Console.WriteLine("merchant's string to hash: " + stringToHash);
            var sign = GenSecurehash(stringToHash, merchantHashCode);
            merchantParams.Add("vpc_SecureHash", sign);
            var queryParam = "";
            foreach (var items in merchantParams)
            {
                var key = items.Key;
                var value = items.Value;
                queryParam += key + "=" + HttpUtility.UrlEncode(value) + "&";
            }
            var requetsUrl = BASE_URL + "?" + queryParam;
            return requetsUrl;
        }

        public static ResponseOnepay MerchantSendRequestDynamic(string orderCode, string amount,string vpcMerchantTxnRef, string returnUrl, string customerEmail = "")
        {
            
            //string vpcMerchantTxnRef = "TEST_" + ticks.ToString();
            Dictionary<string, string> merchantParams = new Dictionary<string, string>
            {
                { "vpc_Version", "2" },
                { "vpc_Currency", "VND" },
                { "vpc_Command", "pay" },
                { "vpc_AccessCode", accessCode },
                { "vpc_Merchant", merchantId },
                { "vpc_Locale", "en" },
                { "vpc_ReturnURL", returnUrl },
                { "vpc_MerchTxnRef", vpcMerchantTxnRef },
                { "vpc_OrderInfo", orderCode },
                { "vpc_Amount", amount },
                { "vpc_TicketNo", "192.168.166.149" },
                //{ "vpc_CardList", "VC" },
                { "AgainLink", "https://mtf.onepay.vn/client/qt/" },
                { "Title", "PHP VPC 3-Party" },
                { "vpc_Customer_Email", customerEmail }
            };
            Dictionary<string, string> dictSorted = SortParamsMap(merchantParams);
            var stringToHash = GenerateStringToHash(dictSorted);
            Console.WriteLine("merchant's string to hash: " + stringToHash);
            var sign = GenSecurehash(stringToHash, merchantHashCode);
            merchantParams.Add("vpc_SecureHash", sign);
            var queryParam = "";
            foreach (var items in merchantParams)
            {
                var key = items.Key;
                var value = items.Value;
                queryParam += key + "=" + HttpUtility.UrlEncode(value) + "&";
            }
            var requetsUrl = BASE_URL + "?" + queryParam;

            var crypKey = "93a8b6c4d2e1f08a45c9d6e3b7f2a1d3";
            var crypVector = "b1e2d3c4a5f6b7c8";

            var merchantTxnRefCryped = Encrypt(vpcMerchantTxnRef);
            var response = new ResponseOnepay();
            response.returnUrl = requetsUrl;
            response.merchantTxnRefCrypted = merchantTxnRefCryped;
            return response;
        }

        public static void OnePayVerifySecureHash(string url, string merchantHashCode)
        {
            Dictionary<string, string> queryParamMap = new Dictionary<string, string>();
            var uri = new Uri(url);
            var param = HttpUtility.ParseQueryString(uri.Query);
            for (int i = 0; i < param.Count; i++)
            {
                var key = param.GetKey(i);
                var value = param.Get(i);
                if (key != null && value != null)
                {
                    queryParamMap.Add(key, value);
                }
            }

            var dicSorted = SortParamsMap(queryParamMap);
            var stringToHash = GenerateStringToHash(dicSorted);
            var OnePaySign = GenSecurehash(stringToHash, merchantHashCode);
            var merchantSign = queryParamMap["vpc_SecureHash"];
            Console.WriteLine("OnePay's Sign: " + OnePaySign);
            Console.WriteLine("Merchant's Sign: " + merchantSign);
            if (OnePaySign == merchantSign)
            {
                Console.WriteLine("vpc_SecureHash is Valid");
            }
            else
            {
                Console.WriteLine("vpc_SecureHash invalid");
            }
        }
        public static byte[] HexToBytes(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        public static string GenSecurehash(string notEncodeData, string merchantHashCode)
        {
            var keyHash = HexToBytes(merchantHashCode);
            var hmac = new HMACSHA256(keyHash);
            var inputBytes = Encoding.UTF8.GetBytes(notEncodeData);
            var hashBytes = hmac.ComputeHash(inputBytes);
            var sign = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            return sign;
        }

        public static string GenerateStringToHash(Dictionary<string, string> dictSorted)
        {
            var notEncodeData = "";
            foreach (var items in dictSorted)
            {
                var key = items.Key;
                var value = items.Value;
                var pref4 = key.Substring(0, Math.Min(key.Length, 4));
                var pref5 = key.Substring(0, Math.Min(key.Length, 5));
                if (pref4 == "vpc_" || pref5 == "user_")
                {
                    if (!key.Equals("vpc_SecureHashType") && !key.Equals("vpc_SecureHash"))
                    {
                        if (value.Length > 0)
                        {
                            if (notEncodeData.Length > 0)
                            {
                                notEncodeData += "&";
                            }
                            notEncodeData += key + "=" + value;
                        }
                    }
                }
            }

            return notEncodeData;
        }

        public static Dictionary<string, string> SortParamsMap(Dictionary<string, string> paramsMap)
        {
            var l = paramsMap.OrderBy(key => key.Key, StringComparer.Ordinal);
            Dictionary<string, string> dictSorted = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
            return dictSorted;
        }

        public static ResponseOnepay ExcuteGetMethod(string orderCode, string amount, string returnUrl, string customerEmail = "", string customerPhone = "", string customerId = "", RequestCreateMultipleItemOrder requestBody = null)
        {
            long ticks = DateTime.Now.Ticks;
            string vpcMerchantTxnRef = "TEST_" + ticks.ToString();
            var encryptTxnRef = Onepay.Encrypt(vpcMerchantTxnRef);
            returnUrl = returnUrl + "/" + encryptTxnRef;
            //b1. Luu vao bang logs
            if(requestBody != null)
            {
                try
                {
                    using (IDbContext context = new IDbContext())
                    {
                        OnepayRef _newPaymentOrder = new OnepayRef();
                        _newPaymentOrder.EncryptMerchantTxtCode = encryptTxnRef;
                        _newPaymentOrder.Object = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);

                        context.OnepayRefs.Add(_newPaymentOrder);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                
            }
            
                //Sau khi thanh toán trả về, kiểm tra encrypt xong thì sẽ 
            var generated = MerchantSendRequestDynamic(orderCode, amount, vpcMerchantTxnRef, returnUrl, customerEmail);
            
            if(generated != null)
            {
                return generated;
                
            }
            return null;
            

        }
        public static async Task<string> GetRedirectUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                // Không cần thiết lập vì HttpClient không có thuộc tính AllowAutoRedirect

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.Headers.Location != null)
                {
                    // Xử lý phản hồi chuyển hướng tại đây
                    string redirectUrl = response.Headers.Location.AbsoluteUri;
                    return redirectUrl;
                }
                else
                {
                    // Xử lý các trường hợp khác
                    return null;
                }
            }
        }
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    byte[] encryptedBytes = msEncrypt.ToArray();
                    string base64String = Convert.ToBase64String(encryptedBytes);
                    return base64String.Replace('+', '-').Replace('/', '_').Replace("=", "");
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            string base64String = cipherText.Replace('-', '+').Replace('_', '/');
            switch (cipherText.Length % 4)
            {
                case 2: base64String += "=="; break;
                case 3: base64String += "="; break;
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(base64String)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
