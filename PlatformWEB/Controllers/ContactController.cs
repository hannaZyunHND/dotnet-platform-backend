using Microsoft.AspNetCore.Mvc;
using PlatformWEB.Services.Article.Repository;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Store.Repository;
using PlatformWEB.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IStoreRepository _storeRepository;

        public ContactController(IZoneRepository zoneRepository, IProductRepository productRepository, IArticleRepository articleRepository, IStoreRepository storeRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
            _storeRepository = storeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetDepartments(int loc_id)
        {
            if (loc_id > 0)
            {
                var model = _storeRepository.GetDepartmentByLocationID(CurrentLanguageCode, loc_id);
                return View(model);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
