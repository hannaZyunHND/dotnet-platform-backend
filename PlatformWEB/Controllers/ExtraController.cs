
using MI.Entity.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using PayPalHttp;
using PlatformWEB.Services.Article.Repository;
using PlatformWEB.Services.BannerAds.Repository;
using PlatformWEB.Services.Extra.Repository;
using PlatformWEB.Services.Extra.ViewModel;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Zone.Repository;
using PlatformWEB.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEB.Controllers
{
    public class ExtraController : BaseController
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IExtraRepository _extraRepository;
        private readonly IBannerAdsRepository _bannerAdsRepository;
        private const string API_ENDPOINT = "https://translate.googleapis.com/translate_a/single";
        private IHostingEnvironment _hostingEnvironment;
        //private readonly HttpClient _httpClient;


        public ExtraController(IExtraRepository extraRepository, IZoneRepository zoneRepository, IBannerAdsRepository bannerAdsRepository, IHostingEnvironment hostingEnvironment)
        {
            _extraRepository = extraRepository;
            _zoneRepository = zoneRepository;
            _bannerAdsRepository = bannerAdsRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult RedirectToParentZone(string alias, int parent_id)
        {
            var list_parents = _zoneRepository.GetListZoneByParentId((int)TypeZone.AllButProduct, CurrentLanguageCode);
            var type_target = list_parents.Where(r => r.Id == parent_id).FirstOrDefault();
            if (type_target != null)
            {
                switch (type_target.Type)
                {
                    case (int)TypeZone.Article:
                        //return RedirectToAction("BlogList1", "Blog");
                        return RedirectToRoute("Blogs", new { alias = alias, zone_id = parent_id });
                    case (int)TypeZone.Promotion:
                        return RedirectToRoute("Promotions");
                    case (int)TypeZone.DiemDen:
                        return RedirectToRoute("quotation");
                    case (int)TypeZone.Recruitment:
                        return RedirectToRoute("Recruitment");
                    case (int)TypeZone.Visa:
                        return RedirectToRoute("DownloadCategory");
                    default:
                        return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public IActionResult GetPropertyDetails()
        {
            var result = _extraRepository.GetPropertyDetails(CurrentLanguageCode);
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }
        [HttpPost]
        public IActionResult CreateRating(int objectId, int objectType, int rate)
        {
            var result = _extraRepository.CreateRating(objectId, objectType, rate);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateViewCount(int objectId, string type)
        {
            var result = _extraRepository.CreateViewCount(objectId, type);
            return Ok(result);
        }
        public IActionResult CreateComment(int objectId, int objectType, string name, string phoneOrEmail, string avatar, string content, string type, int rate, int parentId = 0)
        {
            var result = _extraRepository.CreateComment(objectId, objectType, name, phoneOrEmail, avatar, content, type, rate, CurrentLanguageCode, parentId);

            return Ok(result);
        }

        public IActionResult CreateServiceTicket(ServiceTicket ticket)
        {

            var result = _extraRepository.CreateContact(ticket);

            var body = "";
            body += "<h1>Bạn có thông tin liên hệ mới</h1>";
            body += "<table>";
            body += "<tbody>";
            body += "<tr>";

            body += "<td>Họ tên: </td>";
            body += "<td>" + ticket.Name + "</td>";

            body += "<td>Số điện thoại: </td>";
            body += "<td>" + ticket.Phone + "</td>";

            body += "<td>Nội dung: </td>";
            body += "<td>" + ticket.Content + "</td>";

            body += "</tr>";
            body += "</tbody>";
            body += "</table>";

            try
            {
                var list_mail_receivers = _bannerAdsRepository.GetConfigByName(CurrentLanguageCode, "MailManager");

                if (list_mail_receivers != null)
                {
                    var x = list_mail_receivers.Split(",");
                    foreach (var item in x)
                        Email.Send(item, "THÔNG BÁO TƯ VẤN ĐƠN HÀNG", body, null);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateServiceTicketWBookingTime(ServiceTicket ticket)
        {

            var result = _extraRepository.CreateContactwBookingTime(ticket);


            return Ok(result);
        }

        [HttpPost]
        public IActionResult SendMail(string type, string body)
        {
            var list_mail_receivers = _bannerAdsRepository.GetConfigByName(CurrentLanguageCode, "MailManager");

            if (type == "tu-van")
            {
                //var list_mail_receivers = _bannerAdsRepository.GetConfigByName(CurrentLanguageCode,"MailManager");
                if (list_mail_receivers != null)
                {
                    var x = list_mail_receivers.Split(",");
                    foreach (var item in x)
                        Email.Send(item, "THÔNG BÁO TƯ VẤN ĐƠN HÀNG", body, null);
                }
            }

            if (type == "order")
            {
                //var list_mail_receivers = _bannerAdsRepository.GetConfigByName(CurrentLanguageCode,"MailManager");
                if (list_mail_receivers != null)
                {
                    var x = list_mail_receivers.Split(",");
                    foreach (var item in x)
                        Email.Send(item, "THÔNG BÁO ĐƠN HÀNG", body, null);
                }
            }
            if (type == "comment")
            {
                if (list_mail_receivers != null)
                {
                    var x = list_mail_receivers.Split(",");
                    foreach (var item in x)
                        Email.Send(item, "THÔNG BÁO COMMENT", body, null);
                }
            }
            if (type == "rating")
            {
                if (list_mail_receivers != null)
                {
                    var x = list_mail_receivers.Split(",");
                    foreach (var item in x)
                        Email.Send(item, "THÔNG BÁO RATING", body, null);
                }
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult GetCommentList(int object_id, int object_type, int? pageIndex)
        {
            pageIndex = pageIndex ?? 1;
            return ViewComponent("Comment", new { object_id = object_id, object_type = object_type, pageIndex = pageIndex });
        }
        [HttpPost]
        public IActionResult GetReplyComment(int id, int obj_id, int obj_type)
        {
            if (obj_id > 0)
            {
                ViewBag.Id = id;
                ViewBag.ObjId = obj_id;
                ViewBag.ObjType = obj_type;
                return View();
            }
            return BadRequest();

        }
        public IActionResult SiteMapXml()
        {
            var sitemapNodes = _bannerAdsRepository.GetConfigByName(CurrentLanguageCode, "SiteMap");
            var url = Utils.UIHelper.StoreFilePath(sitemapNodes, false);
            var textFromFile = (new WebClient()).DownloadString(url);
            return this.Content(textFromFile, "text/xml", Encoding.UTF8);
        }

        public IActionResult SiteMapGenerate()
        {

            return View();
        }

        public IActionResult RobotsTxT()
        {
            var sitemapNodes = _bannerAdsRepository.GetConfigByName(CurrentLanguageCode, "Robotxt");
            var url = Utils.UIHelper.StoreFilePath(sitemapNodes, false);
            var textFromFile = (new WebClient()).DownloadString(url);
            return this.Content(textFromFile, "text/plain", Encoding.UTF8);
        }

        [HttpPost]
        public IActionResult Tool_ImportSpectifications(IFormFile file, string culture_code, string transLanguage = "vi")
        {
            try
            {
                // Kiểm tra xem tệp có được tải lên không
                if (file == null || file.Length <= 0)
                {
                    return BadRequest("Tệp không tồn tại hoặc trống.");
                }

                // Kiểm tra loại tệp có phải là Excel không
                if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Định dạng tệp không hợp lệ. Chỉ chấp nhận tệp Excel (.xlsx).");
                }

                // Đọc dữ liệu từ tệp Excel
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        // Xử lý dữ liệu từ tệp Excel ở đây
                        // Ví dụ: Lưu dữ liệu vào cơ sở dữ liệu, xử lý và trả về kết quả
                        // Khởi tạo DataTable
                        DataTable dataTable = new DataTable();
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        //Tao ra cac cot cho DataTable
                        dataTable.Columns.Add("productId", typeof(int));
                        dataTable.Columns.Add("value", typeof(string));
                        dataTable.Columns.Add("spectificationId", typeof(int));
                        dataTable.Columns.Add("language_code", typeof(string));

                        //Lap qua bang de lay du lieu
                        // Bắt đầu đọc từ dòng thứ hai (vì hàng đầu tiên là tiêu đề)
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            // Tạo một hàng mới cho DataTable


                            //Doc du leiu cot dau de lay id
                            var id = int.Parse(worksheet.Cells[row, 1].Value?.ToString());

                            // Lấy số cột trong tệp Excel
                            int columns = worksheet.Dimension.End.Column;
                            for (int col = 3; col <= columns; col++)
                            {
                                DataRow dataRow = dataTable.NewRow();
                                dataRow["productId"] = id;
                                dataRow["spectificationId"] = int.Parse(worksheet.Cells[1, col].Value?.ToString());
                                if (col == 3)
                                {
                                    dataRow["value"] = worksheet.Cells[row, col].Value?.ToString();
                                }
                                else
                                {
                                    if (culture_code == "en-US")
                                    {
                                        dataRow["value"] = worksheet.Cells[row, col].Value?.ToString();
                                    }
                                    else
                                    {
                                        var trans = this.TranslateAsync(worksheet.Cells[row, col].Value?.ToString(), transLanguage).GetAwaiter().GetResult();
                                        dataRow["value"] = trans;
                                    }

                                }

                                dataRow["language_code"] = culture_code;
                                dataTable.Rows.Add(dataRow);
                            }
                            // Thêm hàng vào DataTable

                        }

                        // Goi vao Database Insert du lieu
                        _extraRepository.Tool_ImportSpectifications(dataTable);

                        // Trả về kết quả thành công
                        return Ok("Tệp Excel đã được tải lên thành công và xử lý.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }

        }


        [HttpGet]
        public async Task<IActionResult> Tool_ImportZoneAsync(string img, string zoneName)
        {
            var image_location = await DownloadImage(img);
            var zoneVnName = await TranslateAsync(zoneName);
            _extraRepository.Tool_ImportZone(image_location, zoneVnName, zoneName);
            return Ok(image_location);

        }

        [HttpGet]
        public async Task<IActionResult> Tool_UpdateLanguageZone(string langcode)
        {
            _extraRepository.Tool_ImportTranslateLanguage(langcode);
            return Ok("Success!");
        }

        private async Task<string> DownloadImage(string url)
        {
            // Kiểm tra nếu URL không hợp lệ
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL không được để trống", nameof(url));
            }

            // Thư mục đích để lưu hình ảnh
            var wwwroot = Path.Combine(_hostingEnvironment.WebRootPath);
            string thuMucDich = Path.Combine(wwwroot, "icon_countries");

            // Tạo thư mục nếu nó không tồn tại
            if (!Directory.Exists(thuMucDich))
            {
                Directory.CreateDirectory(thuMucDich);
            }

            // Lấy tên tệp hình ảnh từ URL
            string tenHinh = Path.GetFileName(new Uri(url).AbsolutePath);

            // Tạo đường dẫn đầy đủ đến tệp hình ảnh
            string duongDanHinh = Path.Combine(thuMucDich, tenHinh);

            // Tạo một HTTP client để tải hình ảnh từ URL
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                // Tải hình ảnh từ URL
                HttpResponseMessage response = await client.GetAsync(url);

                // Kiểm tra xem có lỗi không
                response.EnsureSuccessStatusCode();

                // Đọc dữ liệu hình ảnh
                byte[] bytes = await response.Content.ReadAsByteArrayAsync();

                // Ghi dữ liệu hình ảnh vào tệp
                await System.IO.File.WriteAllBytesAsync(duongDanHinh, bytes);
            }

            // Trả về đường dẫn bắt đầu từ 'icon_countries'
            return Path.Combine("icon_countries", tenHinh);
        }
        private async Task<string> TranslateAsync(string text, string transLang = "vi")
        {
            try
            {
                // Sử dụng "using" để đảm bảo rằng HttpClient được giải phóng sau khi hoàn thành yêu cầu
                using (System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient())
                {
                    // Tạo yêu cầu GET đến Google Translate API
                    var API_ENDPOINT = "https://translate.googleapis.com/translate_a/single";
                    string url = $"{API_ENDPOINT}?client=gtx&sl=en&tl={transLang}&dt=t&q={Uri.EscapeUriString(text)}";
                    var response = await _httpClient.GetAsync(url);

                    // Đảm bảo yêu cầu thành công
                    response.EnsureSuccessStatusCode();

                    // Đọc dữ liệu JSON từ phản hồi và phân tích kết quả để lấy văn bản dịch
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JArray jsonArray = JArray.Parse(jsonResponse);
                    string translatedText = jsonArray[0][0][0].ToString();

                    return translatedText;
                }

            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine("Đã xảy ra lỗi khi dịch văn bản: " + ex.Message);
                return "";
            }
        }

    }
}