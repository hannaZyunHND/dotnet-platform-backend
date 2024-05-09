using MI.Entity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.Locations.Repository;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;
using PlatformWEBAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;


namespace PlatformWEBAPI.Controllers
{
    public class ProductComponentController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ILocationsRepository _locationsRepository;


        public ProductComponentController(IZoneRepository zoneRepository, ILocationsRepository locationsRepository, IProductRepository productRepository, IArticleRepository articleRepository, IHttpContextAccessor httpContextAccessor)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
            _locationsRepository = locationsRepository;
            _httpContextAccessor = httpContextAccessor;

        }

        public IActionResult ProductComponentList(int? pageIndex, int? pageSize, int? zoneId, int? zoneIdChild, int? productCpnId)
        {
            var TypeView = 0;
            var total_row = 0;
            var location_id = Request.Cookies[CookieLocationId] == null ? 0 : int.Parse(Request.Cookies[CookieLocationId]);
            var daxem = pageIndex * pageSize;
            var model = _productRepository.GetProductComponent(zoneId ?? 0, zoneIdChild ?? 0, productCpnId ?? 0, location_id, CurrentLanguageCode, "", pageIndex ?? 1, pageSize ?? 100, out total_row);
            ViewBag.Type_view = TypeView;
            ViewBag.ZoneId = zoneId;
            return View(model);
        }

        public IActionResult ProductComponentList_Zone(string alias, int? pageIndex, int? pageSize, int? zoneId, int? zoneIdChild, int? productCpnId)
        {
            var zone_tar = _zoneRepository.GetZoneByAlias(alias, "vi-VN");
            var zone_detail = new ZoneDetail();
            var z = 0;
            var z_parent = 0;
            if (zone_tar != null)
            {
                z = zone_tar.Id;
                z_parent = zone_tar.ParentId;
                zone_detail = _zoneRepository.GetZoneDetail(z, "vi-VN");
            }
            var TypeView = 0;
            var total_row = 0;
            var location_id = Request.Cookies[CookieLocationId] == null ? 0 : int.Parse(Request.Cookies[CookieLocationId]);
            var daxem = pageIndex * pageSize;
            var model = new List<ProductMinify>();

            var fullUrl = HttpContext.Request.GetDisplayUrl();
            var search_sp = "";
            var queryString = fullUrl.Split("?s=").ToList();
            if (queryString.Count >= 2)
            {
                search_sp = queryString[1];
            }
            if (z_parent <= 0)
            {
                model = _productRepository.GetProductComponent(zoneId ?? z, zoneIdChild ?? 0, productCpnId ?? 0, location_id, CurrentLanguageCode, search_sp, pageIndex ?? 1, pageSize ?? 100, out total_row);
            }

            if (z_parent > 0)
            {
                model = _productRepository.GetProductComponent(zoneId ?? z_parent, zoneIdChild ?? z, productCpnId ?? 0, location_id, CurrentLanguageCode, search_sp, pageIndex ?? 1, pageSize ?? 100, out total_row);
            }

            ViewBag.ZoneDetail = zone_detail;
            ViewBag.Type_view = TypeView;
            ViewBag.ZoneId = z;
            ViewBag.Alias = alias;
            ViewBag.ZoneSelectedId = z;
            ViewBag.ZoneParentSelected = z_parent;
            ViewBag.FullUrl = fullUrl;
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductComponentListJson(int? pageIndex, int? pageSize, int? zoneId, int? zoneIdChild, int? productCpnId)
        {
            var total_row = 0;
            var location_id = Request.Cookies[CookieLocationId] == null ? 0 : int.Parse(Request.Cookies[CookieLocationId]);
            int zoneIdValue = (zoneId == null || zoneId == -1) ? 0 : zoneId.Value;
            var model = _productRepository.GetProductComponent(zoneIdValue, zoneIdChild ?? 0, productCpnId ?? 0, location_id, CurrentLanguageCode, "", pageIndex ?? 1, pageSize ?? 10, out total_row);
            foreach (var item in model)
            {
                item.PriceStr = UIHelper.FormatNumber(item.Price);
                item.Linktar = LinkRedirectUrlUtility.ProductDetailUrl(item.Url);
                item.LinkImageAvatar = UIHelper.StoreFilePath(item.Avatar);
                item.Title = item.Title.Replace(";", " Phiên bản ");
            }
            return Json(model);
        }

        [HttpGet]
        public IActionResult ListChangeChildZone(int zoneId)
        {
            var list_zone_all = _zoneRepository.GetZoneByTreeViewShowMenuMinifies(1, CurrentLanguageCode, 0, 1);
            var model = new List<ZoneByTreeViewMinify>();
            if (zoneId == -1)
                model = list_zone_all.Where(r => r.ParentId != 0 && r.IsShowMenu == 1).ToList();
            else
                model = list_zone_all.Where(r => r.ParentId == zoneId && r.IsShowMenu == 1).ToList();
            return Json(model);
        }
        [HttpGet]
        public IActionResult ListProductCpn(int zoneId)
        {
            var model = _productRepository.GetAllProductComponent(zoneId);
            return Json(model);
        }
    }
}