using MI.Dapper.Data.Repositories.Interfaces;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController : ControllerBase
    {

        private readonly IZoneRepository _zoneRepository;
        public ZonesController(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        [HttpPost]
        [Route("GetZoneByType")]
        public async Task<IActionResult> GetZoneByType([FromBody] RequestGetZoneByType request)
        {
            var response = new List<ResponseZoneMinify>();
            if(request != null)
            {
                var result = _zoneRepository.GetZoneByTreeViewMinifies(request.type, request.cultureCode, 0);
                foreach(var item in result)
                {
                    var _obj = new ResponseZoneMinify();
                    _obj.id = item.Id;
                    _obj.parentId = item.ParentId;
                    _obj.icon = item.Icon;
                    _obj.title = item.Name;
                    _obj.alias = item.Url;
                    _obj.mapCroods = item.MapCoords;
                    _obj.level = item.level;
                    _obj.order = item.order;
                    _obj.avatar = item.Avatar;
                    response.Add(_obj);
                }

            }
            return Ok(response);
            
        }

        [HttpPost]
        [Route("GetZoneDetailMinifyById")]
        public async Task<IActionResult> GetZoneDetailMinifyById([FromBody] RequestGetZoneDetailById request)
        {
            var response = _zoneRepository.GetZoneDetailMinifyById(request.zoneId, request.cultureCode);
            return Ok(response);

        }



    }
}
