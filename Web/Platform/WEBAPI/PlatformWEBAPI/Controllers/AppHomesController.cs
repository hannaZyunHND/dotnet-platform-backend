using MI.Entity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Orders;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.PageHome.ViewModel;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppHomesController : ControllerBase
    {
        private readonly IExtraRepository _extraRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;

        public AppHomesController(IExtraRepository extraRepository, IZoneRepository zoneRepository, IProductRepository productRepository, IArticleRepository articleRepository)
        {
            _extraRepository = extraRepository;
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
        }
        [HttpPost]
        [Route("GetListOfRegions")]
        public async Task<IActionResult> GetListOfRegions([FromBody] RequestHomeRegionViewModel request)
        {
            if (request != null)
            {
                var regions = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.Region, request.cultureCode, request.parentId);
                return Ok(regions);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("GetListProductInRegion")]
        public async Task<IActionResult> GetListProductInRegion([FromBody] RequestGetListProductInRegionViewModel request)
        {
            var _t = 0;
            if (request != null)
            {
                var products = _productRepository.GetProductInRegionByZoneIdMinify(request.regionId, 0, request.cultureCode, 1, 9, out _t);
                if (products != null)
                {
                   return Ok(products);
                }
            }
            return BadRequest();
        }
    }
}
