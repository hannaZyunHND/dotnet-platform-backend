using MI.Dal.IDbContext;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiHelpersController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly string KEY = "";

        public AiHelpersController(IConfiguration configuration)
        {
            _configuration = configuration;
            KEY = configuration["OpenAiApiKey"];
        }

        
        [HttpGet]
        [Route("UpdateProductInLanguage/{productId}")]
        public async Task<IActionResult> UpdateProductInLanguage(int productId)
        {
            using (IDbContext _context = new IDbContext())
            {
                var products = _context.Product.Where(r => (r.Status == (int)StatusProduct.Public && r.Id == productId) || (r.Status == (int)StatusProduct.Public && r.ParentId == productId));
                var languagesTarget = new List<string>() { "ko-KR", "zh-CN" };

                var message = "";


                foreach (var item in products)
                {
                    var langEng = _context.ProductInLanguage.Where(r => r.ProductId == item.Id && r.LanguageCode.Contains("en-US")).FirstOrDefault();
                    //langEng.Product = null;
                    if (langEng != null)
                    {
                        foreach (var lang in languagesTarget)
                        {
                            var checker = _context.ProductInLanguage.Where(r => r.ProductId == item.Id && r.LanguageCode.Contains(lang)).FirstOrDefault();

                            if (checker == null)
                            {
                                // Them moi
                                var settings = new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                };
                                var newLang = JsonConvert.DeserializeObject<ProductInLanguage>(
                                    JsonConvert.SerializeObject(langEng, settings)
                                );

                                newLang.LanguageCode = lang;
                                newLang.Id = Guid.NewGuid().ToString();
                                newLang.Title = await TranslateTextUsingAi(langEng.Title, lang);
                                newLang.Title = newLang.Title.Replace("\"", "");
                                newLang.Description = await TranslateHtmlUsingAi(langEng.Description, lang);
                                newLang.Content = await TranslateHtmlUsingAi(langEng.Content, lang);
                                newLang.Url = langEng.Url;
                                newLang.UrlOld = langEng.UrlOld;
                                var lichTour = new List<TourMetaData>();

                                var lichTourEng = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TourMetaData>>(langEng.LichTour);
                                var thongTinTourEng = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TourMetaData>>(langEng.ThongTinTour);
                                var thuTucVisaEng = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TourMetaData>>(langEng.ThuTucVisa);

                                foreach (var mtd in lichTourEng)
                                {
                                    mtd.tieuDe = await TranslateTextUsingAi(mtd.tieuDe, lang);
                                    mtd.tieuDe = mtd.tieuDe.Replace("\"", "");
                                    mtd.noiDung = await TranslateHtmlUsingAi(mtd.noiDung, lang);
                                }

                                newLang.LichTour = Newtonsoft.Json.JsonConvert.SerializeObject(lichTourEng);

                                foreach (var mtd in thongTinTourEng)
                                {
                                    mtd.tieuDe = await TranslateTextUsingAi(mtd.tieuDe, lang);
                                    mtd.noiDung = await TranslateHtmlUsingAi(mtd.noiDung, lang);
                                }

                                newLang.ThongTinTour = Newtonsoft.Json.JsonConvert.SerializeObject(thongTinTourEng);

                                foreach (var mtd in thuTucVisaEng)
                                {
                                    mtd.tieuDe = await TranslateTextUsingAi(mtd.tieuDe, lang);
                                    mtd.noiDung = await TranslateHtmlUsingAi(mtd.noiDung, lang);
                                }

                                newLang.ThuTucVisa = Newtonsoft.Json.JsonConvert.SerializeObject(thuTucVisaEng);


                                newLang.location = await TranslateTextUsingAi(langEng.location, lang);
                                newLang.unit = await TranslateTextUsingAi(langEng.unit, lang);



                                await _context.ProductInLanguage.AddAsync(newLang);
                                await _context.SaveChangesAsync();

                                message += $"Thực hiện dịch thành công cho sản phẩm {langEng.ProductId} - {langEng.Title} với ngôn ngữ {lang} \n";

                            }

                        }
                    }
                }
                return Ok(message);
            }
                
        }


        private async Task<string> TranslateTextUsingAi(string text, string languageCode)
        {
            
            // Tôi cần viết một nghiệp vụ với .NET Core 2.1
            // Sử dụng API KEY để dịch 1 đoạn text sang ngôn ngữ chỉ định ở `languageCode`, (Ví dụ như ko-KR, zh-CN, vi-VN, en-US...)
            // Dịch sẽ giữ nguyên các dấu | hoặc - giống trong phân cách của tiêu đề
            // Mục đích là dịch tiêu đề của các dịch vụ du lịch tại Việt Nam
            // Nhớ kiểm tra nếu text là rỗng thì ko cần làm gì cả

            if (string.IsNullOrWhiteSpace(text))
                return text;

            var prompt = $"Translate the following tourism-related title to {languageCode}. Keep any separators such as '|' or '-' intact. Do not translate separator symbols and number. Text is:\n\n\"{text}\"";

            var requestBody = new
            {
                model = "gpt-4", // hoặc "gpt-3.5-turbo"
                messages = new[]
                {
            new { role = "system", content = "You are a translation assistant for tourism content." },
            new { role = "user", content = prompt }
        },
                temperature = 0.3
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", KEY);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
            string result = jsonResponse?.choices?[0]?.message?.content?.ToString();

            return result ?? text;

        }

        private async Task<string> TranslateHtmlUsingAi(string text, string languageCode)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var prompt = $@"
                        You are a translation assistant. Translate the following HTML content to {languageCode}. 
                        Preserve all HTML tags and attributes exactly as they are. Only translate the visible text content 
                        to the target language. The HTML is from a tourism-related website in Vietnam:

                        {text}
                        ";

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
            new { role = "system", content = "You are an expert in translating HTML without modifying the structure." },
            new { role = "user", content = prompt }
        },
                temperature = 0.3
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", KEY);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
            string result = jsonResponse?.choices?[0]?.message?.content?.ToString();

            return result ?? text;
        }

    }

    public class TourMetaData
    {
        public string tieuDe { get; set; }
        public string noiDung { get; set; }
    }
}
