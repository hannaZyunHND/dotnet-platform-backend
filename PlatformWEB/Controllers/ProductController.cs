using MI.Dapper.Data.Models;
using MI.Entity.Common;
using MI.Entity.Enums;
using MI.Entity.ViewModel;
using MI.ES;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PlatformWEB.Services.Extra.Repository;
using PlatformWEB.Services.Locations.Repository;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Product.ViewModel;
using PlatformWEB.Services.Zone.Repository;
using PlatformWEB.Utility;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEB.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IExtraRepository _extratRepository;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ILocationsRepository _locationsRepository;
        private readonly ICookieUtility _cookieUtility;

        public ProductController(IZoneRepository zoneRepository, ILocationsRepository locationsRepository, IProductRepository productRepository, IStringLocalizer<HomeController> localizer, IExtraRepository extraRepository, ICookieUtility cookieUtility)
        {
            _localizer = localizer;
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _extratRepository = extraRepository;
            _cookieUtility = cookieUtility;
            _locationsRepository = locationsRepository;

            //Task.Run(() =>
            //{
            //    if (Utils.Utility.DateMerge < DateTime.Now)
            //    {
            //        var Check = Utils.Settings.AppSettings.GetByKey("ESEnable").ToLower();
            //        if (Check == "True".ToLower())
            //        {
            //            mergeElasticPv();
            //            Utils.Utility.DateMerge = DateTime.Now.AddHours(2);
            //        }
            //        else
            //        {
            //            Utils.Utility.DateMerge = DateTime.Now.AddHours(2);
            //        }
            //    }
            //});

        }

        //[Route("/{alias}-c{zoneId}")]
        public IActionResult ProductList(string alias, int zoneId)
        {
            var zone_selected_with_treeview = _zoneRepository.GetZoneByTreeViewMinifies(1, CurrentLanguageCode, zoneId);
            var zone_details = _zoneRepository.GetZoneDetail(zoneId, CurrentLanguageCode);
            ViewBag.ZoneTreeView = zone_selected_with_treeview;
            ViewBag.ZoneDetail = zone_details;

            return View();
        }

        private void mergeElasticPv()
        {
            var result = _productRepository.GetEsSearchResult();
            //Ghi nhan vao elastic
            bool isCreated = MI.ES.BCLES.AutocompleteService.CreateIndexAsync().Result;

            if (isCreated)
            {
                var data = MI.ES.BCLES.AutocompleteService.IndexAsync(result);
            }
        }

        [HttpGet]

        public IActionResult ElasticFilter(string keyword, int index = 1, int size = 50)
        {
            //MI.Service.SyncProductToES.Run();
            var Check = Utils.Settings.AppSettings.GetByKey("ESEnable").ToLower();
            if (Check == "True".ToLower())
            {
                long total = 0;
                var result = MI.ES.BCLES.AutocompleteService.SuggestWay2GoAsync(keyword, CurrentLanguageCode, index, size);
                //return MI.ES.BCLES.AutocompleteService.SuggestAsync(keyword, CurrentLanguageCode, 0, 10, out total);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult MergeElasticSearch()
        {
            mergeElasticPv();
            return Ok("Merge Success!");
        }

        [HttpGet]
        public ProductSuggestResponse Get(string keyword)
        {
            //MI.Service.SyncProductToES.Run();
            var Check = Utils.Settings.AppSettings.GetByKey("ESEnable").ToLower();
            if (Check == "True".ToLower())
            {
                long total = 0;
                //return MI.ES.BCLES.AutocompleteService.SuggestAsync(keyword, CurrentLanguageCode, 0, 10, out total);
                return MI.ES.BCLES.AutocompleteService.SuggestAsyncAll(keyword, CurrentLanguageCode, out total);
            }
            else return new ProductSuggestResponse();

        }
        [HttpGet]
        public ProductSuggestResponse GetElasticAll(string keyword, int index, int size)
        {
            //MI.Service.SyncProductToES.Run();
            var Check = Utils.Settings.AppSettings.GetByKey("ESEnable").ToLower();
            if (Check == "True".ToLower())
            {
                long total = 0;
                return MI.ES.BCLES.AutocompleteService.SuggestAsync(keyword, CurrentLanguageCode, index, size, out total);
            }
            else return new ProductSuggestResponse();

        }
        [HttpGet]
        public IActionResult Merge()
        {
            MI.Service.SyncProductToES.Run();
            return Ok("OK");
        }
        public IActionResult ProductFilterInHome()
        {
            //var total = 0;
            //var result = _productRepository.FilterProductBySpectificationsInZone(fp, out total);
            return View();
        }

        public IActionResult ESSearchResult()
        {
            //ViewBag.Index = index;
            //ViewBag.Size = size;
            //ViewBag.KeyWord = keyword;
            return View();
        }
        [HttpGet]
        public IActionResult ExtraHeaderMenu()
        {
            return ViewComponent("ExtraHeaderMenu");
        }
        public IActionResult ESSeachResultComponent(string keyword, int index, int size)
        {
            return ViewComponent("GetElasticAll", new { keyword = keyword, index = index, size = size });
        }
        [HttpPost]
        public IActionResult HomeQuerySearchComponent(FilterProductBySpectification fp)
        {
            return ViewComponent("GetHomeQuerySearch", new { fp = fp });
        }
        //[HttpPost]
        public IActionResult ProductDetail(string alias, int product_id)
        {
            //   ViewData["Title"] = "chi tiet ";
            var cookieUtility = _cookieUtility.SetCookieDefault();
            var current_location_id = 0;
            var current_location_name = string.Empty;
            if (cookieUtility != null)
            {
                current_location_id = _cookieUtility.SetCookieDefault().LocationId;
                current_location_name = _cookieUtility.SetCookieDefault().LocationName;
            }
            var product_infomatin_detail = _productRepository.GetProductInfomationDetail(product_id, CurrentLanguageCode);
            if (product_infomatin_detail != null)
            {
                var product_spectification_detail = _productRepository.GetProductSpectificationDetail(product_id, CurrentLanguageCode);
                var product_price_detail = _productRepository.GetProductPriceInLocationDetail(product_id, CurrentLanguageCode);
                if (product_price_detail != null)
                {
                    var location_id = Request.Cookies[CookieLocationId] == null ? product_price_detail.FirstOrDefault().LocationId : int.Parse(Request.Cookies[CookieLocationId]);
                    var same_zone_total = 0;
                    var product_same_zone = _productRepository.GetProductInZoneByZoneIdMinify(product_infomatin_detail.ZoneId, location_id, CurrentLanguageCode, 1, 6, out same_zone_total);
                    ViewBag.SameZone = product_same_zone;
                    ViewBag.SameTotal = same_zone_total;

                    var default_price_item = product_price_detail.Where(r => r.LocationId == current_location_id).FirstOrDefault();
                    ViewBag.DefaultLocationPrice = default_price_item ?? new ProductPriceInLocationDetail();

                }

                var total_row_combo = 0;
                var products_in_product_combo = _productRepository.GetProductsInProductById(product_id, "com-bo", current_location_id, CurrentLanguageCode, 1, 4, out total_row_combo);
                ViewBag.Combo = products_in_product_combo;

                var promotions_in_product = _productRepository.GetPromotionInProduct(product_id, current_location_id, CurrentLanguageCode);
                ViewBag.Promotion = promotions_in_product;

                ViewBag.Infomation = product_infomatin_detail;
                ViewBag.Zone = product_infomatin_detail.ZoneId;
                ViewBag.ZoneUrl = product_infomatin_detail.ZoneUrl;
                ViewBag.Spectification = product_spectification_detail;
                ViewBag.DefaultLocation = current_location_name;
                ViewBag.ListLocation = product_price_detail;

                var list_comment = _extratRepository.GetCommentPublisedByObjectId(product_id, (int)CommentType.Product);
                ViewBag.Comments = list_comment;
            }

            //ViewBag.DefaultPrice = product_price_detail.Where(r => r.)
            return View();
        }
        [HttpPost]
        public IActionResult GetComboByLocationId(int product_id, int location_id)
        {
            return ViewComponent("ComboInProductByLocation", new { product_id = product_id, location_id = location_id });
        }

        [HttpPost]
        public IActionResult GetProductByZoneId(int zone_Id, int location_id)
        {
            return ViewComponent("SwitchProductList", new { zone_parent_id = zone_Id, locationId = location_id });
        }
        [HttpPost]
        public IActionResult GetProductModalDetail(int product_id)
        {
            return ViewComponent("ModalBuySim", new { product_id = product_id });
        }


        [HttpPost]
        public IActionResult ViewMore(int zone_parent_id, int locationId, int skip, int size)
        {
            return ViewComponent("ViewMore", new { zone_parent_id = zone_parent_id, locationId = locationId, skip = skip, size = size });
        }
        [HttpPost]
        public IActionResult VLastSeen(string product_ids)
        {
            return ViewComponent("ProductLastSeen", new { product_ids = product_ids });
        }
        //FilterSpectificationInZoneFilterSpectificationInZone
        [HttpPost]
        public IActionResult FilterSpectificationInZone(FilterProductBySpectification fp)
        {
            return ViewComponent("FilterSpectificationInZone", new { fp = fp });
        }
        [HttpPost]
        public IActionResult FilterSpectificationInZoneListProductList(FilterProductBySpectification fp)
        {
            return ViewComponent("FilterSpectificationInZoneProductList", new { fp = fp });
        }
        public IActionResult ProductOldRenewal(string alias, int id)
        {
            int total_row = 0;
            int pageIndex = 1;
            int pageSize = 50;

            int location_id = 0;
            int.TryParse(Request.Cookies[CookieLocationId], out location_id);

            var model = _productRepository.GetProductOldRenewalDetail(id, CurrentLanguageCode, location_id);
            var modelF = model.First();
            int Zid = modelF.ZoneId ?? 0;
            var listProduct = _productRepository.GetProductInZoneByZoneIdMinifyV2(0, 0, CurrentLanguageCode, pageIndex, pageSize, out total_row).Where(x => x.ProductId != id).ToList();
            if (listProduct != null && listProduct.Any())
            {
                var firstPro = model.FirstOrDefault();
                firstPro.PriceRefer = firstPro.DefaultPrice > 0 ? firstPro.DefaultPrice : firstPro.Price;
                if (firstPro != null)
                {
                    foreach (var item in listProduct)
                    {
                        item.PriceRefer = firstPro.DefaultPrice > 0 ? firstPro.DefaultPrice : firstPro.Price;
                        item.PriceReferStr = UIHelper.FormatNumber(item.PriceRefer);
                    }
                }
                listProduct = listProduct.Where(x => x.Price >= firstPro.PriceRefer || x.DefaultPrice >= firstPro.PriceRefer).ToList();
            }
            ViewBag.ListProduct = listProduct;
            if (total_row > 10)
                ViewBag.Total_row = total_row - 10;
            var listLocation = _locationsRepository.GetLocations(CurrentLanguageCode);
            ViewBag.Locations = listLocation;
            return View(model);

        }
        [HttpGet]
        public IActionResult ListProductOldRenewal(int pageIndex, int pageSize, int ZoneId, int typeView)
        {

            return ViewComponent("ProductOldRenewal", new { pageIndex = pageIndex, pageSize = pageSize, ZoneId = ZoneId, typeView });

        }

        [HttpGet]
        public IActionResult ListProductOldRenewalJson(int pageIndex, int pageSize, int ZoneId, int typeView)
        {
            int total_row = 0;
            var model = _productRepository.GetProductOldRenewalV2(pageIndex, pageSize, CurrentLanguageCode, 0, ZoneId, out total_row);

            if (model != null && model.Any())
            {
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                foreach (var item in model)
                {
                    item.Name = !string.IsNullOrEmpty(item.Name) ? item.Name : item.Title;
                    item.Avatar = UIHelper.StoreFilePath(item.Avatar);
                    item.PriceStr = UIHelper.FormatNumber(item.Price);
                    item.Linktar = string.Format("/{0}.pn{1}.html", item.Url, item.ProductId);
                    item.DiscountStr = (item.DefaultPrice / 100 * item.DiscountPercent) > 0 ? UIHelper.FormatNumber(item.DefaultPrice / 100 * item.DiscountPercent) : "0";
                    item.DifferenceStr = UIHelper.FormatNumber(item.DefaultPrice - item.Price - (item.DefaultPrice / 100 * item.DiscountPercent));
                    item.DefaultPriceStr = UIHelper.FormatNumber(item.DefaultPrice);
                    item.JsonProduct = JsonConvert.SerializeObject(item, serializerSettings).ToString();

                }
            }
            return Json(model);

        }

        [HttpGet]
        public IActionResult ListProductOldRenewalGetAllJson(int pageIndex, int pageSize, int ZoneId, int typeView, string firstPrice, int oldProductId)
        {
            int total_row = 0;
            var model = _productRepository.GetProductMinifiesTreeViewByZoneParentId(ZoneId, CurrentLanguageCode, 0, pageIndex, pageSize, out total_row);

            if (model != null && model.Any())
            {
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                foreach (var item in model)
                {
                    item.Avatar = UIHelper.StoreFilePath(item.Avatar);
                    item.PriceStr = UIHelper.FormatNumber(item.Price);
                    item.Linktar = string.Format("/{0}.html", item.Url);
                    item.DiscountStr = (item.DefaultPrice / 100 * item.DiscountPercent) > 0 ? UIHelper.FormatNumber(item.DefaultPrice / 100 * item.DiscountPercent) : "0";
                    item.DifferenceStr = UIHelper.FormatNumber(item.Price - Convert.ToDecimal(string.IsNullOrEmpty(firstPrice) ? "0" : firstPrice) - (item.DefaultPrice / 100 * item.DiscountPercent));
                    item.DefaultPriceStr = UIHelper.FormatNumber(item.DefaultPrice > 0 ? item.DefaultPrice : item.Price);
                    item.PriceReferStr = UIHelper.FormatNumber(Convert.ToDecimal(firstPrice));
                    item.OldProductId = oldProductId;
                    item.JsonProduct = JsonConvert.SerializeObject(item, serializerSettings).ToString();

                }

                model = model.Where(x => x.DefaultPrice >= Convert.ToDecimal(firstPrice) || x.Price >= Convert.ToDecimal(firstPrice)).ToList();
            }
            return Json(model);

        }
        [HttpPost]
        public IActionResult FilterProductByNameAutocomplete(string filter, int pageSize)
        {
            return null;
        }

        [HttpPost]
        public JsonResult AddCustomerProductOldRenewalRequest(int productId, string fullName, string phoneNumber, string email, int locationId, int type, int departmentId, decimal? dealPrice, int? productIdToExchange)
        {
            var product = new CustomerProductOldRenewal()
            {
                productId = productId,
                email = email,
                fullName = fullName,
                locationId = locationId,
                phoneNumber = phoneNumber,
                type = type,
                departmentId = departmentId,
                dealPrice = dealPrice,
                productIdToExchange = productIdToExchange
            };
            return Json(new { Result = _productRepository.InsertCustomerProductOldRenewal(product) });
        }

        [HttpPost]
        public IActionResult GetDetailInfomation(int productId)
        {
            dynamic result = new System.Dynamic.ExpandoObject(); ;
            var detail = _productRepository.GetProductInfomationDetail(productId, CurrentLanguageCode);
            if (detail != null)
                detail.Avatar = UIHelper.StoreFilePath(detail.Avatar, false);
            var specs = _productRepository.GetProductSpectificationDetail(productId, CurrentLanguageCode);

            result.detail = detail;
            result.specs = specs;

            return Json(result);
        }

        [HttpPost]
        public IActionResult GetProductChildInProductParent(int productId, int locationId)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            var lstChildData = new List<ProductMinify>();
            var firstData = _productRepository.GetProductChildInProductParent(productId, locationId);
            foreach (var item in firstData)
            {
                var lstobj = _productRepository.GetProductChildInProductParent(item.ProductId, locationId);
                lstChildData.AddRange(lstobj);
            }
            result.phienBan = firstData;
            result.phienBanMau = lstChildData;

            return Json(result);
        }

        [HttpPost]
        public IActionResult GetPromotionByLocation(int productId, int locationId)
        {
            var result = _productRepository.GetPromotionInProduct(productId, locationId, CurrentLanguageCode);
            return Json(result);
        }


    }
}