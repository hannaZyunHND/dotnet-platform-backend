using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Product.ViewModel;
using PlatformWEB.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.ViewComponents
{
    public class ViewMoreViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;

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

        public IProductRepository ProductRepository => _productRepository;

        public ViewMoreViewComponent(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IViewComponentResult Invoke(int zone_parent_id, int locationId, int skip, int size)
        {
            var total = size;
            ViewBag.Total = size;
            ViewBag.Id = zone_parent_id;
            var model = ProductRepository.GetProductMinifiesTreeViewByZoneParentIdSkipping(zone_parent_id, CurrentLanguageCode, locationId, skip, size);
            var res = new List<ProductMinify>();
            foreach (var item in model)
            {
                if (!res.Where(r => r.ProductId == item.ProductId).Any())
                {
                    res.Add(item);
                }
            }
            var diff = total - res.Count();
            ViewBag.Total = diff;
            return View(res);
        }
    }
}
