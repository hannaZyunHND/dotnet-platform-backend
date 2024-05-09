using MI.Entity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Localization;
using PlatformWEB.Models;
using PlatformWEB.Services.Extra.Repository;
using PlatformWEB.Services.Locations.Repository;
using PlatformWEB.Services.Order.Repository;
using PlatformWEB.Services.Order.ViewModal;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Zone.Repository;
using PlatformWEB.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEB.Controllers
{
    public class CustomerController : BaseController
    {
        private IHostingEnvironment _env;
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICookieUtility _cookieUtility;
        private readonly IActionContextAccessor _accessor;
        private readonly IOrderRepository _orderRepository;
        private readonly IExtraRepository _extraRepository;
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
        public CustomerController(IHostingEnvironment envrnmt, IZoneRepository zoneRepository, ILocationsRepository locationsRepository, IHttpContextAccessor httpContextAccessor, ICookieUtility cookieUtility, IActionContextAccessor accessor, IProductRepository productRepository, IOrderRepository orderRepository, IExtraRepository extraRepository)
        {
            _zoneRepository = zoneRepository;
            _locationsRepository = locationsRepository;
            _cookieUtility = cookieUtility;
            _httpContextAccessor = httpContextAccessor;
            _extraRepository = extraRepository;
            _env = envrnmt;
            _accessor = accessor;
            _productRepository = productRepository;
            _orderRepository = orderRepository;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetProductsByCustomerId(int customerId, int localtionId)
        {
            return ViewComponent("ProductListByCustomer", new { customerId = customerId, locationId = localtionId });
        }
        [HttpPost]
        public IActionResult GetProductsInCartByProductIs(List<int> products)
        {
            return ViewComponent("ProductsInCartByProductIs", new { products = products });
        }
        [HttpPost]
        public IActionResult GetTopUpsModalDetail(int product_id, int order_id, string order_iccid)
        {
            return ViewComponent("ModalChooseTopUp", new { product_id = product_id, order_id = order_id, order_iccid = order_iccid });
        }
        [HttpGet]
        [Route("buy-all-cart/{productIds}")]
        public IActionResult BuyAllCart(string productIds)
        {
            var products = new List<int>();
            if (!string.IsNullOrEmpty(productIds))
            {
                foreach (var item in productIds.Split(","))
                {
                    products.Add(int.Parse(item));
                }
            }

            if (products.Count > 0)
            {
                var currentLanguage = CurrentLanguageCode;
                var locationId = 0;
                var result = _productRepository.GetProductByListId(products, currentLanguage, locationId);

                return View(result);
            }
            return null;
        }
        [HttpPost]
        public IActionResult DoLogin(CustomerAuthViewModel request)
        {
            var result = _orderRepository.DoLogin(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public IActionResult ChangePassword(CustomerAuthViewModel request)
        {
            var result = _orderRepository.ChangePassword(request);
            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public IActionResult DoSignUp(CustomerAuthViewModel request)
        {
            var result = _orderRepository.DoSignUp(request);
            if (result > 0)
            {
                _extraRepository.SendEmailRegister(request);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }


    }
}
