using Dapper;
using HtmlAgilityPack;
using MI.Dal.IDbContext;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPalCheckoutSdk.Orders;
using PlatformWEBAPI.ExecuteCommand;
using PlatformWEBAPI.Services.Extra.ViewModel;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Order.ViewModal;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Utility;
using QRCoder;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEBAPI.Services.Extra.Repository
{
    public interface IExtraRepository
    {
        List<PropertyDetail> GetPropertyDetails(string lang_code);
        List<CommentDetail> GetCommentPublisedByObjectId(int id, int type, int pageIndex = 1, int pageSize = 10);
        List<ManufactureViewModel> GetManufactures(string lang_code);
        List<ColorViewModel> GetColors(string lang_code);
        Product.ViewModel.TagViewModel GetTagTarget(string tag);

        int CreateRating(int objectId, int objectType, int rate);
        int CalculateDepartment();
        decimal GetRatingByObjectId(int objectId, int objectType);
        int CreateComment(int objectId, int objectType, string name, string phoneOrEmail, string avatar, string content, string type, int rate, string lang_code, int parentId = 0);
        int CreateContact(ServiceTicket ticket);
        int CreateContactwBookingTime(ServiceTicket ticket);
        int CreateViewCount(int objectId, string type);
        List<SiteMapViewModel> GetSiteMapUrl();
        List<ManufactureViewModel> GetManufacturesByZoneId(string lang_code, int zone_id);
        void AutoBackupDatabase();
        void TestJob();

        List<LanguageViewModel> GetAllLanguages();

        void SendEmailRegister(CustomerAuthViewModel request);
        void SendEmailRegister_v2(CustomerAuthViewModel request);
        void SendEmailSuccessOrder(SuccessEmailRequest request);
        void SendEmailSuccessOrder_v2(SuccessEmailRequest request);
        void SendEmailUpdateIccid(string currentIccid, string newIccid, int orderId, int productId, string orderCode);
        void Tool_ImportSpectifications(DataTable data);
        void Tool_ImportZone(string avatar, string vnName, string engName);

        void Tool_ImportTranslateLanguage(string langCode);
        void SendEmailEsimQRCode(string productName, string qrUrl, string customerEmail);
        Task<List<SitemapItem>> GetDynamicSiteMap();

        Task<List<SitemapItem>> GetFirstLevelSiteMap();

        Task<List<SitemapItem>> GetSecondLevelSiteMap(string culture_code);

        Task<List<SitemapItem>> GetThirdLevelSiteMap_StaticPages(string culture_code);
        Task<List<SitemapItem>> GetThirdLevelSiteMap_blog_category(string culture_code);
        Task<List<SitemapItem>> GetThirdLevelSiteMap_product_category(string culture_code);
        Task<List<SitemapItem>> GetThirdLevelSiteMap_blogs(string culture_code);
        Task<List<SitemapItem>> GetThirdLevelSiteMap_products(string culture_code);
        Task<List<Redirect>> GetRedirects();
    }
    public class ExtraRepository : IExtraRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecuters _executers;
        private readonly IDistributedCache _distributedCache;

        private readonly IConnectionMultiplexer _multiplexer;

        private readonly IZoneRepository _zoneRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public ExtraRepository(IConfiguration configuration, IExecuters executers, IDistributedCache distributedCache, IConnectionMultiplexer multiplexer, IZoneRepository zoneRepository, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;
            _zoneRepository = zoneRepository;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public List<PropertyDetail> GetPropertyDetails(string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetPropertiesByLanguage";
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<PropertyDetail>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }
        //usp_Web_CreateRating
        public int CreateRating(int objectId, int objectType, int rate)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_CreateRating";
            p.Add("@objectId", objectId);
            p.Add("@objectType", objectType);
            p.Add("@rate", rate);
            p.Add("@insertedId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            var insertedId = p.Get<int>("@insertedId");
            return insertedId;
        }

        public int CreateComment(int objectId, int objectType, string name, string phoneOrEmail, string avatar, string content, string type, int rate, string lang_code, int parentId = 0)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_CreateCommentInWebsite";
            p.Add("@objectId", objectId);
            p.Add("@objectType", objectType);
            p.Add("@name", name);
            p.Add("@phoneOrMail", phoneOrEmail);
            p.Add("@avatar", avatar);
            p.Add("@content", content);
            p.Add("@type", type);
            p.Add("@rate", rate);
            p.Add("@parentId", parentId);
            p.Add("@lang_code", lang_code);
            p.Add("@insertedId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            var insertedId = p.Get<int>("@insertedId");
            return insertedId;
        }
        public int CreateContact(ServiceTicket ticket)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_CreateContact";
            p.Add("@Name", ticket.Name);
            p.Add("@Phone", ticket.Phone);
            p.Add("@Address", ticket.Address);
            p.Add("@Title", ticket.Title);
            p.Add("@Content", ticket.Content);
            p.Add("@Type", ticket.Type);
            p.Add("@Source", ticket.Source);
            p.Add("@Inserted", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            var insertedId = p.Get<int>("@Inserted");
            return insertedId;
        }



        public List<CommentDetail> GetCommentPublisedByObjectId(int id, int type, int pageIndex = 1, int pageSize = 10)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetCommentPublisedByObjectId";
            p.Add("@id", id);
            p.Add("@type", type);
            p.Add("@pageIndex", pageIndex);
            p.Add("@pageSize", pageSize);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<CommentDetail>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public decimal GetRatingByObjectId(int objectId, int objectType)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetRatingByObjectId";
            p.Add("@objectId", objectId);
            p.Add("@objectType", objectType);
            p.Add("@rateAvg", dbType: System.Data.DbType.Decimal, direction: System.Data.ParameterDirection.Output);

            _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            var result = p.Get<decimal>("@rateAvg");
            return result;
        }

        public List<ManufactureViewModel> GetManufactures(string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetManufactures";
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ManufactureViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public List<ColorViewModel> GetColors(string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetColors";
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ColorViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public Product.ViewModel.TagViewModel GetTagTarget(string tag)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetTagByAlias";
            p.Add("@tag", tag);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<Product.ViewModel.TagViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }

        public int CreateViewCount(int objectId, string type)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_AddViewCount";
            p.Add("@objectId", objectId);
            p.Add("@type", type);

            p.Add("@insertedId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            try
            {
                _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                var insertedId = p.Get<int>("@insertedId");
                return insertedId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }


        }

        public int CalculateDepartment()
        {

            var keyCache = string.Format("Department_{0}", "CalculateDepartment");
            var result = 0;
            //get in cache
            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
                result = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(result_after_cache));
            if (result_after_cache == null)
            {
                var query = "select count(1) from Department";
                result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<int>(query));
                //Add cache
                var add_to_cache = JsonConvert.SerializeObject(result);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }
            return result;
        }

        public int CreateContactwBookingTime(ServiceTicket ticket)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_CreateContact_w_BookingTime";
            p.Add("@Name", ticket.Name);
            p.Add("@Phone", ticket.Phone);
            p.Add("@Address", ticket.Address);
            p.Add("@Title", ticket.Title);
            p.Add("@Content", ticket.Content);
            p.Add("@Type", ticket.Type);
            p.Add("@Source", ticket.Source);
            p.Add("@BookingTime", ticket.BookingTime);
            p.Add("@Inserted", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            var insertedId = p.Get<int>("@Inserted");
            return insertedId;
        }

        public List<SiteMapViewModel> GetSiteMapUrl()
        {
            var p = new DynamicParameters();
            var commandText = "usp_Tools_GenerateSiteMapUrl";
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<SiteMapViewModel>(commandText, null, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }
        public List<ManufactureViewModel> GetManufacturesByZoneId(string lang_code, int zone_id)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetManufactures_By_ZoneId";
            p.Add("@lang_code", lang_code);
            p.Add("@zone_id", zone_id);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ManufactureViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }
        public void AutoBackupDatabase()
        {
            var p = new DynamicParameters();
            var commandText = "usp_Backupdatabase";
            try
            {
                _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void TestJob()
        {
            Console.WriteLine("Job active");
        }

        public List<LanguageViewModel> GetAllLanguages()
        {
            var commandText = "Select LanguageCode, Name, Flag from Language";
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<LanguageViewModel>(commandText)).ToList();
            return result;

        }

        public void SendEmailRegister(CustomerAuthViewModel request)
        {
            // Set up sender credentials
            string senderEmail = "w2g.cs@consortio.vn";
            string senderPassword = "Csv@2024";

            // Set up recipient email address
            string recipientEmail = request.email;

            // Set up SMTP client
            SmtpClient client = new SmtpClient("mail.consortio.vn")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = false
            };

            var body = "<h1>Welcome to Way2Go</h1>";
            body += string.Format("<div>We created a account {0} with password {1}. You can use this account to buy and manage SIM </div>", request.email, request.password);

            // Set up email message
            MailMessage message = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = "Welcome to Way2GO",
                Body = body,
                IsBodyHtml = true
            };

            try
            {
                // Send the email
                client.Send(message);
                Console.WriteLine("HTML email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send HTML email. Error: " + ex.Message);
            }
        }

        public void SendEmailSuccessOrder(SuccessEmailRequest request)
        {
            // Set up sender credentials
            string senderEmail = "w2g.cs@consortio.vn";
            string senderPassword = "Csv@2024";
            // Set up recipient email address
            string recipientEmail = request.email;

            // Set up SMTP client
            SmtpClient client = new SmtpClient("mail.consortio.vn")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = false
            };

            var body = "<h1>Thank you for support!</h1>";
            body += string.Format("<div>Thank you for your order. We setting up to you a SIM CARD: <b>{0}</b> with ICCID : <b>{1}</b></div>", request.productName, request.iccid);
            body += string.Format("<div>Plase show to our employee in <b>{0}</b> this code below to get SIM</div>", request.pickupPoint);
            body += string.Format("<h2>${0}</h2>", request.orderCode);
            body += string.Format("<div>For management your SIM, please use this <a href=\"javascript:void(0)\">Link</a> and Login with your email.</div>");
            body += string.Format("<h3>Thank you for use Way2Go</h3>");

            // Set up email message
            MailMessage message = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = "Order Infomation",
                Body = body,
                IsBodyHtml = true
            };

            try
            {
                // Send the email
                client.Send(message);
                Console.WriteLine("HTML email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send HTML email. Error: " + ex.Message);
            }
        }

        public void Tool_ImportSpectifications(DataTable data)
        {

            var commandType = "usp_Tools_ImportMaintainSpectifications";
            DynamicParameters p = new DynamicParameters();
            p.Add("@tbl", data.AsTableValuedParameter("type_InsertMaintainProduct"));
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandType, p, commandType: CommandType.StoredProcedure));
        }

        public void Tool_ImportZone(string avatar, string vnName, string engName)
        {
            var commandType = "usp_Tool_W2G_InsertZone";
            DynamicParameters p = new DynamicParameters();
            p.Add("@zoneVnName", vnName);
            p.Add("@zoneEnName", engName);
            p.Add("@zoneParent", 3);
            p.Add("@zoneAvatar", avatar);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandType, p, commandType: CommandType.StoredProcedure));
            Console.WriteLine("Insert Success!");
        }

        public async void Tool_ImportTranslateLanguage(string langCode)
        {
            //Load toan bo Zone san pham ra
            var zoneSP = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.Product, "en-US", 0);
            foreach (var item in zoneSP)
            {
                var translated = await TranslateAsync(item.Name, langCode.Split("-")[0]);
                var commandType = "usp_Tool_TranslateZoneTool";
                DynamicParameters p = new DynamicParameters();
                p.Add("@zoneId", item.Id);
                p.Add("@zoneEngName", item.Name);
                p.Add("@translateName", translated);
                p.Add("@langCode", langCode);
                var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandType, p, commandType: CommandType.StoredProcedure));
            }
            //throw new NotImplementedException();
        }
        private async Task<string> TranslateAsync(string text, string cultureCode)
        {
            try
            {
                // Sử dụng "using" để đảm bảo rằng HttpClient được giải phóng sau khi hoàn thành yêu cầu
                using (System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient())
                {
                    // Tạo yêu cầu GET đến Google Translate API
                    var API_ENDPOINT = "https://translate.googleapis.com/translate_a/single";
                    string url = $"{API_ENDPOINT}?client=gtx&sl=en&tl={cultureCode}&dt=t&q={Uri.EscapeUriString(text)}";
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

        public void SendEmailRegister_v2(CustomerAuthViewModel request)
        {
            // Set up sender credentials
            string senderEmail = "w2g.cs@consortio.vn";
            string senderPassword = "Csv@2024";

            // Set up recipient email address
            string recipientEmail = request.email;

            // Set up SMTP client
            SmtpClient client = new SmtpClient("mail.consortio.vn")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = false
            };
            //Xay dung body o day
            // Doc trong thu muc
            var wwwrootPath = _hostingEnvironment.WebRootPath;
            var registerMailTemplatePath = Path.Combine(wwwrootPath, "mail-templates", "register_success-en.html");
            //
            // Tạo một đối tượng HtmlDocument
            var doc = new HtmlDocument();

            // Đọc nội dung HTML từ file
            doc.Load(registerMailTemplatePath);

            // Tim text va thay

            // Tìm nút HTML chứa class 'txtUsername' và thay đổi nội dung của nó
            var usernameNode = doc.DocumentNode.SelectSingleNode("//div[@class='txtUsername']");
            if (usernameNode != null)
            {
                usernameNode.InnerHtml = request.email;
            }

            // Tìm nút HTML chứa class 'txtPassword' và thay đổi nội dung của nó
            var passwordNode = doc.DocumentNode.SelectSingleNode("//div[@class='txtPassword']");
            if (passwordNode != null)
            {
                passwordNode.InnerHtml = request.password;
            }
            // Chuyển đổi toàn bộ tài liệu HTML thành một chuỗi

            var body = doc.DocumentNode.OuterHtml;

            // Set up email message
            MailMessage message = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = "Welcome to Way2GO",
                Body = body,
                IsBodyHtml = true
            };

            try
            {
                // Send the email
                client.Send(message);
                Console.WriteLine("HTML email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send HTML email. Error: " + ex.Message);
            }
        }

        public void SendEmailSuccessOrder_v2(SuccessEmailRequest request)
        {
            var orderCode = request.orderCode;
            var orderDetails = _orderRepository.GetOrderByCode(orderCode);
            if (orderDetails != null)
            {
                var _first = orderDetails.FirstOrDefault();
                if (_first != null)
                {
                    // Set up sender credentials
                    string senderEmail = "w2g.cs@consortio.vn";
                    string senderPassword = "Csv@2024";

                    // Set up recipient email address
                    string recipientEmail = request.email;

                    // Set up SMTP client
                    SmtpClient client = new SmtpClient("mail.consortio.vn")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(senderEmail, senderPassword),
                        EnableSsl = false
                    };
                    //Xay dung body o day
                    // Doc trong thu muc
                    var wwwrootPath = _hostingEnvironment.WebRootPath;
                    var registerMailTemplatePath = Path.Combine(wwwrootPath, "mail-templates", "order_complete_en.html");
                    //
                    // Tạo một đối tượng HtmlDocument
                    var doc = new HtmlDocument();

                    // Đọc nội dung HTML từ file
                    doc.Load(registerMailTemplatePath);

                    // Tim text va thay

                    //Tim orderCode 
                    var orderCodeNode = doc.DocumentNode.SelectSingleNode("//mj-text[@class='txtOrderCode']");
                    if (orderCodeNode != null)
                    {
                        orderCodeNode.InnerHtml = _first.OrderCode;
                    }
                    // Tim txtOrderTime
                    var orderTimeNode = doc.DocumentNode.SelectSingleNode("//mj-text[@class='txtOrderTime']");
                    if (orderTimeNode != null)
                    {
                        orderTimeNode.InnerHtml = _first.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss");
                    }
                    // Tim imageQrCode
                    var _firstItem = request.productsInfo.FirstOrDefault();
                    if (_firstItem != null)
                    {
                        if (_firstItem.type.Equals("SIM"))
                        {
                            var orderQRCode = GenerateQRCodeImageFile(orderCode);
                            if (!string.IsNullOrEmpty(orderQRCode))
                            {
                                var imageQrCodeNode = doc.DocumentNode.SelectSingleNode("//img[@class='imageQrCode']");
                                if (imageQrCodeNode != null)
                                {
                                    imageQrCodeNode.SetAttributeValue("src", string.Format("https://way2go.vn/export-qr-code/{0}", orderQRCode));
                                }
                            }
                        }

                    }

                    // Tim txtCustomerName

                    var txtCustomerNameNode = doc.DocumentNode.SelectSingleNode("//mj-text[@class='txtCustomerName']");
                    if (txtCustomerNameNode != null)
                    {
                        txtCustomerNameNode.InnerHtml = _first.CustomerEmail;
                    }

                    //txtPickupPoints
                    var txtPickupPointsNode = doc.DocumentNode.SelectSingleNode("//mj-text[@class='txtPickupPoints']");
                    if (txtPickupPointsNode != null)
                    {
                        txtPickupPointsNode.InnerHtml = _first.PickupPoint ?? "";
                        if (_firstItem != null)
                        {
                            if (_firstItem.type.Equals("TOPUP"))
                            {
                                txtPickupPointsNode.InnerHtml = "TOP UP";
                            }

                        }
                    }

                    // xay dung tblOrderInfomation
                    var tblOrderInfomation = doc.DocumentNode.SelectSingleNode("//table[@class='tblOrderInfomation']");
                    if (tblOrderInfomation != null)
                    {
                        foreach (var item in request.productsInfo)
                        {
                            var prod = orderDetails.Where(r => r.ProductId == item.productId).FirstOrDefault();
                            if (prod != null)
                            {
                                var priceString = UIHelper.FormatNumber(item.totalPrice);
                                var totalString = UIHelper.FormatNumber(item.totalPrice * (decimal)item.quantity);
                                var _tRow = $@"<tr>
                            <td>{prod.ProductName}</td>
                            <td>{item.type}</td>
                            <td>{item.quantity}</td>
                            <td>{priceString}</td>
                            <td>{totalString}</td>
                          </tr>";
                                tblOrderInfomation.InnerHtml += _tRow;
                            }

                        }

                    }



                    // Chuyển đổi toàn bộ tài liệu HTML thành một chuỗi

                    var body = doc.DocumentNode.OuterHtml;

                    // Set up email message
                    MailMessage message = new MailMessage(senderEmail, recipientEmail)
                    {
                        Subject = "Order Successful!",
                        Body = body,
                        IsBodyHtml = true
                    };

                    message.CC.Add("w2g.sales@consortio.vn");
                    message.CC.Add("w2g.acct@consortio.vn");

                    //Add them nhieu mail CC don hang vao day
                    try
                    {
                        // Send the email
                        client.Send(message);
                        Console.WriteLine("HTML email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send HTML email. Error: " + ex.Message);
                    }
                }

            }

        }
        private string GenerateQRCodeImageFile(string orderCode)
        {
            // Tạo một instance của QRCodeGenerator
            using (var qrGenerator = new QRCodeGenerator())
            {
                // Tạo QR data từ orderCode
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(orderCode, QRCodeGenerator.ECCLevel.Q);

                // Tạo một QR code bằng cách sử dụng QRCode class
                using (var qrCode = new QRCode(qrCodeData))
                {
                    // Tạo hình ảnh từ QR code
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        using (var stream = new MemoryStream())
                        {
                            // Lưu hình ảnh vào stream dưới dạng PNG
                            qrCodeImage.Save(stream, ImageFormat.Png);
                            byte[] imageBytes = stream.ToArray();
                            var wwwrootPath = _hostingEnvironment.WebRootPath;
                            var export_qr_path = Path.Combine(wwwrootPath, "export-qr-code");
                            if (!Directory.Exists(export_qr_path))
                            {
                                Directory.CreateDirectory(export_qr_path);
                            }
                            var qrCodeFile = orderCode + ".png";
                            var qrCodePath = Path.Combine(export_qr_path, qrCodeFile);

                            //Luu anh vao 
                            File.WriteAllBytes(qrCodePath, imageBytes);

                            // Chuyển đổi mảng byte hình ảnh thành chuỗi base64
                            return qrCodeFile;
                        }
                    }
                }
            }

        }

        public void SendEmailEsimQRCode(string productName, string qrUrl, string customerEmail)
        {
            // Set up sender credentials
            string senderEmail = "w2g.cs@consortio.vn";
            string senderPassword = "Csv@2024";

            // Set up recipient email address
            string recipientEmail = customerEmail;

            // Set up SMTP client
            SmtpClient client = new SmtpClient("mail.consortio.vn")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = false
            };
            var title = "QR CODE FOR SIM " + productName;
            var body = string.Format("<h1>This is QR CODE for SIM {0}</h1>", productName);
            body += "<br>";
            body += string.Format("<img src='{0}' style='width:200px' >", qrUrl);

            // Set up email message
            MailMessage message = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = title,
                Body = body,
                IsBodyHtml = true
            };

            try
            {
                // Send the email
                client.Send(message);
                Console.WriteLine("HTML email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send HTML email. Error: " + ex.Message);
            }
        }

        public void SendEmailUpdateIccid(string currentIccid, string newIccid, int orderId, int productId, string orderCode)
        {
            var orderDetails = _orderRepository.GetOrderByCode(orderCode);
            if (orderDetails != null)
            {
                var _first = orderDetails.FirstOrDefault();
                if (_first != null)
                {
                    // Set up sender credentials
                    string senderEmail = "w2g.cs@consortio.vn";
                    string senderPassword = "Csv@2024";

                    // Set up recipient email address
                    string recipientEmail = _first.CustomerEmail;

                    // Set up SMTP client
                    SmtpClient client = new SmtpClient("mail.consortio.vn")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(senderEmail, senderPassword),
                        EnableSsl = false
                    };
                    //Xay dung body o day
                    // Doc trong thu muc
                    var wwwrootPath = _hostingEnvironment.WebRootPath;
                    var registerMailTemplatePath = Path.Combine(wwwrootPath, "mail-templates", "update_iccid_en.html");
                    //
                    // Tạo một đối tượng HtmlDocument
                    var doc = new HtmlDocument();

                    // Đọc nội dung HTML từ file
                    doc.Load(registerMailTemplatePath);

                    // Tim text va thay

                    //Tim orderCode 
                    var orderCodeNode = doc.DocumentNode.SelectSingleNode("//mj-text[@class='txtOrderCode']");
                    if (orderCodeNode != null)
                    {
                        orderCodeNode.InnerHtml = _first.OrderCode;
                    }
                    // Tim txtOrderTime
                    var orderTimeNode = doc.DocumentNode.SelectSingleNode("//mj-text[@class='txtOrderTime']");
                    if (orderTimeNode != null)
                    {
                        orderTimeNode.InnerHtml = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    }
                    //// Tim imageQrCode
                    //var _firstItem = request.productsInfo.FirstOrDefault();
                    //if (_firstItem != null)
                    //{
                    //    if (_firstItem.type.Equals("SIM"))
                    //    {
                    //        var orderQRCode = GenerateQRCodeImageFile(orderCode);
                    //        if (!string.IsNullOrEmpty(orderQRCode))
                    //        {
                    //            var imageQrCodeNode = doc.DocumentNode.SelectSingleNode("//img[@class='imageQrCode']");
                    //            if (imageQrCodeNode != null)
                    //            {
                    //                imageQrCodeNode.SetAttributeValue("src", string.Format("https://way2go.vn/export-qr-code/{0}", orderQRCode));
                    //            }
                    //        }
                    //    }

                    //}

                    // Tim txtCustomerName

                    var txtCustomerNameNode = doc.DocumentNode.SelectSingleNode("//mj-text[@class='txtCustomerName']");
                    if (txtCustomerNameNode != null)
                    {
                        txtCustomerNameNode.InnerHtml = _first.CustomerEmail;
                    }

                    ////txtPickupPoints
                    //var txtPickupPointsNode = doc.DocumentNode.SelectSingleNode("//mj-text[@class='txtPickupPoints']");
                    //if (txtPickupPointsNode != null)
                    //{
                    //    txtPickupPointsNode.InnerHtml = _first.PickupPoint ?? "";
                    //    if (_firstItem != null)
                    //    {
                    //        if (_firstItem.type.Equals("TOPUP"))
                    //        {
                    //            txtPickupPointsNode.InnerHtml = "TOP UP";
                    //        }

                    //    }
                    //}

                    // xay dung tblOrderInfomation
                    var tblOrderInfomation = doc.DocumentNode.SelectSingleNode("//table[@class='tblOrderInfomation']");
                    if (tblOrderInfomation != null)
                    {
                        var prod = orderDetails.Where(r => r.ProductId == productId && r.ICCID == newIccid).FirstOrDefault();
                        if (prod != null)
                        {
                            var productDetail = _productRepository.GetProductInfomationDetail(productId, "en-US");
                            var dataLimit = productDetail.DataLimit.Replace("NGAY", "DAY");
                            if (productDetail != null)
                            {
                                var _tRow = $@"<tr>
                            <td>{productDetail.Title}</td>
                            <td>{dataLimit}</td>
                            <td>{newIccid}</td>
                            
                          </tr>";
                                tblOrderInfomation.InnerHtml += _tRow;
                            }


                        }

                    }



                    // Chuyển đổi toàn bộ tài liệu HTML thành một chuỗi

                    var body = doc.DocumentNode.OuterHtml;

                    // Set up email message
                    MailMessage message = new MailMessage(senderEmail, recipientEmail)
                    {
                        Subject = "Activate your SIM",
                        Body = body,
                        IsBodyHtml = true
                    };

                    try
                    {
                        // Send the email
                        client.Send(message);
                        Console.WriteLine("HTML email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send HTML email. Error: " + ex.Message);
                    }
                }

            }
        }

        public async Task<List<SitemapItem>> GetDynamicSiteMap()
        {
            var result = new List<SitemapItem>();

            //Lay cau truc cac trang san pham
            using (IDbContext context = new IDbContext())
            {
                var languages = await context.Language.ToListAsync();
                foreach (var language in languages)
                {
                    var languageCode = language.LanguageCode;
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        var languagePrefix = languageCode.Split("-")[0];
                        if (!string.IsNullOrEmpty(languagePrefix))
                        {
                            //Bat dau doc ra o day
                            var productsQuery = await (from p in context.Product
                                                       join pl in context.ProductInLanguage on p.Id equals pl.ProductId
                                                       where p.Status == (int)StatusProduct.Public && pl.LanguageCode == languageCode && p.ParentId <= 0
                                                       select new SitemapItem
                                                       {
                                                           Id = p.Id,
                                                           Type = "products",
                                                           Url = $"/{languagePrefix}/product/{pl.Url}-d={p.Id}"
                                                       }).ToListAsync();
                            result.AddRange(productsQuery);
                            var blogsQuery = await (from a in context.Article
                                                    join al in context.ArticleInLanguage on a.Id equals al.ArticleId
                                                    where a.Status == (int)StatusArticle.Public && al.LanguageCode == languageCode
                                                    select new SitemapItem
                                                    {
                                                        Id = a.Id,
                                                        Type = "blogs",
                                                        Url = $"/{languagePrefix}/blog/{al.Url}-d={a.Id}"
                                                    }).ToListAsync();
                            result.AddRange(blogsQuery);
                            var articlesQuery = await (from z in context.Zone
                                                       join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                                       where z.Status == (int)StatusZone.Normal && z.Type == (int)TypeZone.Article && zl.LanguageCode == languageCode
                                                       select new SitemapItem
                                                       {
                                                           Id = z.Id,
                                                           Type = "articles",
                                                           Url = $"/{languagePrefix}/blogs/{zl.Url}"
                                                       }).ToListAsync();
                            result.AddRange(articlesQuery);
                            var destinationsQuery = await (from z in context.Zone
                                                           join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                                           where z.Status == (int)StatusZone.Normal && z.Type == (int)TypeZone.DiemDen && zl.LanguageCode == languageCode
                                                           select new SitemapItem
                                                           {
                                                               Id = z.Id,
                                                               Type = "destinations",
                                                               Url = $"/{languagePrefix}/service/{zl.Url}"
                                                           }).ToListAsync();
                            result.AddRange(destinationsQuery);
                            var servicesQuery = await (from z in context.Zone
                                                       join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                                       where z.Status == (int)StatusZone.Normal && z.Type == (int)TypeZone.Product && zl.LanguageCode == languageCode
                                                       select new SitemapItem
                                                       {
                                                           Id = z.Id,
                                                           Type = "services",
                                                           Url = $"/{languagePrefix}/service/{zl.Url}"
                                                       }).ToListAsync();
                            result.AddRange(servicesQuery);
                            var combinationsQuery = new List<SitemapItem>();
                            //To hop tiep search vao day
                            foreach(var d in destinationsQuery)
                            {
                                foreach(var s in servicesQuery)
                                {
                                    combinationsQuery.Add(new SitemapItem() { Id = 0, Type = "searchCombination", Url = $"/{languagePrefix}/service/{d.Url}/{s.Url}" });
                                }
                            }
                            result.AddRange(combinationsQuery);
                            //Khuyen mai vao day
                            var discountsQuery = await (from z in context.Zone
                                                       join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                                       where z.Status == (int)StatusZone.Normal && z.Type == (int)TypeZone.Discount && zl.LanguageCode == languageCode
                                                       select new SitemapItem
                                                       {
                                                           Id = z.Id,
                                                           Type = "services",
                                                           Url = $"/{languagePrefix}/promotions/{z.Id}/{zl.Url}"
                                                       }).ToListAsync();
                            result.AddRange(discountsQuery);
                            //Page tinh vao day
                            var staticPagesQuery = new List<SitemapItem>();
                            
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}" }); //Page Home 
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/about-us" });
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/become-a-partner" });
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/carts" });
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/contact" });
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/error" });
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/faq" });
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/observe" });
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/payment" });
                            staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/terms-and-conditions" });
                            result.AddRange(staticPagesQuery);
                        }
                    }



                }

            }
            return result;
        }

        public async Task<List<SitemapItem>> GetFirstLevelSiteMap()
        {
            var result = new List<SitemapItem>();

            //Lay cau truc cac trang san pham
            using (IDbContext context = new IDbContext())
            {
                var languages = await context.Language.ToListAsync();
                foreach (var language in languages)
                {
                    var languageCode = language.LanguageCode;
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        var languagePrefix = languageCode.Split("-")[0];
                        if (!string.IsNullOrEmpty(languagePrefix))
                        {
                            if(languagePrefix == "en")
                            {
                                var firstLevelSitemap = new List<SitemapItem>();
                                firstLevelSitemap.Add(new SitemapItem() { Id = 0, Type = "firstLevel", Url = $"/{languagePrefix}/sitemap.xml" });
                                result.AddRange(firstLevelSitemap);
                            }
                            
                        }
                    }

                }

            }
            return result;
        }

        private Dictionary<string, string> getLanguageDictionary()
        {
             Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("en", "en-US");
            result.Add("vi", "vi-VN");
            result.Add("ko", "ko-KR");
            result.Add("zh", "zh-CN");

            return result;
        }

        public async Task<List<SitemapItem>> GetSecondLevelSiteMap(string culture_code)
        {
            var result = new List<SitemapItem>();

            //Lay cau truc cac trang san pham
            using (IDbContext context = new IDbContext())
            {
                var languages = await context.Language.ToListAsync();
                foreach (var language in languages)
                {
                    var languageCode = language.LanguageCode;
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        var languagePrefix = languageCode.Split("-")[0];
                        if (!string.IsNullOrEmpty(languagePrefix))
                        {
                            if (languagePrefix.Equals(culture_code))
                            {
                                var secondLevelSiteMap = new List<SitemapItem>();
                                secondLevelSiteMap.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/", Priority="1.0", ChangeFreq = "weekly" });
                                secondLevelSiteMap.Add(new SitemapItem() { Id = 0, Type = "static_page", Url = $"/{languagePrefix}/static_page.xml", Priority = "1.0", ChangeFreq = "monthly" });
                                secondLevelSiteMap.Add(new SitemapItem() { Id = 0, Type = "blog_category", Url = $"/{languagePrefix}/blog_category.xml", Priority = "0.6", ChangeFreq = "monthly" });
                                secondLevelSiteMap.Add(new SitemapItem() { Id = 0, Type = "product_category", Url = $"/{languagePrefix}/product_category.xml", Priority = "0.8", ChangeFreq = "weekly" });
                                secondLevelSiteMap.Add(new SitemapItem() { Id = 0, Type = "blogs", Url = $"/{languagePrefix}/blogs.xml" , Priority = "1.0", ChangeFreq = "daily" });
                                secondLevelSiteMap.Add(new SitemapItem() { Id = 0, Type = "products", Url = $"/{languagePrefix}/products.xml", Priority = "1.0", ChangeFreq = "daily" });

                                result.AddRange(secondLevelSiteMap);
                            }
                            
                        }
                    }

                }

            }
            return result;
        }

        public async Task<List<SitemapItem>> GetThirdLevelSiteMap_StaticPages(string culture_code)
        {
            var result = new List<SitemapItem>();

            //Lay cau truc cac trang san pham
            using (IDbContext context = new IDbContext())
            {
                var languages = await context.Language.ToListAsync();
                foreach (var language in languages)
                {
                    var languageCode = language.LanguageCode;
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        var languagePrefix = languageCode.Split("-")[0];
                        if (!string.IsNullOrEmpty(languagePrefix))
                        {
                            if (languagePrefix.Equals(culture_code))
                            {
                                var staticPagesQuery = new List<SitemapItem>();

                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/about-us" });
                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/become-a-partner" });
                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/carts" });
                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/contact" });
                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/error" });
                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/faq" });
                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/observe" });
                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/payment" });
                                staticPagesQuery.Add(new SitemapItem() { Id = 0, Type = "home", Url = $"/{languagePrefix}/terms-and-conditions" });
                                result.AddRange(staticPagesQuery);
                            }

                        }
                    }

                }

            }
            return result;
        }

        public async Task<List<SitemapItem>> GetThirdLevelSiteMap_blog_category(string culture_code)
        {
            var result = new List<SitemapItem>();

            //Lay cau truc cac trang san pham
            using (IDbContext context = new IDbContext())
            {
                var languages = await context.Language.ToListAsync();
                foreach (var language in languages)
                {
                    var languageCode = language.LanguageCode;
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        var languagePrefix = languageCode.Split("-")[0];
                        if (!string.IsNullOrEmpty(languagePrefix))
                        {
                            if (languagePrefix.Equals(culture_code))
                            {
                                var articlesQuery = await (from z in context.Zone
                                                           join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                                           where z.Status == (int)StatusZone.Normal && z.Type == (int)TypeZone.Article && zl.LanguageCode == languageCode
                                                           select new SitemapItem
                                                           {
                                                               Id = z.Id,
                                                               Type = "articles",
                                                               Url = $"/{languagePrefix}/blogs/{zl.Url}"
                                                           }).ToListAsync();
                                result.AddRange(articlesQuery);
                            }
                            
                            
                        }
                    }



                }

            }
            return result;
        }

        public async Task<List<SitemapItem>> GetThirdLevelSiteMap_product_category(string culture_code)
        {
            var result = new List<SitemapItem>();

            //Lay cau truc cac trang san pham
            using (IDbContext context = new IDbContext())
            {
                var languages = await context.Language.ToListAsync();
                foreach (var language in languages)
                {
                    var languageCode = language.LanguageCode;
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        var languagePrefix = languageCode.Split("-")[0];
                        if (!string.IsNullOrEmpty(languagePrefix))
                        {
                            if (languagePrefix.Equals(culture_code))
                            {
                                //var destinationsQuery = await (from z in context.Zone
                                //                               join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                //                               where z.Status == (int)StatusZone.Normal && z.Type == (int)TypeZone.DiemDen && zl.LanguageCode == languageCode
                                //                               select new SitemapItem
                                //                               {
                                //                                   Id = z.Id,
                                //                                   Type = "destinations",
                                //                                   Url = $"/{languagePrefix}/service/{zl.Url}",
                                //                                   Slug = zl.Url
                                //                               }).ToListAsync();
                                //result.AddRange(destinationsQuery);
                                var servicesQuery = await (from z in context.Zone
                                                           join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                                           where z.Status == (int)StatusZone.Normal
                                                               && z.Type == (int)TypeZone.Product
                                                               && zl.LanguageCode == languageCode
                                                               && z.ParentId > 0
                                                               && context.Zone.Any(c => c.ParentId == z.Id) // Kiểm tra có con
                                                           select new SitemapItem
                                                           {
                                                               Id = z.Id,
                                                               Type = "services",
                                                               Url = $"/{languagePrefix}/{zl.Url}",
                                                               Slug = zl.Url
                                                           }).ToListAsync();
                                result.AddRange(servicesQuery);
                                foreach(var item in servicesQuery)
                                {
                                    var childQuery = await (from z in context.Zone
                                                            join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                                            where z.Status == (int)StatusZone.Normal && z.Type == (int)TypeZone.Product && zl.LanguageCode == languageCode && (z.ParentId == item.Id)
                                                            select new SitemapItem
                                                            {
                                                                Id = z.Id,
                                                                Type = "services",
                                                                Url = $"{item.Url}/{zl.Url}",
                                                                Slug = zl.Url
                                                            }).ToListAsync();
                                    result.AddRange(childQuery);
                                }
                                var combinationsQuery = new List<SitemapItem>();
                                //To hop tiep search vao day
                                //foreach (var d in destinationsQuery)
                                //{
                                //    foreach (var s in servicesQuery)
                                //    {
                                //        combinationsQuery.Add(new SitemapItem() { Id = 0, Type = "searchCombination", Url = $"/{languagePrefix}/service/{d.Slug}/{s.Slug}" });
                                //    }
                                //}
                                //result.AddRange(combinationsQuery);
                                //Khuyen mai vao day
                                var discountsQuery = await (from z in context.Zone
                                                            join zl in context.ZoneInLanguage on z.Id equals zl.ZoneId
                                                            where z.Status == (int)StatusZone.Normal && z.Type == (int)TypeZone.Discount && zl.LanguageCode == languageCode
                                                            select new SitemapItem
                                                            {
                                                                Id = z.Id,
                                                                Type = "services",
                                                                Url = $"/{languagePrefix}/promotions/{z.Id}/{zl.Url}"
                                                            }).ToListAsync();
                                result.AddRange(discountsQuery);

                            }

                        }
                    }



                }

            }
            return result;
        }

        public async Task<List<SitemapItem>> GetThirdLevelSiteMap_blogs(string culture_code)
        {
            var result = new List<SitemapItem>();

            //Lay cau truc cac trang san pham
            using (IDbContext context = new IDbContext())
            {
                var languages = await context.Language.ToListAsync();
                foreach (var language in languages)
                {
                    var languageCode = language.LanguageCode;
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        var languagePrefix = languageCode.Split("-")[0];
                        if (!string.IsNullOrEmpty(languagePrefix))
                        {
                            if (languagePrefix.Equals(culture_code))
                            {
                                var blogsQuery = await (from a in context.Article
                                                        join al in context.ArticleInLanguage on a.Id equals al.ArticleId
                                                        where a.Status == (int)StatusArticle.Public && al.LanguageCode == languageCode
                                                        select new SitemapItem
                                                        {
                                                            Id = a.Id,
                                                            Type = "blogs",
                                                            Url = $"/{languagePrefix}/blog/{a.Id}-{al.Url}.html"
                                                        }).ToListAsync();
                                result.AddRange(blogsQuery);
                            }
                            
                        }
                    }



                }

            }
            return result;
        }
        public async Task<List<SitemapItem>> GetThirdLevelSiteMap_products(string culture_code)
        {
            var result = new List<SitemapItem>();

            //Lay cau truc cac trang san pham
            using (IDbContext context = new IDbContext())
            {
                var languages = await context.Language.ToListAsync();
                foreach (var language in languages)
                {
                    var languageCode = language.LanguageCode;
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        var languagePrefix = languageCode.Split("-")[0];
                        if (!string.IsNullOrEmpty(languagePrefix))
                        {
                            if (languagePrefix.Equals(culture_code))
                            {
                                //Bat dau doc ra o day
                                var productsQuery = await (from p in context.Product
                                                           join pl in context.ProductInLanguage on p.Id equals pl.ProductId
                                                           where p.Status == (int)StatusProduct.Public && pl.LanguageCode == languageCode && p.ParentId == 0
                                                           select new SitemapItem
                                                           {
                                                               Id = p.Id,
                                                               Type = "products",
                                                               Url = $"/{languagePrefix}/product/{p.Id}-{pl.Url}.html"
                                                           }).ToListAsync();
                                result.AddRange(productsQuery);
                            }
                        }
                    }



                }

            }
            return result;
        }

        public async Task<List<Redirect>> GetRedirects()
        {

            var keyCache = string.Format("JT_GetRedirects");
            var result = new List<Redirect>();

            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Redirect>>(Encoding.UTF8.GetString(result_after_cache));
            }
            else
            {
                using (IDbContext context = new IDbContext())
                {
                    result = await context.Redirect.ToListAsync();
                    var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                    var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                    _distributedCache.Set(keyCache, result_after_cache, cache_options);
                }
            }
            return result;

        }
    }
}
