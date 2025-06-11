
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.PageSearch.ViewModel;
using PlatformWEBAPI.Services.Product.Repository;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public SearchController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        [Route("MergeES")]
        public async Task<IActionResult> MergeEs()
        {
            var result = _productRepository.GetEsSearchResult();
            //Ghi nhan vao elastic
            bool isCreated = MI.ES.BCLES.AutocompleteService.CreateIndexAsync().Result;

            if (isCreated)
            {
                var data = MI.ES.BCLES.AutocompleteService.IndexAsync(result);
            }

            return Ok("ES was merged");
        }

        [HttpPost]
        [Route("ElasticFilter")]
        public IActionResult ElasticFilter(RequestElasticFilter request)
        {
            //MI.Service.SyncProductToES.Run();
            var Check = Utils.Settings.AppSettings.GetByKey("ESEnable").ToLower();
            if (Check == "True".ToLower())
            {
                var total = 0;
                var result = MI.ES.BCLES.AutocompleteService.SuggestJoytimeAsync(request.keyword, request.culture_code, out total, request.index, request.size);
                //return MI.ES.BCLES.AutocompleteService.SuggestAsync(keyword, CurrentLanguageCode, 0, 10, out total);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
