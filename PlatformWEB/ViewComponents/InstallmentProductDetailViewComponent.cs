
using MI.Entity.Enums;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PlatformWEB.Services.Article.Repository;
using PlatformWEB.Services.Extra.Repository;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Product.ViewModel;
using PlatformWEB.Services.Zone.Repository;
using PlatformWEB.Utility;
using PlatformWEB.ViewModel.Installment;
using System.Linq;

namespace PlatformWEB.ViewComponents
{
    public class InstallmentProductDetailViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IExtraRepository _extraRepository;
        private readonly ICookieUtility _cookieUtility;
        const string CookieLocationId = "_LocationId";
        const string CookieLocationName = "_LocationName";
        private string _currentLanguage;
        private string _currentLanguageCode;

        public InstallmentProductDetailViewComponent(
            IZoneRepository zoneRepository,
            IProductRepository productRepository,
            IArticleRepository articleRepository,
            IExtraRepository extraRepository,
            ICookieUtility cookieUtility
        )
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
            _extraRepository = extraRepository;
            _cookieUtility = cookieUtility;
        }
        public IViewComponentResult Invoke(ProductDetail request)
        {
            if (request != null)
            {
                InstallmentProductDetailViewModel response = new InstallmentProductDetailViewModel();

                var productId = request.Id;
                if (request != null)
                {
                    var cookie = _cookieUtility.SetCookieDefault();
                    var currentLocationId = cookie?.LocationId ?? 0;
                    var currentLocationName = cookie?.LocationName;

                    response.Infomation = request;
                    response.LocationId = currentLocationId;
                    response.LocationName = currentLocationName;

                    response.Spectification = _productRepository.GetProductSpectificationDetail(productId, CurrentLanguageCode);
                    response.ListLocation = _productRepository.GetProductPriceInLocationDetail(productId, CurrentLanguageCode);
                    var defaultPriceItem = response.ListLocation.Where(r => r.LocationId == currentLocationId).FirstOrDefault();
                    if (defaultPriceItem != null)
                    {
                        response.DefaultLocationPrice = defaultPriceItem;
                    }
                    ViewBag.DefaultLocationPrice = defaultPriceItem;
                    var totalRowCombo = 0;
                    response.Combo = _productRepository.GetProductsInProductById(productId, "com-bo", currentLocationId, CurrentLanguageCode, 1, 4, out totalRowCombo);
                    response.Promotion = _productRepository.GetPromotionInProduct(productId, currentLocationId, CurrentLanguageCode);

                    var sameZoneTotal = 0;
                    response.SameZone = _productRepository.GetProductInZoneByZoneIdMinify(request.ZoneId, currentLocationId, CurrentLanguageCode, 1, 6, out sameZoneTotal);
                    response.SameTotal = sameZoneTotal;

                    response.DefaultLocation = currentLocationName;
                    response.Comments = _extraRepository.GetCommentPublisedByObjectId(productId, (int)CommentType.Product);
                }

                return View(response);
            }
            return null;
        }


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
    }
}
