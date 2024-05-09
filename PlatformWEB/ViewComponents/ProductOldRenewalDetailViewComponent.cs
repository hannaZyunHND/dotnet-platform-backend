using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEB.ViewComponents
{
    //[ResponseCache(Duration =3600)]
    public class ProductOldRenewalDetailViewComponent : ViewComponent
    {
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<ProductOldRenewalDetailViewComponent> _localizer;
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
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguageCode = feature.RequestCulture.Culture.ToString();
                }

                return _currentLanguageCode;
            }


        }
        public ProductOldRenewalDetailViewComponent(IProductRepository productRepository, IStringLocalizer<ProductOldRenewalDetailViewComponent> localizer)
        {
            _localizer = localizer;
            _productRepository = productRepository;
        }

        public IViewComponentResult Invoke(int pageIndex, int pageSize, int LocationId, int ZoneId)
        {

            int total_row = 0;
            var model = _productRepository.GetProductOldRenewal(pageIndex, pageSize, CurrentLanguageCode, LocationId, ZoneId, out total_row);
            ViewBag.total_row = total_row;
            return View(model);
        }
    }
}
