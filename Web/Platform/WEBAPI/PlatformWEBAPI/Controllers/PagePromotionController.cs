using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagePromotionController : ControllerBase
    {
        private readonly IZoneRepository _zoneRepository;
        public PagePromotionController(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }
        [HttpPost]
        [Route("GetPromotionDetailByZoneId")]
        public IActionResult GetPromotionDetailByZoneId(RequestGetPromotionDetailByZoneId request)
        {
            if(request != null)
            {

            }
            return Ok();
        }
    }
}
