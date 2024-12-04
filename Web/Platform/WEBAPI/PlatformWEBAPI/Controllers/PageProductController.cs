using HtmlAgilityPack;
using MI.Dal.IDbContext;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Newtonsoft.Json;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web;
using Utils;

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
        /*
         private  string ConvertToCorrectEncoding(string input)
        {
            // Giải mã các ký tự HTML entities thành chuỗi Unicode
            string decodedString = HttpUtility.HtmlDecode(input);

            return decodedString;

        }
         */
        public async Task<IActionResult> GetProductDetailById(RequestGetProductDetail request)
        {
            var response = new ProductDetail();
            if (request != null)
            {
                response = _productRepository.GetProductInfomationDetail(request.id, request.cultureCode);
                
                //TourItem
                if (response != null)
                {
                    //Xay dung shortCode o day
                    //
                    //
                    //


                    //Social Description
                    response.SocialDescription = HttpUtility.HtmlDecode(UIHelper.ClearHtmlTag(response.Description));
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
                                if(tieuDeSplited.Length > 2)
                                {
                                    var starNo = 0;
                                    int.TryParse(tieuDeSplited[2], out starNo);
                                    reviewItem.startNumber = starNo;
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
                                if(imageUrls.Count > 0)
                                {
                                    reviewItem.avatar = imageUrls[0];
                                }
                                reviewItem.content = content;
                                reviewItem.images = imageUrls.Skip(1).ToList();

                            }
                            //Noi dung su dung HTML AgilityPack
                            reviews.Add(reviewItem);
                        }

                    }
                    response.reviews = reviews;
                    // Get childs
                    response.productChilds = _productRepository.GetProductByParentId(response.Id, request.cultureCode);
                    response.productSameZones = _productRepository.GetProductSameZoneByProductId(response.Id, request.cultureCode);
                    // Get bookingNote



                    var bookingNotes = _productRepository.GetProductBookingNote(response.Id, request.cultureCode);
                    foreach(var item in bookingNotes){
                        item.noteOptionItems = new List<NoteOptionItem>();
                        if (!string.IsNullOrEmpty(item.noteOptions))
                        {
                            var splited = item.noteOptions.Split(",");
                            foreach(var s in splited)
                            {
                                item.noteOptionItems.Add(new NoteOptionItem() { label = s, value = s });
                            }
                        }
                    }
                    if(bookingNotes.Count > 0)
                    {
                        response.productBookingNoteGroups = (from b in bookingNotes
                                                        group b by new { b.ZoneParentId, b.ZoneParentName } into g
                                                        select new ProductBookingNoteGroup
                                                        {
                                                            ZoneParentId = g.Key.ZoneParentId,
                                                            ZoneParentName = g.Key.ZoneParentName,
                                                            NoteList = g.ToList()
                                                        }).ToList();
                    }

                    // Get feedback
                    var totalFeedback = 0;
                    var rqFeedback = new RequestGetProductCommentFeedback() { productId = response.Id, index = 1, size= 5 };
                    response.feedbacks = _productRepository.GetProductCommentFeedback(rqFeedback, out totalFeedback);
                    response.totalFeedback = totalFeedback;
                    // 
                    
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
                            var childItems = zoneOptions.Where(r => r.ParentId == parent).Where(r => groupedZoneList.Contains(r.Id)).OrderBy(r => r.SortOrder).ToList();
                            resItem.zoneChilds = childItems;
                            resItem.combinations = productZoneOptions;
                            response.Add(resItem);
                        }
                        
                    }



                }
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("GetProductOptionsPriceByDate")]
        public async Task<IActionResult> GetProductOptionsPriceByDate(RequestGetProductOptionsPriceByDate request)
        {
            var response = new List<ProductPriceInZoneListByDateWithIsPass>();
            using (IDbContext context = new IDbContext())
            {
                
                if(request != null)
                {
                    var _r = context.ProductPriceInZoneListByDate.Where(r => r.ProductId == request.id && r.ZoneList.Equals(request.combination) && r.Date.Month == request.month && r.Date.Year == request.year).ToList();
                    var mapperStrirng = Newtonsoft.Json.JsonConvert.SerializeObject(_r);
                    if (!string.IsNullOrEmpty(mapperStrirng))
                    {
                        response = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductPriceInZoneListByDateWithIsPass>>(mapperStrirng);
                    }
                }
                //Tinh luon pass o day
                var lastMinuteInfomation = context.ProductPriceInZoneList.Where(r => r.ProductId == request.id && r.ZoneList.Equals(request.combination)).FirstOrDefault();
                if (lastMinuteInfomation != null)
                {
                    var lastMinuteDays = lastMinuteInfomation.LastMinuteSetupDay;
                    var lastMinuteTime = lastMinuteInfomation.LastMinuteSetupTime;
                    var minimumDate = DateTime.Now.Date;
                    //Tinh toan de kiem tra ngay la lastMinute

                    if(lastMinuteDays != null)
                    {
                        minimumDate = minimumDate.AddDays(lastMinuteDays.Value);
                        
                    }
                    foreach (var item in response)
                    {
                        if (item.Date < minimumDate.Date)
                        {
                            item.isPast = true;
                        }
                        if(item.PriceEachNguoiLon == 0)
                        {
                            item.isPast = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(lastMinuteTime))
                    {
                        var lastMinuteTimeSplited = lastMinuteTime.Split(":");
                        if(lastMinuteTimeSplited.Length == 2)
                        {
                            var hour = 0;
                            var minute = 0;

                            var checkerHour = int.TryParse(lastMinuteTimeSplited[0], out hour);
                            var checkerMinute = int.TryParse(lastMinuteTimeSplited[1], out minute);

                            if(checkerHour && checkerMinute)
                            {
                                minimumDate = minimumDate.AddHours(hour).AddMinutes(minute);
                                var _now = DateTime.Now;

                                if(_now > minimumDate)
                                {
                                    //Cho thang ngay bang bi disable
                                    var af = response.Where(r => r.Date.Date == _now.Date).FirstOrDefault();
                                    if(af != null)
                                    {
                                        af.isPast = true;
                                    }
                                }
                                
                            }
                        }
                    }

                }


            }
            return Ok(response);
        }

        [HttpPost]
        [Route("AddViewCount")]
        public async Task<IActionResult> AddViewCount(RequestAddViewCount request)
        {
            using (IDbContext context = new IDbContext())
            {
                if(request != null)
                {
                    if (request.productId > 0)
                    {
                        var p = await context.Product.FindAsync(request.productId);
                        if (p != null)
                        {
                            p.ViewCount = p.ViewCount + 1;
                            context.Product.Update(p);
                            await context.SaveChangesAsync();
                            return Ok("Add viewcount Succesful!");
                        }
                    }
                }
                
            }
            return BadRequest("Cannot find product");
        }

        [HttpPost]
        [Route("GetProductCommentFeedback")]
        public async Task<IActionResult> GetProductCommentFeedback(RequestGetProductCommentFeedback request)
        {
            var total = 0;
            var responese = _productRepository.GetProductCommentFeedback(request, out total);
            return Ok(responese);
        }


    }
}

