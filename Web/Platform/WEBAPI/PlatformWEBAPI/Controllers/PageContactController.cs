
using MI.Entity.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using PayPalHttp;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.BannerAds.Repository;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Extra.ViewModel;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Utility;
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

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageContactController : ControllerBase
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IExtraRepository _extraRepository;
        private readonly IBannerAdsRepository _bannerAdsRepository;
        private const string API_ENDPOINT = "https://translate.googleapis.com/translate_a/single";
        private IHostingEnvironment _hostingEnvironment;
        //private readonly HttpClient _httpClient;


        public PageContactController(IExtraRepository extraRepository, IZoneRepository zoneRepository, IBannerAdsRepository bannerAdsRepository, IHostingEnvironment hostingEnvironment)
        {
            _extraRepository = extraRepository;
            _zoneRepository = zoneRepository;
            _bannerAdsRepository = bannerAdsRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("CreateServiceTicket")]
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
                var list_mail_receivers = _bannerAdsRepository.GetConfigByName(ticket.CultureCode, "MailManager");

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
    }
}
