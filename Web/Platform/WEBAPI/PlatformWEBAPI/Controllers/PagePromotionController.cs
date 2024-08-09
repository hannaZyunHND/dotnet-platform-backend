using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;

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
    }
}
