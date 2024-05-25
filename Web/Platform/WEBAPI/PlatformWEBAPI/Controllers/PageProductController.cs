using HtmlAgilityPack;
using MI.Dal.IDbContext;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.Repository;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        private readonly IZoneRepository _zoneRepository;
        public PageProductController(IProductRepository productRepository, IZoneRepository zoneRepository)
        {
            _productRepository = productRepository;
            _zoneRepository = zoneRepository;
        }
        [HttpPost]
        [Route("GetProductDetail")]
        public async Task<IActionResult> GetProductDetail(RequestGetProductDetail request)
        {
            var response = new ProductDetail();
            if(request != null)
            {

                response = _productRepository.GetProductInfomationDetail(request.alias, request.cultureCode);

                //TourItem
                if(response != null)
                {
                    // Get review
                    var reviews = new List<ResponseReviewModel>();
                    var lichTour = response.LichTour; //Toi coi review la lich tour vi toi kha luoi
                    if (!string.IsNullOrEmpty(lichTour))
                    {
                        var rawReview = JsonConvert.DeserializeObject<List<TourItem>>(lichTour);
                        foreach(var item in rawReview)
                        {
                            var reviewItem = new ResponseReviewModel();
                            if (!string.IsNullOrEmpty(item.tieuDe))
                            {
                                var tieuDeSplited = item.tieuDe.Split("|");
                                if(tieuDeSplited.Length > 0)
                                {
                                    reviewItem.userName = tieuDeSplited[0];

                                }
                                if(tieuDeSplited.Length > 1)
                                {
                                    reviewItem.country = tieuDeSplited[1];
                                }
                            }
                            if (!string.IsNullOrEmpty(item.noiDung))
                            {
                                // Tạo một đối tượng HtmlDocument từ chuỗi HTML đầu vào
                                var doc = new HtmlDocument();
                                doc.LoadHtml(item.noiDung);

                                // Tìm tất cả các thẻ img và lấy giá trị của thuộc tính src
                                var imageNodes = doc.DocumentNode.SelectNodes("//img");
                                var imageUrls = new List<string>();

                                if (imageNodes != null)
                                {
                                    foreach (var img in imageNodes)
                                    {
                                        var src = img.GetAttributeValue("src", null);
                                        if (src != null)
                                        {
                                            imageUrls.Add(src);
                                        }
                                        // Xóa thẻ img khỏi tài liệu HTML
                                        img.Remove();
                                    }
                                }

                                // Lấy phần nội dung còn lại sau khi đã xóa các thẻ img
                                var content = doc.DocumentNode.OuterHtml;

                                reviewItem.content = content;
                                reviewItem.images = imageUrls;

                            }
                            //Noi dung su dung HTML AgilityPack
                            reviews.Add(reviewItem);
                        }

                    }
                    response.reviews = reviews;
                    // Get childs
                    response.productChilds = _productRepository.GetProductByParentId(response.Id, request.cultureCode);
                    response.productSameZones = _productRepository.GetProductSameZoneByProductId(response.Id, request.cultureCode);
                    
                }
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("GetProductDetailById")]
        public async Task<IActionResult> GetProductDetailById(RequestGetProductDetail request)
        {
            var response = new ProductDetail();
            if (request != null)
            {

                response = _productRepository.GetProductInfomationDetail(request.id, request.cultureCode);
                //TourItem
                if (response != null)
                {
                    // Get review
                    var reviews = new List<ResponseReviewModel>();
                    var lichTour = response.LichTour; //Toi coi review la lich tour vi toi kha luoi
                    if (!string.IsNullOrEmpty(lichTour))
                    {
                        var rawReview = JsonConvert.DeserializeObject<List<TourItem>>(lichTour);
                        foreach (var item in rawReview)
                        {
                            var reviewItem = new ResponseReviewModel();
                            if (!string.IsNullOrEmpty(item.tieuDe))
                            {
                                var tieuDeSplited = item.tieuDe.Split("|");
                                if (tieuDeSplited.Length > 0)
                                {
                                    reviewItem.userName = tieuDeSplited[0];

                                }
                                if (tieuDeSplited.Length > 1)
                                {
                                    reviewItem.country = tieuDeSplited[1];
                                }
                            }
                            if (!string.IsNullOrEmpty(item.noiDung))
                            {
                                // Tạo một đối tượng HtmlDocument từ chuỗi HTML đầu vào
                                var doc = new HtmlDocument();
                                doc.LoadHtml(item.noiDung);

                                // Tìm tất cả các thẻ img và lấy giá trị của thuộc tính src
                                var imageNodes = doc.DocumentNode.SelectNodes("//img");
                                var imageUrls = new List<string>();

                                if (imageNodes != null)
                                {
                                    foreach (var img in imageNodes)
                                    {
                                        var src = img.GetAttributeValue("src", null);
                                        if (src != null)
                                        {
                                            imageUrls.Add(src);
                                        }
                                        // Xóa thẻ img khỏi tài liệu HTML
                                        img.Remove();
                                    }
                                }

                                // Lấy phần nội dung còn lại sau khi đã xóa các thẻ img
                                var content = doc.DocumentNode.OuterHtml;

                                reviewItem.content = content;
                                reviewItem.images = imageUrls;

                            }
                            //Noi dung su dung HTML AgilityPack
                            reviews.Add(reviewItem);
                        }

                    }
                    response.reviews = reviews;
                    // Get childs
                    response.productChilds = _productRepository.GetProductByParentId(response.Id, request.cultureCode);
                    response.productSameZones = _productRepository.GetProductSameZoneByProductId(response.Id, request.cultureCode);

                }
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("GetProductPriceOptions")]

        public async Task<IActionResult> GetProductPriceOptions(RequestGetProductPriceOptions request)
        {
            var response = new List<ResponseGetProductPriceOptions>();
            if(request != null)
            {
                var zoneOptions = _zoneRepository.GetZoneByTreeViewMinifies((int)TypeZone.ProductOptions, request.cultureCode,0);
                var productZoneOptions = new List<ProductPriceInZoneList>();
                using (IDbContext context = new IDbContext())
                {
                    productZoneOptions = context.ProductPriceInZoneList.Where(r => r.ProductId == request.id).ToList();
                }
                if(productZoneOptions.Count > 0)
                {
                    
                    var rawZoneList = new List<int>();
                    var groupedZoneList = new List<int>();

                    var arrZoneList = productZoneOptions.Select(r => r.ZoneList);
                    foreach (var zone in arrZoneList)
                    {
                        var splited = zone.Split(',');
                        foreach(var s in splited)
                        {
                            rawZoneList.Add(int.Parse(s));
                        }
                    }

                    groupedZoneList = (from raw in rawZoneList
                                      group raw by raw into grouped
                                      select grouped.Key).ToList();

                    var rawZoneParent = new List<int>();
                    foreach(var item in groupedZoneList)
                    {
                        var affected = zoneOptions.Where(r => r.Id == item).FirstOrDefault();
                        if (affected != null)
                        {
                            rawZoneParent.Add(affected.ParentId);
                        }
                    }

                    var groupedZoneParent = (from raw in rawZoneParent
                                             group raw by raw into grouped
                                             select grouped.Key).ToList();
                    //Fecht data
                    foreach(var parent in groupedZoneParent)
                    {
                        var resItem = new ResponseGetProductPriceOptions();
                        var parentItem = zoneOptions.Where(r => r.Id == parent).FirstOrDefault();
                        if(parentItem != null)
                        {
                            resItem.zoneParent = parentItem;
                            var childItems = zoneOptions.Where(r => r.ParentId == parent).Where(r => groupedZoneList.Contains(r.Id)).ToList();
                            resItem.zoneChilds = childItems;
                            resItem.combinations = productZoneOptions;
                            response.Add(resItem);
                        }
                        
                    }



                }
            }
            return Ok(response);
        }


    }
}
