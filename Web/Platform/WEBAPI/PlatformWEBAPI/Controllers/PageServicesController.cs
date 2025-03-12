using MI.Entity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageServicesController : ControllerBase
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IExtraRepository _extraRepository;

        public PageServicesController(IZoneRepository zoneRepository, IExtraRepository extraRepository)
        {
            _zoneRepository = zoneRepository;
            _extraRepository = extraRepository;

        }
        [HttpPost]
        [Route("GetServiceFullDetail")]
        public async Task<IActionResult> GetServiceFullDetail(RequestGetZoneDetailByAlias request)
        {
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.alias) && !string.IsNullOrEmpty(request.cultureCode))
                {
                    var objectItem = _zoneRepository.GetZoneByAlias(request.alias, request.cultureCode);
                    if (objectItem != null)
                    {
                        var zoneDetail = _zoneRepository.GetZoneDetail(objectItem.Id, request.cultureCode);
                        if(zoneDetail != null)
                        {
                            zoneDetail.breadcrumbs = _zoneRepository.GetBreadcrumbByZoneId(objectItem.Id, request.cultureCode);
                            if(zoneDetail.breadcrumbs != null)
                            {
                                zoneDetail.breadcrumbs = zoneDetail.breadcrumbs.Where(r => r.ParentId > 0).ToList();
                            }
                        }
                        return Ok(zoneDetail);
                    }

                }

            }
            return Ok(null);
        }

        [HttpPost]
        [Route("GetDestinationSubAlias")]
        public async Task<IActionResult> GetDestinationSubAlias(RequestGetZoneDetailByAlias request)
        {
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.alias) && !string.IsNullOrEmpty(request.cultureCode))
                {
                    var objectItem = _zoneRepository.GetZoneByAlias(request.alias, request.cultureCode);
                    if (objectItem != null)
                    {
                        var destinationSearchItems = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.Product, request.cultureCode, objectItem.Id);
                        destinationSearchItems = destinationSearchItems.Where(r => r.Id != objectItem.Id).ToList();
                        
                        return Ok(destinationSearchItems);
                    }

                }

            }
            return Ok(null);
        }

    }
}
