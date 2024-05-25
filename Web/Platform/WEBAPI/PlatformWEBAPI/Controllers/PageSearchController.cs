using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal;
using PlatformWEBAPI.Services.PageSearch.ViewModel;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageSearchController : ControllerBase
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        public PageSearchController(IZoneRepository zoneRepository, IProductRepository productRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
        }
        [HttpPost]
        [Route("GetSearchableZone")]
        public async Task<IActionResult> GetSearchableZone(RequestGetSearchableZone request)
        {
            var response = new List<ZoneByTreeViewMinify>();
            if(request != null)
            {
                var zoneDiemDens = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.DiemDen, request.cultureCode, 0);
                var zoneDanhMucs = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.Product, request.cultureCode, 0);
                response.AddRange(zoneDiemDens);
                response.AddRange(zoneDanhMucs);
            }
            return Ok(response);
        }
        

        [HttpPost]
        [Route("GetProductBySearchdZone")]
        public async Task<IActionResult> GetProductBySearchdZone(RequestGetProductBySearchdZone request)
        {
            var response = new ResponseGetProductBySearchdZone();
            if(request != null)
            {
                var total = 0;
                var products = _productRepository.GetProductBySearchdZone(request.selectedZones,request.cultureCode, request.pageIndex, request.pageSize, out total);
                response.products = products;
                response.total = total;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("GetProductByKeywords")]
        public async Task<IActionResult> GetProductByKeywords(RequestGetProductByKeywords request)
        {
            var response = new ResponseGetProductByKeywords();
            if(request != null)
            {
                var total = 0;
                var products = _productRepository.GetProductByKeywords(request.keywords,request.selectedZones, request.cultureCode, request.pageIndex, request.pageSize, out total);
                response.products = products;
                response.total = total;
            }
            return Ok(response);
        }
    }
}
