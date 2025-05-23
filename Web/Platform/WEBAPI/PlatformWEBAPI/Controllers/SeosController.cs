using MI.Dal.IDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformWEBAPI.Services.BannerAds.Repository;
using PlatformWEBAPI.Services.Extra.Repository;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeosController : ControllerBase
    {
        private readonly IExtraRepository _extraRepository;
        private readonly IBannerAdsRepository _bannerAdsRepository;
        public SeosController(IExtraRepository extraRepository, IBannerAdsRepository bannerAdsRepository)
        {
            _extraRepository = extraRepository;
            _bannerAdsRepository = bannerAdsRepository;
        }
        [HttpGet]
        [Route("GetDynamicSiteMap")]
        public async Task<IActionResult> GetDynamicSiteMap()
        {
            var result = await _extraRepository.GetDynamicSiteMap();
            return Ok(result);
        }

        [HttpGet]
        [Route("Redirects")]
        public async Task<IActionResult> Redirects()
        {
            using (IDbContext context = new IDbContext())
            {
                var redirects = await _extraRepository.GetRedirects();
                return Ok(redirects);
            }

        }

        [HttpGet]
        [Route("GetBannerAds/{code}/{languageCode}")]
        public async Task<IActionResult> GetBannerAds(string code, string languageCode)
        {
            if(!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(languageCode))
            {
                var response = _bannerAdsRepository.GetConfigByName(languageCode, code);

                return Ok(response);
            }
            return NotFound();
        }
    }
}

