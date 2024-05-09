using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.Locations.Repository;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEBAPI.ViewComponents
{
    public class RedirectCategoriesListViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ILocationsRepository _locationsRepository;

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
        public RedirectCategoriesListViewComponent(IZoneRepository zoneRepository, ILocationsRepository locationsRepository, IProductRepository productRepository, IArticleRepository articleRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
            _locationsRepository = locationsRepository;

        }
        public IViewComponentResult Invoke()
        {
            int total_row = 0;
            var location_id = Request.Cookies[CookieLocationId] == null ? 0 : int.Parse(Request.Cookies[CookieLocationId]);
            var model = _productRepository.GetProductOldRenewalV2(1, 10, CurrentLanguageCode, 0, 0, out total_row);
            if (total_row > 10)
                ViewBag.total_row = total_row - 1 * 10;
            var listLocation = _locationsRepository.GetLocations(CurrentLanguageCode);
            ViewBag.Locations = listLocation;
            return View(model);
        }
    }
}
