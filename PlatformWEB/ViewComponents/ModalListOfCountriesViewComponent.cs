using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Zone.Repository;
using System.Collections.Generic;

namespace PlatformWEB.ViewComponents
{
    public class ModalListOfCountriesViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<ModalListOfCountriesViewComponent> _localizer;
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
        public ModalListOfCountriesViewComponent(IProductRepository productRepository, IStringLocalizer<ModalListOfCountriesViewComponent> localizer, IZoneRepository zoneRepository)
        {
            _localizer = localizer;
            _productRepository = productRepository;
            _zoneRepository = zoneRepository;
        }

        public IViewComponentResult Invoke(string zoneIds)
        {
            var result = _zoneRepository.GetListOfCountry(zoneIds, CurrentLanguageCode);

            return View(result);

        }
    }
}
