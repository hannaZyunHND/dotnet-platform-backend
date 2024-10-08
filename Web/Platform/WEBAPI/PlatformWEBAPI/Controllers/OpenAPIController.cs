using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.OpenAPI;
using PlatformWEBAPI.Services.OpenAPI.Model;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAPIController : ControllerBase
    {
        private readonly IOpenAPIRepository _openAPIRepository;
        public OpenAPIController(IOpenAPIRepository openAPIRepository)
        {
            _openAPIRepository = openAPIRepository;
        }
        [HttpPost("GetProductsInServicesByCouponCode")]
        public async Task<IActionResult> GetProductsInServicesByCouponCode(RequestGetProductsInServicesByCouponCode request)
        {
            var response = await _openAPIRepository.GetProductsInServicesByCouponCode(request);
            return Ok(response);
        }
        [HttpPost("GetServicesByCouponId")]
        public async Task<IActionResult> GetServicesByCouponId(RequestGetServicesByCouponId request)
        {
            var response = await _openAPIRepository.GetServicesByCouponId(request);
            return Ok(response);
        }
    }
}
