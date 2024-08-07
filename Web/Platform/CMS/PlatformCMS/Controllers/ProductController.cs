using MI.Bo.Bussiness;
using MI.Dal.IDbContext;
using MI.Dapper.Data.Models;
using MI.Dapper.Data.Repositories.Interfaces;
using MI.Entity.Common;
using MI.Entity.Enums;
using MI.Entity.Models;
using MI.ES;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductInLanguageBCL productInLanguageBCL;
        LocationBCL locationBCL;
        ZoneInLanguageBCL zoneInLanguageBCL;
        ZoneBCL zoneBCL;
        ProductPriceInLocationBCL priceInLocationBCL;
        ProductBCL productBCL;
        TagInProductBCL tagInProductBCL;
        ProductInZoneBCL productInZoneBCL;
        PropertyBCL propertyBCL;
        ArticleBCL articleBCL;
        IConnectionMultiplexer _multiplexer;
        IConfiguration _configuration;
        private readonly IProductRepository _productRepository;



        public ProductController(IProductRepository productRepository, IConnectionMultiplexer multiplexer, IConfiguration configuration)
        {
            productBCL = new ProductBCL();
            productInLanguageBCL = new ProductInLanguageBCL();
            zoneInLanguageBCL = new ZoneInLanguageBCL();
            tagInProductBCL = new TagInProductBCL();
            priceInLocationBCL = new ProductPriceInLocationBCL();
            locationBCL = new LocationBCL();
            productInZoneBCL = new ProductInZoneBCL();
            propertyBCL = new PropertyBCL();
            zoneBCL = new ZoneBCL();
            articleBCL = new ArticleBCL();
            _multiplexer = multiplexer;
            _configuration = configuration;
            _productRepository = productRepository;
        }


        [HttpGet("GetListProductParent")]
        public ResponseData GetListProductParent()
        {
            var responseData = new ResponseData();
            var data = productBCL.FindAll(x => x.ParentId == 0);
            responseData.Success = true;
            responseData.Message = "Thành công";
            responseData.ListData = data.Select(x => new { id = x.Id, label = x.Name }).ToList<object>();
            return responseData;
        }


        [HttpGet("GetAll")]
        public ResponseData GetAll()
        {
            ResponseData responseData = new ResponseData();
            try
            {
                var data = productBCL.FindAll();
                responseData.Success = true;
                responseData.Message = "Thành công";
                responseData.ListData = data.ToList<object>();
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }
        [HttpGet("GetPropertyProduct")]
        public IActionResult GetPropertyProduct()
        {
            var lstStatus = propertyBCL.FindAll();
            return Ok(lstStatus.Select(x => new { id = x.Id, label = x.Name }).ToList<object>());

        }

        [HttpGet("Supports")]
        public ResponseSuports Supports()
        {
            ResponseSuports responseData = new ResponseSuports();
            try
            {
                var lstStatus = MI.Entity.EnumHelper.ToList(typeof(MI.Entity.Enums.StatusProduct), true);
                responseData.ListStatus = lstStatus.Cast<object>().ToList();
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }

        [HttpGet("GetPriceByLocation")]
        public ResponseTwoList GetPriceByLocation(int idProduct)
        {
            ResponseTwoList responseData = new ResponseTwoList();
            try
            {
                var locations = locationBCL.FindAll().ToDictionary(x => x.Id, x => x);
                var locationPrice = priceInLocationBCL.FindAll(x => x.ProductId == idProduct);

                responseData.ListObj1 = locations.Values.Where(x =>
                        !locationPrice.Select(d => d.LocationId).Contains(x.Id)).Select(x => new
                        { Id = 0, LocationId = x.Id, Price = 0, SalePrice = 0, Ten = x.Name, ProductId = idProduct })
                    .ToList<object>();
                responseData.ListObj2 = locationPrice.Select(x => new
                {
                    x.Id,
                    x.LocationId,
                    x.Price,
                    x.ProductId,
                    x.SalePrice,
                    Ten = locations.ContainsKey(x.LocationId) ? (locations[x.LocationId].Id == 0 ? "Tất cả" : locations[x.LocationId].Name) : ""
                }).ToList<object>();
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }


        [HttpPost("Search")]
        public ResponsePaging Search([FromBody] FilterProduct filter)
        {
            ResponsePaging responseData = new ResponsePaging();
            try
            {
                int total = 0;

                var data = productInLanguageBCL.FindAll(filter, out total).Where(x => x.Status != 0);
                var lstId = data.Select(x => x.ProductInZone).SelectMany(d => d).ToList().Select(x => x.ZoneId);
                var dicZone = zoneBCL.GetById(lstId.Distinct().ToList(), filter.languageCode)
                    .ToDictionary(x => x.Id, z => z.Name);

                responseData.ListData = data.Select(x => new
                {
                    x.Id,
                    Name = x.Name,
                    Lang = x.LangCount,
                    x.Avatar,
                    x.Status,
                    price = Utils.Utility.ConvertCurentce(x.Price.ToString()),
                    category = Utils.Utility.BuildZone(x.ProductInZone.Select(d => d.ZoneId).ToList(), dicZone),
                    x.Code,
                    x.ViewCount,
                    x.SortOrder,
                    x.ArticleId,
                    BaseUrl = x.ListUrl.Select(d => new KeyValuePair<string, string>(d.Key, $"{Utils.Utility.BaseDomain(Utils.BaseBA.UrlProductJanhome("", d.Value))}")).ToList()
                }).ToList<object>();
                responseData.Total = total;
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }


        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            ViewModel.Products responseData = new ViewModel.Products();
            try
            {
                var objPro = productBCL.FindById(x => x.Id == id, x => x.ProductInZone);

                if (objPro != null)
                {
                    var listZoneDiemDen = zoneBCL.FindAll().Where(r => r.Type == (int)TypeZone.DiemDen).Select(r => r.Id).ToList();
                    var listZoneOption = zoneBCL.FindAll().Where(r => r.Type == (int)TypeZone.ProductOptions).Select(r => r.Id).ToList();
                    var listZoneTag = zoneBCL.FindAll().Where(r => r.Type == (int)TypeZone.Tag).Select(r => r.Id).ToList();
                    var listZoneNote = zoneBCL.FindAll().Where(r => r.Type == (int)TypeZone.Note).Select(r => r.Id).ToList();
                    var listZoneCoupon = zoneBCL.FindAll().Where(r => r.Type == (int)TypeZone.Discount).Select(r => r.Id).ToList();
                    responseData.ListCat = objPro.ProductInZone.OrderByDescending(x => x.IsPrimary).Select(x => x.ZoneId).Where(r => !listZoneDiemDen.Contains(r) && !listZoneOption.Contains(r) && !listZoneTag.Contains(r) && !listZoneNote.Contains(r) && !listZoneCoupon.Contains(r)).ToList();
                    responseData.ListDiemDen = objPro.ProductInZone.OrderByDescending(x => x.IsPrimary).Select(x => x.ZoneId).Where(r => listZoneDiemDen.Contains(r)).ToList();
                    responseData.ListProductOption = objPro.ProductInZone.OrderByDescending(x => x.IsPrimary).Select(x => x.ZoneId).Where(r => listZoneOption.Contains(r)).ToList();
                    responseData.ListTagOption = objPro.ProductInZone.OrderByDescending(x => x.IsPrimary).Select(x => x.ZoneId).Where(r => listZoneTag.Contains(r)).ToList();
                    responseData.ListNoteOption = objPro.ProductInZone.OrderByDescending(x => x.IsPrimary).Select(x => x.ZoneId).Where(r => listZoneNote.Contains(r)).ToList();
                    responseData.ListCouponZoneOptions = objPro.ProductInZone.OrderByDescending(x => x.IsPrimary).Select(x => x.ZoneId).Where(r => listZoneCoupon.Contains(r)).ToList();
                    responseData.ListProp = Utils.Utility.SplitStringToListInt(objPro.PropertyId);
                    responseData.ListColor = Utils.Utility.SplitStringToList(objPro.Color);
                    responseData.ListArticle = MI.Cache.RamCache.DicArticle.ToList();
                    objPro.ProductInZone = new List<ProductInZone>();
                    if (String.IsNullOrEmpty(objPro.MetaFile))
                    {
                        objPro.MetaFile = JsonConvert.SerializeObject(new MI.Entity.CustomClass.FileProduct());
                    }
                    responseData.Data = objPro;
                }
            }
            catch (Exception ex)
            {
            }

            return Ok(responseData);
        }

        [HttpGet("GetLanguageById")]
        public ResponseData GetLanguageById(int id)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                var ProductInLanguage = productInLanguageBCL.GetByProductId(id);
                if (ProductInLanguage != null && ProductInLanguage.Count > 0)
                {
                    var TagName = tagInProductBCL.FindByProductId(ProductInLanguage.Select(x => x.Id).ToList());

                    responseData.Success = true;
                    responseData.Message = "Thành công";
                    responseData.ListData = ProductInLanguage.Select(x =>
                            new ProductInLanguage(x, TagName.ContainsKey(x.Id) ? TagName[x.Id] : new List<string>()))
                        .ToList<object>();
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }

        [HttpPost("Add")]
        public ResponseData Add([FromBody] MI.Entity.Models.Product product)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                //insert product
                product.CreatedBy = product.ModifyBy;
                product.Status = (int)MI.Entity.Enums.StatusProduct.NotPublic;
                product.CreatedDate = DateTime.Now;
                product.ParentId = 0;
                var countProduct = 0;
                using (IDbContext context = new IDbContext())
                {
                    countProduct = context.Product.Count();
                }
                

                if (String.IsNullOrWhiteSpace(product.Code))
                    {
                    product.Code = CreateProductCode(countProduct);
                    }
                var respone = Utils.Utility.AutoCheckSlug(product.Name, product.Url);
                if (respone.Key)
                {
                    product.Url = respone.Value;
                    product.Status = 2;
                    product.Avatar = Utils.Utility.RemoveDomainImage(product.Avatar);


                    int result1 = productBCL.Add(product, product.ProductInZone.ToList());
                    if (result1 > 0)
                    {
                        //Update Cancel Policy
                        responseData.Success = true;
                        responseData.Message = "Thành công";
                        responseData.Id = result1;
                    }
                    else
                    {
                        if (result1 == -1)
                        {
                            responseData.Success = false;
                            responseData.Message = "Mã sản phẩm đã tồn tại";
                        }
                        else
                        {
                            responseData.Success = false;
                            responseData.Message = "Thất bại";
                        }

                    }
                }
                else
                {
                    responseData.Message = respone.Value;
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }

        private string CreateProductCode(int count)
        {
            // Tăng giá trị của count lên 1
            int orderNumber = count + 1;

            // Tạo chuỗi định dạng với 5 chữ số, có thêm các số 0 ở đầu nếu cần
            string productCode = $"SP_{orderNumber:D5}";

            return productCode;
        }

        [HttpPut("Update")]
        public async Task<ResponseData> UpdateAsync([FromBody] UpdateProductParameterWithPhienBan request)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                var product = request.Product;
                var phienBans = request.PhienBans;
                var respone = Utils.Utility.AutoCheckSlug(product.Name, product.Url);
                if (respone.Key)
                {
                    product.Url = respone.Value;
                    product.ModifyDate = DateTime.Now;
                    int result = productBCL.Update(product, product.ProductInZone.ToList());
                    if (result > 0)
                    {
                        // Update phien ban o day
                        var updatedPhienBan = productBCL.UpdatePhienBan(request.PhienBans, result);
                        // Update Cancel Policy
                        foreach(var item in request.CancelPolicies)
                        {
                            item.ProductId = product.Id;

                        }
                        productBCL.UpdateCancelPolicies(request.CancelPolicies);
                        responseData.Success = true;
                        responseData.Message = "Thành công";
                        responseData.Id = result;
                        MI.Cache.RedisUtils.DeleteAllCacheAsyn(_multiplexer, _configuration, "productInRegion");
                    }
                    else if (result == -1)
                    {
                        responseData.Success = false;
                        responseData.Message = "Mã sản phẩm đã tồn tại";
                    }
                }
                else
                {
                    responseData.Message = respone.Value;
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }
        [HttpGet("GetPhienBan")]
        public List<PhienBans> GetPhienBansByParentId(int idProduct)
        {
            var response = new List<PhienBans>();
            if (idProduct > 0)
            {
                using (IDbContext context = new IDbContext())
                {
                    var res = context.ProductPriceInZoneList.Where(r => r.ProductId == idProduct).ToList();
                    if(res != null)
                    {
                        foreach(var item in res)
                        {
                            var _obj = new PhienBans();
                            _obj.priceEachNguoiLon = item.PriceEachNguoiLon;
                            _obj.priceEachTreEm = item.PriceEachTreEm;
                            _obj.netEachNguoiLon = item.NetEachNguoiLon;
                            _obj.netEachTreEm = item.NetEachTreEm;
                            _obj.priceEachNguoiGia = item.PriceEachNguoiGia;
                            _obj.netEachNguoiGia = item.NetEachNguoiGia;
                            _obj.minimumNguoiLon = item.MinimumNguoiLon;
                            _obj.minimumTreEm = item.MinimumTreEm;
                            _obj.minimumNguoiGia = item.MinimumNguoiGia;
                            _obj.emailSupplier = item.EmailSupplier;
                            var selectedOptionsStringArr = item.ZoneList.Split(",");
                            _obj.selectedOptions = new List<int>();
                            foreach(var option in selectedOptionsStringArr) {
                                _obj.selectedOptions.Add(int.Parse(option));
                            }
                            _obj.lastMinuteSetupDay = item.LastMinuteSetupDay;
                            _obj.lastMinuteSetupTime = item.LastMinuteSetupTime;
                            response.Add(_obj);
                        }
                    }
                }
                    
            }
            return response;
        }

        [HttpGet("GetCancelPolicies")]
        public async Task<IActionResult> GetCancelPolicies(int idProduct)
        {
            if(idProduct > 0)
            {
                using (IDbContext context = new IDbContext())
                {
                    var result = context.ProductCancelPolicy.Where(r => r.ProductId == idProduct).ToList();
                    return Ok(result);
                }
            }
            return BadRequest();
        }

        [HttpGet("GetGiaPhienBanTheoNgay/{zoneList}/{productId}")]
        public IActionResult GetGiaPhienBanTheoNgay(string zoneList, int productId)
        {
            if(!string.IsNullOrEmpty(zoneList) && productId > 0)
            {
                var result = productBCL.GetAllProductPriceInZoneListByDate(zoneList, productId).OrderBy(r => r.Date);
                return Ok(result);
            }
            return BadRequest("Co loi xay ra, vui long thu lai");
            
        }


        [HttpPost("UpdateGiaPhienBanTheoNgay")]
        public IActionResult UpdateGiaPhienBanTheoNgay([FromBody] List<ProductPriceInZoneListByDate> request)
        {
            var result = productBCL.UpdateGiaPhienBanTheoNgay(request);
            dynamic response = new ExpandoObject();
            response.status = result;
            return Ok(response);
        }

        [HttpGet("GetTopUps")]
        public IActionResult GetListTopUpByParentId(int idProduct)
        {
            var result = _productRepository.GetListTopUpByParentId(idProduct);
            return Ok(result);

        }
        [HttpGet("GetSerialNumbers")]
        public IActionResult GetSerialNumbersByParentId(int idProduct)
        {
            var result = _productRepository.GetListProductSerialNumbers(idProduct);
            return Ok(result);
        }
        [HttpPut("Delete")]
        public ResponseData Delete(int id, int status)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                bool ads = productBCL.UpdateTrangThai(new MI.Entity.Models.Product { Id = id, Status = status });
                if (ads)
                {
                    responseData.Success = true;
                    responseData.Message = "Thành công";
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = "Thất bại";
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }
        [HttpPut("UpdateSort")]
        public ResponseData UpdateSort(int id, int sortNew)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                bool ads = productBCL.UpdateSort(new MI.Entity.Models.Product { Id = id, SortOrder = sortNew });
                if (ads)
                {
                    responseData.Success = true;
                    responseData.Message = "Thành công";
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = "Thất bại";
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }

        [HttpPost("GetAllProductComponent")]
        public IActionResult GetAllProductComponent([FromBody] Utils.FilterQuery filter)
        {
            ResponsePaging responseData = new ResponsePaging();
            var total = 0;
            var data = productBCL.GetAllProductComponent(filter, out total);
            responseData.ListData = data.ToList<object>();
            responseData.Total = total;
            return Ok(responseData);
        }

        [HttpGet("GetProductComponentById")]
        public IActionResult GetProductComponentById(int id)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                var objPro = productBCL.GetProductComponentById(id);
                if (objPro != null)
                    responseData.Data = objPro;
            }
            catch (Exception ex)
            {
            }

            return Ok(responseData);
        }

        [HttpPut("UpdateProductComponent")]
        public ResponseData UpdateProductComponent([FromBody] MI.Entity.Models.ProductComponent request)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                int result = productBCL.UpdateProductComponent(request);
                if (result > 0)
                {
                    responseData.Success = true;
                    responseData.Message = "Thành công";
                    responseData.Id = result;
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = "Lưu không thành công";
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }

        [HttpPut("DeleteProductComponentById")]
        public ResponseData DeleteProductComponentById(int id)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                int result = productBCL.DeleteProductComponentById(id);
                if (result > 0)
                {
                    responseData.Success = true;
                    responseData.Message = "Thành công";
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = "Thất bại";
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }


        [HttpGet("GetAllCatProductComponent")]
        public ResponseData GetAllCatProductComponent()
        {
            ResponseData responseData = new ResponseData();
            try
            {
                var lstProductUnit = productBCL.GetAllProductComponent();
                responseData.ListData = lstProductUnit.Cast<object>().ToList();
            }
            catch (Exception ex)
            {

            }
            return responseData;
        }
    }
    //MI.Entity.Models.Product product, List<PhienBans> phienBans
    public class UpdateProductParameterWithPhienBan
    {
        public MI.Entity.Models.Product Product { get; set; }
        public List<PhienBans> PhienBans { get; set; }

        public List<ProductCancelPolicy> CancelPolicies { get; set; } = new List<ProductCancelPolicy>();
    }

}
