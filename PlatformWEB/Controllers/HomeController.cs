using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Localization;
using PlatformWEB.Models;
using PlatformWEB.Services.Extra.Repository;
using PlatformWEB.Services.Locations.Repository;
using PlatformWEB.Services.Zone.Repository;
using PlatformWEB.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEB.Controllers
{
    public class HomeController : BaseController
    {
        private IHostingEnvironment _env;
        private readonly IZoneRepository _zoneRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICookieUtility _cookieUtility;
        private readonly IActionContextAccessor _accessor;
        private readonly IExtraRepository _extraRepository;
        public HomeController(IHostingEnvironment envrnmt, IZoneRepository zoneRepository, ILocationsRepository locationsRepository, IHttpContextAccessor httpContextAccessor, ICookieUtility cookieUtility, IActionContextAccessor accessor, IExtraRepository extraRepository)
        {
            _zoneRepository = zoneRepository;
            _locationsRepository = locationsRepository;
            _cookieUtility = cookieUtility;
            _httpContextAccessor = httpContextAccessor;
            _env = envrnmt;
            _accessor = accessor;
            _extraRepository = extraRepository;

        }
        [HttpGet]
        public IActionResult SetLanguage(string culture)
        {
            culture = culture.Trim();
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), Path = "/", HttpOnly = false, Secure = false, IsEssential = true }
            );

            // Trả về cookie

            return Ok();
        }

        [HttpPost]
        public IActionResult GetAllLanguages()
        {
            var result = _extraRepository.GetAllLanguages();
            foreach (var item in result)
            {
                if (item.LanguageCode.Trim().Equals(CurrentLanguageCode))
                {
                    item.IsActive = true;
                }
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult ListOfCountry()
        {
            return ViewComponent("ListOfCountry");
        }
        [HttpPost]
        public IActionResult ListOfRegion()
        {
            return ViewComponent("ListOfRegion");
        }
        public ActionResult RedirectToDefaultCulture()
        {
            var culture = CurrentLanguage;
            if (string.IsNullOrEmpty(culture))
                culture = "vi";

            return RedirectToAction("IndexPublic");
        }
        public IActionResult Index()
        {


            return View();
        }

        public IActionResult IndexPublic()
        {
            CookieOptions cookie = new CookieOptions()
            {
                Path = "/",
                HttpOnly = false,
                Secure = false,
                IsEssential = true
            };
            if (Request.Cookies[CookieLocationId] == null)
            {
                //Lay IP
                var location_default = _locationsRepository.GetLocationFirst(CurrentLanguageCode);
                if (location_default != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(7);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieLocationId, location_default.Id.ToString(), cookie);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieLocationName, location_default.Name, cookie);
                    var result = new Utility.CookieLocation() { LocationId = location_default.Id, LocationName = location_default.Name };
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult SwitchRegion(int region_id)
        {
            return ViewComponent("SwitchRegion", new { region_id = region_id });
        }
        [HttpPost]
        public IActionResult ViewMoreRegion(int zone_parent_id, int locationId, int skip, int size)
        {
            return ViewComponent("ViewMoreRegion", new { zone_parent_id = zone_parent_id, locationId = locationId, skip = skip, size = size });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("~/Views/P404/P404.cshtml");
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetLocations()
        {
            var result = _locationsRepository.GetLocations(CurrentLanguageCode);
            return Ok(result);
        }

    }
}
