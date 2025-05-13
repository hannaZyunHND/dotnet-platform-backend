using HtmlAgilityPack;
using MI.Entity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System.Collections.Generic;
using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

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
                                zoneDetail.Content = RenderZoneDetail(zoneDetail.Content, request.cultureCode);
                                zoneDetail.breadcrumbs = zoneDetail.breadcrumbs.Where(r => r.ParentId > 0).ToList();
                            }
                        }
                        return Ok(zoneDetail);
                    }

                }

            }
            return Ok(null);
        }

        private string RenderZoneDetail(string body, string cultureCode)
        {
            //data-id-list
            HtmlDocument doc = new HtmlDocument();
            if (!string.IsNullOrEmpty(body))
            {
                doc.LoadHtml(body);
                
                var tableTags = doc.DocumentNode.SelectNodes("//table");
                if (tableTags != null)
                {
                    foreach (var tb in tableTags)
                    {
                        tb.AddClass("table");
                        tb.AddClass("table-bordered");

                        // Tạo thẻ div với class table-responsive
                        var divNode = HtmlNode.CreateNode("<div class='table-responsive'></div>");

                        // Thêm thẻ table vào bên trong div
                        divNode.AppendChild(tb.Clone());

                        // Thay thế thẻ table cũ bằng thẻ div mới
                        tb.ParentNode.ReplaceChild(divNode, tb);
                    }
                }

                var aTags = doc.DocumentNode.SelectNodes("//a");
                if (aTags != null)
                {
                    foreach (var atag in aTags)
                    {
                        atag.Attributes.Add("target", "_blank");
                    }
                }
                return doc.DocumentNode.InnerHtml;
            }
            return body;

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
