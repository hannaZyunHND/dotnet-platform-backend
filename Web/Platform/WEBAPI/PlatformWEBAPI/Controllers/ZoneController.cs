using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    public class ZoneController : BaseController
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IExtraRepository _extratRepository;
        private readonly IArticleRepository _articleRepository;
        //private readonly IStringLocalizer<HomeController> _localizer;

        public ZoneController(IZoneRepository zoneRepository, IProductRepository productRepository, IExtraRepository extraRepository, IArticleRepository articleRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _extratRepository = extraRepository;
            _articleRepository = articleRepository;
        }

        public IActionResult RedirectAction(string alias, int? pageIndex, int? pageSize, string priceRange)
        {
            var aliasArr = alias.Split(" _ ");
            pageIndex = pageIndex ?? 1;
            pageSize = pageSize ?? 12;

            var zone_tar = _zoneRepository.GetZoneByAlias(aliasArr[0], CurrentLanguageCode);
            if (zone_tar != null)
            {
                ViewBag.ZoneId = zone_tar.Id;
                ViewBag.Type = zone_tar.Type;
                ViewBag.Parent = zone_tar.ParentId;
                ViewBag.PageIndex = pageIndex;
                ViewBag.PageSize = pageSize;
                ViewBag.IsHaveChild = zone_tar.isHaveChild;
                ViewBag.PriceRange = aliasArr.Length > 1 ? aliasArr[1] : null;
                return View();
            }
            //return View("~/Views/P404/P404.cshtml");
            return RedirectToAction("IndexPublic", "Home");
        }


        public List<ZoneSugget> GetZoneSugget()
        {
            var zone_tar = _zoneRepository.GetZoneSugget(CurrentLanguageCode);
            return zone_tar;
        }

        public IActionResult ModalListOfCountries(string zoneIds)
        {
            //ModalListOfCountries
            return ViewComponent("ModalListOfCountries", new { zoneIds = zoneIds });
        }

    }
}