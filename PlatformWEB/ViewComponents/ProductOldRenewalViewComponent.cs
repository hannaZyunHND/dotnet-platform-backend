using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PlatformWEB.Services.Locations.Repository;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.ViewComponents
{
    //[ResponseCache(Duration =3600)]
    public class ProductOldRenewalViewComponent : ViewComponent
    {
        private readonly IProductRepository _productRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly IStringLocalizer<ProductMenuViewComponent> _localizer;
        private string _currentLanguage;
        private string _currentLanguageCode;
        public const string CookieLocationId = "_LocationId";
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
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguageCode = feature.RequestCulture.Culture.ToString();
                }

                return _currentLanguageCode;
            }


        }
        public ProductOldRenewalViewComponent(IProductRepository productRepository, ILocationsRepository locationsRepository, IStringLocalizer<ProductMenuViewComponent> localizer)
        {
            _localizer = localizer;
            _productRepository = productRepository;
            _locationsRepository = locationsRepository;
        }

        public IViewComponentResult Invoke(int pageIndex, int pageSize, int ZoneId, int TypeView)
        {
            int total_row = 0;
            //var location_id = Request.Cookies[CookieLocationId] == null ? 0 : int.Parse(Request.Cookies[CookieLocationId]);
            var daxem = pageIndex * pageSize;
            var model = _productRepository.GetProductOldRenewalV2(pageIndex, pageSize, CurrentLanguageCode, 0, ZoneId, out total_row);
            if (total_row > daxem)
                ViewBag.Total_row = total_row - daxem;

            var listLocation = _locationsRepository.GetLocations(CurrentLanguageCode);
            ViewBag.Type_view = TypeView;
            ViewBag.Locations = listLocation;


            return View(model);
        }
    }
}
