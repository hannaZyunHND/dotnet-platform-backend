using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PlatformWEB.Services.Order.Repository;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.ViewComponents
{
    public class ModalChooseTopUpViewComponent : ViewComponent
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<ModalChooseTopUpViewComponent> _localizer;
        private readonly IOrderRepository _orderRepository;
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
        public ModalChooseTopUpViewComponent(IProductRepository productRepository, IStringLocalizer<ModalChooseTopUpViewComponent> localizer, IOrderRepository orderRepository)
        {
            _localizer = localizer;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public IViewComponentResult Invoke(int product_id, int order_id, string order_iccid)
        {
            var total = 0;
            var model = _productRepository.GetProductInfomationDetail(product_id, CurrentLanguageCode, 1);

            //var topupHistory = 
            var topUpHistory = _orderRepository.GetListOrderDetailByOrderId(order_id, CurrentLanguageCode);
            ViewBag.Total = total;
            ViewBag.Id = product_id;
            ViewBag.OrderICCID = order_iccid;
            ViewBag.OrderID = order_id;
            ViewBag.TopUpHistory = topUpHistory;
            return View(model);
        }
    }
}
