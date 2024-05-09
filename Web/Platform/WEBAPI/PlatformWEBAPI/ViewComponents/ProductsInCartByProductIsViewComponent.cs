using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Promotion.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.ViewComponents
{
    public class ProductsInCartByProductIsViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICookieUtility _cookieUnitily;
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
        public ProductsInCartByProductIsViewComponent(IZoneRepository zoneRepository, IProductRepository productRepository, IPromotionRepository promotionRepository, IOrderRepository orderRepository, ICookieUtility cookieUtility)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _promotionRepository = promotionRepository;
            _orderRepository = orderRepository;
            _cookieUnitily = cookieUtility;
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
