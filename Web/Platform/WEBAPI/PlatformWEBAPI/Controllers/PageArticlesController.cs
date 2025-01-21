using HtmlAgilityPack;
using MI.Dal.IDbContext;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.Article.ViewModel;
using PlatformWEBAPI.Services.Product.Repository;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageArticlesController : ControllerBase
    {

        private readonly IArticleRepository _articleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        public PageArticlesController(IArticleRepository articleRepository, IProductRepository productRepository, IHostingEnvironment hostingEnvironment)
        {
            _articleRepository = articleRepository;
            _productRepository = productRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("GetBlogDetailById")]
        public async Task<IActionResult> GetBlogDetailById(RequestGetBlogDetailById request)
        {
            if (request != null)
            {
                var response = _articleRepository.GetArticleDetail(request.id, request.cultureCode);
                if (response != null)
                {
                    var newBody = RenderProductInBlogDetail(response.Body, request.cultureCode);
                    response.Body = newBody;
                    return Ok(response);
                }
            }
            return BadRequest();
        }


        private string RenderProductInBlogDetail(string body, string cultureCode)
        {
            //data-id-list
            HtmlDocument doc = new HtmlDocument();
            if (!string.IsNullOrEmpty(body))
            {
                doc.LoadHtml(body);
                var productTags = doc.DocumentNode.SelectNodes("//product");
                if (productTags != null)
                {
                    foreach (var pTag in productTags)
                    {
                        var listId = pTag.GetAttributeValue("data-id-list", "");
                        if (!string.IsNullOrEmpty(listId))
                        {
                            var listIdString = listId.Split(",");
                            var listIdInt = new List<int>();
                            foreach (var i in listIdString)
                            {
                                var t = 0;
                                int.TryParse(i, out t);
                                if (t > 0)
                                {
                                    listIdInt.Add(t);
                                }
                            }
                            var products = _productRepository.GetProductByListId(listIdInt, cultureCode, 0);
                            var html = "";
                            if (products != null)
                            {
                                var from = "From";
                                var bookNow = "Book Now";
                                var langShortUrl = "en";
                                if (cultureCode == "vi-VN")
                                {
                                    from = "Từ";
                                    bookNow = "Đặt ngay";
                                    langShortUrl = "vi";
                                }


                                html += $"<div class='container'>";
                                foreach (var prod in products)
                                {
                                    html += $"<div class='row blog-gt'>";
                                    html += $"<div class='col-12 col-md-3 mb-3 mb-md-0'>";
                                    html += $"<img class='img-blog img-fluid' src='{UIHelper.StoreFilePath(prod.Avatar, false)}' />";
                                    html += $"</div>";
                                    html += $"<div class='col-12 col-md-9 thong-tin-blog'>";
                                    html += $"<div>";
                                    html += $"<div class='title-blog'>{prod.Title}</div>";
                                    html += $"<div class='danh-gia-blog'>";
                                    html += $"<i class='fas fa-star sao-blog' style='color: #f09b0a;'></i>";
                                    html += $"<span style='letter-spacing: 0.2px; color: #f09b0a; font-size: 14px; font-weight: 400;'>{Math.Round(prod.Rate, 1)}</span>";
                                    html += $"</div>";
                                    html += $"</div>";
                                    html += $"<div class='row price-blog'>";
                                    html += $"<div class='col-6 inner-footer'>";
                                    html += $"<div class='inner-price'><span class='from'>{from} </span><span class='gia-ca'><small>VND</small> {UIHelper.FormatNumber(prod.Price)}</span></div>";
                                    html += $"</div>";
                                    html += $"<div class='col-6 bao-button text-md-right mt-2 mt-md-0'>";
                                    html += $"<a href='/{langShortUrl}/products/{prod.ProductId}/{prod.Url}' class='btn inner-button-book'>{bookNow}</a>";
                                    html += $"</div>";
                                    html += $"</div>";
                                    html += $"</div>";
                                    html += $"</div>";
                                }
                                html += $"</div>";
                            }
                            // Tôi muốn set html của pTag ở đây thành biến html mà tôi đã dựng
                            pTag.InnerHtml = html;
                        }
                    }
                }
                var tableTags = doc.DocumentNode.SelectNodes("//table");
                if(tableTags != null)
                {
                    foreach(var tb in tableTags)
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
                if(aTags != null)
                {
                    foreach(var atag in aTags)
                    {
                        atag.Attributes.Add("target", "_blank");
                    }
                }
                return doc.DocumentNode.InnerHtml;
            }
            return body;

        }

        [HttpPost]
        [Route("GetBlogsSameZone")]
        public async Task<IActionResult> GetBlogsSameZone(RequestGetBlogsSameZone request)
        {
            if (request != null)
            {
                var _t = 0;

                var response = _articleRepository.GetArticlesInZoneId_Minify_FullFilter(request.zoneId, (int)TypeZone.All, request.type, 0, request.cultureCode, "", 1, 6, out _t);
                response = response.Where(r => r.Id != request.blogId).ToList();
                return Ok(response);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("GetBlogsInZone")]
        public async Task<IActionResult> GetBlogsInZone(RequestGetBlogsInZone request)
        {
            if (request != null)
            {
                var _t = 0;

                //Get Zone by Alias
                var zoneId = 0;
                if (!string.IsNullOrEmpty(request.zoneUrl))
                {
                    using (IDbContext context = new IDbContext())
                    {
                        var zoneInLang = await (from z in context.Zone
                                                join zil in context.ZoneInLanguage on z.Id equals zil.ZoneId
                                                where zil.Url.Equals(request.zoneUrl) && zil.LanguageCode.Equals(request.cultureCode) && z.Type == (int)TypeZone.Article
                                                select zil).FirstOrDefaultAsync();
                        if (zoneInLang != null)
                        {
                            zoneId = zoneInLang.ZoneId;
                        }
                    }
                }
                
                var result = _articleRepository.GetArticlesInZoneId_Minify_FullFilter(zoneId, (int)TypeZone.All, (int)TypeArticle.All, 2, request.cultureCode, "", request.pageIndex, request.pageSize, out _t);
                //response = response.Where(r => r.Id != request.blogId).ToList();

                dynamic response = new ExpandoObject();
                var totalPage = _t / request.pageSize;
                if (_t % request.pageSize > 0)
                {
                    totalPage++;
                }
                response.totalPage = totalPage;
                response.firstItem = result.FirstOrDefault();
                response.nextThreeItem = result.Skip(1).Take(3).ToList();
                response.lastItems = result.Skip(4).ToList();
                response.totalItems = _t;
                return Ok(response);
            }
            return BadRequest();
        }

    }
}
