using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal;
using PayPalCheckoutSdk.Orders;
using PlatformWEBAPI.Services.PageSearch.ViewModel;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var zoneRegion = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.Region, request.cultureCode, 0);
                response.AddRange(zoneDiemDens);
                response.AddRange(zoneDanhMucs);
                response.AddRange(zoneRegion);
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
                var mapTotal = 0;
                var products = new List<ProductMinify>();
                var productsMap = new List<ProductMapLocation>();

                var countSearchZone = 0;
                foreach(var item in request.selectedZones)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        countSearchZone++;
                    }
                }

                var typeSearch = "";
                if(request.keywords.Count == 0 && countSearchZone == 2)
                {
                    typeSearch = "SEARCH_ONLY_ZONE";
                }
                if(request.keywords.Count > 0 && countSearchZone == 0)
                {
                    typeSearch = "SEARCH_ONLY_ELASTIC";
                }
                if(request.keywords.Count == 0 && (countSearchZone > 0 && countSearchZone <= 2)){
                    typeSearch = "SEARCH_ONLY_ZONE";
                }

                if (request.keywords.Count > 0 && (countSearchZone > 0 && countSearchZone <= 2))
                {
                    typeSearch = "SEARCH_KET_HOP";
                }
                if(request.keywords.Count == 0 && countSearchZone == 0)
                {
                    typeSearch = "SEARCH_KET_HOP";
                }
                Console.WriteLine("NEO DEBUGG");
                if(typeSearch == "SEARCH_ONLY_ZONE" || typeSearch == "SEARCH_KET_HOP")
                {
                    products = _productRepository.GetProductByKeywords(request.keywords, request.selectedZones, request.cultureCode, request.pageIndex, request.pageSize, out total, request.sortBy, request.startPrice, request.endPrice);
                    productsMap = _productRepository.GetProductByKeywordsALLMAP(request.keywords, request.selectedZones, request.cultureCode, request.pageIndex, request.pageSize, out mapTotal, request.sortBy, request.startPrice, request.endPrice);
                    foreach (var item in products)
                    {
                        var googleMapCroodSplited = item.googleMapCrood.Split("-");
                        if (googleMapCroodSplited.Length == 2)
                        {
                            item.lat = decimal.Parse(googleMapCroodSplited[0]);
                            item.lng = decimal.Parse(googleMapCroodSplited[1]);

                        }
                    };
                }
                if(typeSearch == "SEARCH_ONLY_ELASTIC" || typeSearch == "SEARCH_KET_HOP")
                {
                    if (request.keywords.Count > 0)
                    {
                        var _firstKeyword = request.keywords.FirstOrDefault();

                        if (_firstKeyword != null)
                        {
                            var esResult = MI.ES.BCLES.AutocompleteService.SuggestJoytimeAsync(_firstKeyword, request.cultureCode, request.pageIndex, request.pageSize, "PRODUCT");
                            if (esResult != null)
                            {
                                //KHong phai la group by
                                //Ma phai loc ra cac thang co diem chung

                                var esResultId = esResult.Select(r => r.itemId);
                                //Group by 
                                var productsId = products.Select(r => r.ProductId).ToList();
                                if (productsId.Count > 0)
                                {
                                    esResultId = from e in esResultId
                                                 join p in productsId on e equals p
                                                 group e by e into grouped
                                                 select grouped.Key;
                                }

                                esResultId = esResultId.Where(r => !productsId.Contains(r));

                                var p_EsResult_ids = esResultId.ToList();
                                if (p_EsResult_ids != null)
                                {
                                    var p_esResult_products = _productRepository.GetProductByListIdVersionSearch(p_EsResult_ids, request.cultureCode, 0, request.sortBy, request.startPrice, request.endPrice);
                                    foreach(var item in p_esResult_products)
                                    {
                                        var m = new ProductMapLocation();
                                        m.Id = item.ProductId;
                                        m.googleMapCrood = item.googleMapCrood;

                                        productsMap.Add(m);
                                    }
                                    products.AddRange(p_esResult_products);
                                    total += p_esResult_products.Count;
                                }
                            }
                        }

                    }
                }

                foreach(var item in productsMap)
                {
                    if (!string.IsNullOrEmpty(item.googleMapCrood))
                    {
                        var splitted = item.googleMapCrood.Split("-");
                        if(splitted.Length == 2)
                        {
                            decimal lat = 0;
                            decimal lng = 0;
                            decimal.TryParse(splitted[0],out lat);
                            decimal.TryParse(splitted[1], out lng);
                            item.lat = lat;
                            item.lng = lng;
                        }
                    }
                }
                

                
                
                //if (request.selectedZones.Count > 0)
                //{
                //    if(request.selectedZones.Where(r => string.IsNullOrEmpty(r)).Count() <= 1)
                //    {
                        
                //    }
                    
                //}

                
                response.products = products;
                response.itemMaps = productsMap;
                response.total = total;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("GetProductMinifyById")]
        public async Task<IActionResult> GetProductMinifyById(RequestGetProductMinifyById request)
        {
            if(request != null)
            {
                if(request.productId > 0)
                {
                    var result = _productRepository.GetProductMinifyById(request.productId, request.cultureCode);
                    return Ok(result);
                }
            }
            return BadRequest();
        }

    }
}
