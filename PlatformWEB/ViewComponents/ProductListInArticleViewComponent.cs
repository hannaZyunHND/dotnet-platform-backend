
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PlatformWEB.Services.Product.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.ViewComponents
{
    public class ProductListInArticleViewComponent : ViewComponent
    {
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<ProductListInArticleViewComponent> _localizer;
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
        public ProductListInArticleViewComponent(IProductRepository productRepository, IStringLocalizer<ProductListInArticleViewComponent> localizer)
        {
            _localizer = localizer;
            _productRepository = productRepository;
        }
        public IViewComponentResult Invoke(string product_ids, int location_id)
        {
            var total_row = 0;
            var products = new List<int>();
            if (!string.IsNullOrEmpty(product_ids))
            {
                var splitted = product_ids.Split(',');
                foreach (var product in splitted)
                {
                    products.Add(int.Parse(product.Trim()));
                }
            }

            var model = _productRepository.GetProductByListId(products, CurrentLanguageCode, location_id);
            ViewBag.Total = total_row;
            return View(model);

        }
    }
}
