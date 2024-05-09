using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.ViewComponents
{
    public class ModalBuyCartsViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<ModalBuyCartsViewComponent> _localizer;
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
        public ModalBuyCartsViewComponent(IProductRepository productRepository, IStringLocalizer<ModalBuyCartsViewComponent> localizer)
        {
            _localizer = localizer;
            _productRepository = productRepository;
        }

        public IViewComponentResult Invoke(List<int> products)
        {
            if (products.Count > 0)
            {
                var currentLanguage = CurrentLanguageCode;
                var locationId = 0;
                var result = _productRepository.GetProductByListId(products, currentLanguage, locationId);

                return View(result);
            }
            return null;
        }
    }
}
