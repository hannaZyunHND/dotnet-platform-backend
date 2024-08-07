using MI.Entity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.PageHome.ViewModel;
using PlatformWEBAPI.Services.Zone.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.Article.ViewModel;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageHomeController : ControllerBase
    {
        private readonly IExtraRepository _extraRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        public PageHomeController(IExtraRepository extraRepository, IZoneRepository zoneRepository, IProductRepository productRepository, IArticleRepository articleRepository)
        {
            _extraRepository = extraRepository;
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
           _articleRepository = articleRepository;
        }

        [HttpPost]
        [Route("GetAllLanguage")]
        public async Task<IActionResult> GetLanguages()
        {
            var result = _extraRepository.GetAllLanguages();
            return Ok(result);
        }

        [HttpPost]
        [Route("GetProductsInRegion")]
        public async Task<IActionResult> GetProductsInRegion([FromBody] RequestHomeRegionViewModel request)
        {
            var response = new List<ResponseHomeRegionViewModel>();
            if(request != null)
            {
                var regions = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.Region, request.cultureCode, 0);
                if (regions != null)
                {
                    foreach (var item in regions)
                    {
                        var resItem = new ResponseHomeRegionViewModel();
                        resItem.region = item;
                        var _t = 0;
                        var products = _productRepository.GetProductInRegionByZoneIdMinify(item.Id, 0, request.cultureCode, 1, 9, out _t);
                        if (products != null)
                        {
                            resItem.products.AddRange(products);
                        }
                        response.Add(resItem);
                    }
                }
            }
            
            return Ok(response);
        }

        [HttpPost]
        [Route("GetProductLastSeen")]
        public async Task<IActionResult> GetProductLastSeen(RequestGetProductLastSeen request)
        {
            var response = new List<ProductMinify>();
            if (request != null)
            {
                response = _productRepository.GetProductByListId(request.ids, request.cultureCode, 0);
            }
            return Ok(response);    
        }


        [HttpPost]
        [Route("GetBlogsHomePage")]
        public async Task<IActionResult> GetBlogsHomePage(RequestGetBlogsHomePage request)
        {
            var response = new List<ArticleMinify>();
            if (request != null)
            {
                var _t = 0;
                response = _articleRepository.GetArticlesInZoneId_Minify_FullFilter(0, (int)TypeZone.All, (int)TypeArticle.Blog, 1, request.cultureCode, "", 1, 12, out _t);
            }
            return Ok(response);
            
        }
    }
}
