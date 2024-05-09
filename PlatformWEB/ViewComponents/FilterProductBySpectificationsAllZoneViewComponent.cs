using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PlatformWEB.Services.Article.Repository;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Product.ViewModel;
using PlatformWEB.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.ViewComponents
{
    public class FilterProductBySpectificationsAllZoneViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
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
        public FilterProductBySpectificationsAllZoneViewComponent(IZoneRepository zoneRepository, IProductRepository productRepository, IArticleRepository articleRepository, IHttpContextAccessor httpContextAccessor)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public IViewComponentResult Invoke(FilterProductBySpectification fp)
        {
            if (fp != null)
            {
                var parentId = fp.parentId;
                var total = 0;
                var model = _productRepository.FilterProductBySpectificationsInZone(fp, out total);
                var res = new List<ProductMinify>();
                foreach (var item in model)
                {
                    if (!res.Where(r => r.ProductId == item.ProductId).Any())
                    {
                        res.Add(item);
                    }
                    if (res.Where(r => r.ProductId == item.ProductId).Any())
                    {
                        if (item.ZoneId == fp.parentId)
                        {
                            res.Where(r => r.ProductId == item.ProductId).FirstOrDefault().IsHot = item.IsHot;
                        }
                    }
                }
                res = res.OrderBy(r => r.IsHot).ToList();
                var diff = res.Count();
                ViewBag.Total = diff;
                ViewBag.Size = fp.pageSize;
                ViewBag.Index = fp.pageNumber;
                ViewBag.ZoneId = parentId;

                return View(res);
            }
            return null;
        }
    }
}
