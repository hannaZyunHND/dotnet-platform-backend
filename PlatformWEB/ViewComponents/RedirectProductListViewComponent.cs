using MI.Entity.Enums;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PlatformWEB.Services.Article.Repository;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Zone.Repository;
using PlatformWEB.Services.Zone.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.ViewComponents
{
    public class RedirectProductListViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        const string CookieLocationId = "_LocationId";
        const string CookieLocationName = "_LocationName";
        private string _currentLanguage;
        private string _currentLanguageCode;
        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                    return _currentLanguage;

                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                }

                return _currentLanguage;
            }
        }
        private string CurrentLanguageCode
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguageCode))
                    return _currentLanguageCode;

                if (string.IsNullOrEmpty(_currentLanguageCode))
                {
                    IRequestCultureFeature feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguageCode = feature.RequestCulture.Culture.ToString();
                }

                return _currentLanguageCode;
            }


        }
        public RedirectProductListViewComponent(IZoneRepository zoneRepository, IProductRepository productRepository, IArticleRepository articleRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
        }
        public IViewComponentResult Invoke(ZoneDetail zoneDetal, int pageIndex, int pageSize, string priceRange)
        {
            if (zoneDetal != null)
            {
                var zoneId = zoneDetal.Id;
                var zone_selected_with_treeview_all = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.Product, CurrentLanguageCode, 0);
                var zone_selected_with_treeview = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.Product, CurrentLanguageCode, zoneId);
                var zone_details = zoneDetal;
                ViewBag.ZoneTreeViewAll = zone_selected_with_treeview_all;
                ViewBag.ZoneTreeView = zone_selected_with_treeview;
                ViewBag.ZoneDetail = zone_details;
                ViewBag.Index = pageIndex;
                ViewBag.Size = pageSize;
                ViewBag.Name = zoneDetal.Name;

                double fromPrice = 0;
                double toPrice = 0;

                if (!string.IsNullOrEmpty(priceRange))
                {
                    var lstPrice = priceRange.Split("-");
                    fromPrice = double.Parse(lstPrice[0]);
                    toPrice = double.Parse(lstPrice[1]);
                }

                ViewBag.FPrice = fromPrice;
                ViewBag.TPrice = toPrice;

                return View();
            }
            return null;

        }
    }
}
