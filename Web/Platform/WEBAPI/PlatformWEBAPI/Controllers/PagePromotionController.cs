using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.PageSearch.ViewModel;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Promotion.ViewModel;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagePromotionController : ControllerBase
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        public PagePromotionController(IZoneRepository zoneRepository, IProductRepository productRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
        }
        [HttpPost]
        [Route("GetPromotionDetailByZoneId")]
        public IActionResult GetPromotionDetailByZoneId(RequestPromotionDetail request)
        {
            var response = _productRepository.GetPromotionDetail(request);
            return Ok(response);

        }

        [HttpPost]
        [Route("GetPorductInPromotionByZoneUrl")]
        public IActionResult GetPorductInPromotionByZoneUrl(RequestGetProductByKeywords request)
        {
            var total = 0;
            var response = _productRepository.GetProductInProdmotion(request.keywords, request.selectedZones, request.cultureCode, request.pageIndex, request.pageSize, out total, request.sortBy, request.startPrice, request.endPrice);
            return Ok(response);
               
        }

        [HttpPost]
        [Route("CheckValuePromotionCodeByProductId")]
        public async Task<IActionResult> CheckValuePromotionCodeByProductId (RequestCheckValuePromotionCodeByProductId request)
        {
            var response = await _productRepository.CheckValuePromotionCodeByProductId(request);
            return Ok(response);
        }

    }
}
