using Microsoft.AspNetCore.Localization;
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
    public class SwitchProductListViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<SwitchProductListViewComponent> _localizer;
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
        public SwitchProductListViewComponent(IZoneRepository zoneRepository, IProductRepository productRepository, IStringLocalizer<SwitchProductListViewComponent> localizer)
        {
            _localizer = localizer;
            _productRepository = productRepository;
            _zoneRepository = zoneRepository;
        }

        public IViewComponentResult Invoke(int zone_parent_id, int locationId)
        {
            var total = 0;
            var discountPercent = 0;
            if (Request.Cookies["discount"] != null)
            {
                // Lấy giá trị của cookie "myCookie"
                string cookieValue = Request.Cookies["discount"];

                // Xử lý giá trị cookie ở đây
                // Ví dụ: Trả về một dòng chứa giá trị cookie
                int.TryParse(cookieValue, out discountPercent);
            }
            var zoneDetail = _zoneRepository.GetZoneDetail(zone_parent_id, CurrentLanguageCode);
            var model = _productRepository.GetProductMinifiesTreeViewByZoneParentId(zone_parent_id, CurrentLanguageCode, locationId, 1, 99, discountPercent, out total).Distinct().ToList();
            ViewBag.Total = total;
            ViewBag.Id = zone_parent_id;
            ViewBag.ZoneName = zoneDetail.Name;
            return View(model);
        }
    }
}
