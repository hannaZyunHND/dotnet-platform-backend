
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
    public class ExtraController : Controller
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

        

        public async Task<IActionResult> SiteMapGenerate()
        {

            return View("~/Views/Extra/SiteMapGenerate.cshtml");
        }

        

    }
}