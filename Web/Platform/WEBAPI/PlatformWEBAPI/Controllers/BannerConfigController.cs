using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.BannerAds.Repository;
using PlatformWEBAPI.Services.BannerAds.ViewModel;
using PlatformWEBAPI.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerConfigController : ControllerBase
    {
        private IBannerAdsRepository _bannerAdsRepository;
        public BannerConfigController(IBannerAdsRepository bannerAdsRepository)
        {
            _bannerAdsRepository = bannerAdsRepository;
        }
        [HttpPost]
        [Route("GetBannerConfigByCodes")]
        public async Task<IActionResult> GetBannerConfigByCodes([FromBody] RequestBannerAdsByCodes request)
         {

            var response = new List<MultipleBannerAdsViewModal>();
            if(request != null)
            {
                foreach (var item in request.codes)
                {
                    var result = _bannerAdsRepository.GetBannerAds_By_Code(request.cultureCode, item);
                    if (result != null)
                    {
                        var detail = WebHelper.ConvertSlide(result);
                        if (detail != null)
                        {
                            var _obj = new MultipleBannerAdsViewModal();
                            _obj.bannerAdsCode = item;
                            _obj.details = detail;
                            response.Add(_obj);
                        }
                    }
                }
            }
            
            if(response.Count > 0)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Code is not correct!");
            }
            
        }
    }
}
