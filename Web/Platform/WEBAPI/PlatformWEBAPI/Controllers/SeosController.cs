using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Extra.Repository;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeosController : ControllerBase
    {
        private readonly IExtraRepository _extraRepository;
        public SeosController(IExtraRepository extraRepository)
        {
            _extraRepository = extraRepository;
        }
        [HttpGet]
        [Route("GetDynamicSiteMap")]
        public async Task<IActionResult> GetDynamicSiteMap()
        {
            var result = await _extraRepository.GetDynamicSiteMap();
            return Ok(result);
        }


        

    }
}
