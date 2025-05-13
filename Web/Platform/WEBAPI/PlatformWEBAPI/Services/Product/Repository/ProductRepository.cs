using Dapper;
using MI.Bo.Bussiness;
using MI.Dapper.Data.Models;
using MI.Entity.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Nest;
using PlatformWEBAPI.ExecuteCommand;
using PlatformWEBAPI.Services.Product.ViewModel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Services.Product.Repository
{
    public interface IProductRepository
    {
        List<ProductMinify> GetProductByKeywords(List<string> keywords, List<string> zoneUrl, string lang_code, int pageNumber, int pageSize, out int total, string sortBy = "TOP_VIEW", decimal startPrice = 0, decimal endPrice = 10000000);
        List<ProductMapLocation> GetProductByKeywordsALLMAP(List<string> keywords, List<string> zoneUrl, string lang_code, int pageNumber, int pageSize, out int total, string sortBy = "TOP_VIEW", decimal startPrice = 0, decimal endPrice = 10000000);
        List<ProductMinify> GetProductBySearchdZone(List<int> zone_ids, string lang_code, int pageNumber, int pageSize, out int total);
        List<ProductMinify> GetProductMinifiesTreeViewByZoneParentId(int parentId, string lang_code, int locationId, int pageNumber, int pageSize, out int total);
        List<ProductMinify> GetProductMinifiesTreeViewByZoneParentId(int parentId, string lang_code, int locationId, int pageNumber, int pageSize, int? discountPercent, out int total);
        List<ProductMinify> GetAllProductSortBy(int sort_rate, int locationId, string lang_code, int page_index, int page_size, out int total_row);
        List<ProductMinify> GetProductByParentId(int idProduct, string lang_code);
        List<ProductMinify> GetProductSameZoneByProductId(int idProduct, string lang_code);
        List<ProductBookingNote> GetProductBookingNote(int productId, string lang_code);
        List<ProductMinify> GetProductMinifiesTreeViewByZoneParentIdSkipping(int parentId, string lang_code, int locationId, int skip, int size);
        List<ProductMinify> GetProductsInRegionByZoneParentIdSkipping(int parentId, string lang_code, int locationId, int skip, int size);
        List<ProductMinify> FilterProductBySpectifications(FilterProductBySpectification fp, out int total);
        List<ProductMinify> FilterProductBySpectificationsInZone(FilterProductBySpectification fp, out int total);
        ProductDetail GetProductInfomationDetail(string alias, string lang_code);
        ProductDetail GetProductInfomationDetail(int id, string lang_code);
        List<FilterAreaCooked> GetFilterProductByZoneId(int zone_id, string lang_code);
        List<SpectificationEstimatesCooked> GetSpectificationByMaterialType(int materialType, string lang_code);
        List<ProductSpectificationDetail> GetProductSpectificationDetail(int id, string lang_code);
        List<ProductMinify> GetProductInZoneByZoneIdMinify(int zone_id, int locationId, string lang_code, int pageNumber, int pageSize, out int total);
        List<ProductMinify> GetProductInZoneByZoneIdMinifyV2(int zone_id, int locationId, string lang_code, int pageNumber, int pageSize, out int total);
        List<ProductMinify> GetProductInZoneByZoneParentIdMinify(int zone_id, int locationId, string lang_code, int pageNumber, int pageSize, out int total);
        List<ProductMinify> GetProductInRegionByZoneIdMinify(int zone_id, int locationId, string lang_code, int pageNumber, int pageSize, out int total);
        List<ProductMinify> GetProductInDiemDenByZoneIdMinify(int zone_id, DateTime? tuNgay, DateTime? denNgay, int locationId, string lang_code, int pageNumber, int pageSize, out int total);
        List<ProductPriceInLocationDetail> GetProductPriceInLocationDetail(int product_id, string lang_code);
        List<ProductMinify> GetProductsInProductById(string productIds, string type, int locationId, string lang_code, int pageIndex, int pageSize, out int total_row);
        List<ProductMinify> GetProductsInProductById(int productId, string type, int locationId, string lang_code, int pageIndex, int pageSize, out int total_row);
        List<PromotionInProduct> GetPromotionInProduct(int productId, int locationId, string lang_code);
        List<ProductMinify> GetProductInListProductsMinify(string productIds, int location_id, string lang_code, int page_index, int page_size, out int total_row);
        List<ProductMinify> GetProductInListProductsMinify(string productIds, string keyword, int location_id, string lang_code, int page_index, int page_size, out int total_row);
        List<ProductMinify> GetProductInListProductsMinify_WithTotalPrice(string productIds, int location_id, string lang_code, int page_index, int page_size, out int total_row, out int total_price);
        int GetTotalPriceInListProductsMinify(string productIds, int location_id, string lang_code);
        List<TagViewModel> GetTagsInProduct(string lang_code);
        List<ProductMinify> GetProductInTagMinify(string tag, int locationId, string lang_code, int pageNumber, int pageSize, out int total);
        List<ProductMinify> GetProductChildInProductParent(int parentId, int locationId);
        List<ProductOldRenewalViewModel> GetProductOldRenewal(int pageIndex, int pageSize, string lang_code, int locationId, int ZoneId, out int total_row);
        List<ProductOldRenewalViewModel> GetProductOldRenewalV2(int pageIndex, int pageSize, string lang_code, int locationId, int ZoneId, out int total_row);
        List<ProductOldRenewalViewModel> GetProductOldRenewalDetail(int productId, string lang_code, int locationId);
        int InsertCustomerProductOldRenewal(CustomerProductOldRenewal product);
        List<MI.Entity.Models.ProductComponent> GetAllProductComponent(int zoneId);
        List<ProductMinify> GetProductComponent(int zone_id, int zone_childId, int productCpnId, int locationId, string lang_code, string filter, int pageNumber, int pageSize, out int total);
        List<ProductMinify> GetProductByCustomerId(int customerId, string lang_code, int locationId);
        List<ProductMinify> GetProductByListId(List<int> products, string lang_code, int locationId);
        List<ProductMinify> GetProductByListIdVersionSearch(List<int> products, string lang_code, int locationId, string sortBy = "TOP_VIEW", decimal startPrice = 0, decimal endPrice = 10000000);
        List<EsSearchItem> GetEsSearchResult();
        ProductMinify GetProductMinifyById(int productId, string cultureCode);
        List<ProductCommentFeedback> GetProductCommentFeedback(RequestGetProductCommentFeedback request, out int total);
        ResponsePromotionDetail GetPromotionDetail(RequestPromotionDetail request);

        List<ProductMinify> GetProductByKeywords_V2(List<string> keywords, List<int> selectedZoneDestinations, List<int> selectedZoneServices, List<int> selectedZoneRegions, string lang_code, int pageNumber, int pageSize, out int total, string sortBy = "TOP_VIEW", decimal startPrice = 0, decimal endPrice = 10000000);


        //List<ProductMinify> GetProductsByStringFilter(string filter, string lang_code, int location_id, out int total_row);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecuters _executers;
        private readonly IDistributedCache _distributedCache;

        private readonly IConnectionMultiplexer _multiplexer;
        public ProductRepository(IConfiguration configuration, IExecuters executers, IDistributedCache distributedCache, IConnectionMultiplexer multiplexer)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;
        }
        public List<ProductMinify> GetProductMinifiesTreeViewByZoneParentId(int parentId, string lang_code, int locationId, int pageNumber, int pageSize, out int total)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductTreeviewByZoneParentShowLayout";
            p.Add("@parentId", parentId);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageNumber);
            p.Add("@pageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total = p.Get<int>("@total");
            return result;
        }
        public List<ProductMinify> GetProductByCustomerId(int customerId, string lang_code, int locationId)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductByCustomerId";
            p.Add("@customerId", customerId);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();

            return result;
        }
        public List<ProductMinify> GetProductByParentId(int productId, string lang_code)
        {

            var keyCache = string.Format("JT_{0}_{1}_{2}", "GetProductByParentId", productId, lang_code);
            var result = new List<ProductMinify>();

            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductMinify>>(Encoding.UTF8.GetString(result_after_cache));
            }
            else
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetProductByParentId";
                p.Add("@productId", productId);
                p.Add("@lang_code", lang_code);

                result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }

            return result;

        }
        public List<ProductMinify> GetProductSameZoneByProductId(int productId, string lang_code)
        {

            var keyCache = string.Format("JT_{0}_{1}_{2}", "GetProductSameZoneByProductId", productId, lang_code);
            var result = new List<ProductMinify>();

            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductMinify>>(Encoding.UTF8.GetString(result_after_cache));
            }
            else
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetProductSameZoneByProductId";
                p.Add("@productId", productId);
                p.Add("@lang_code", lang_code);

                result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }

            return result;

        }
        public List<ProductMinify> GetProductChildInProductParent(int parentId, int locationId)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductChildInProductParent";
            p.Add("@parentId", parentId);
            p.Add("@locationId", locationId);

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }
        public List<ProductMinify> GetProductMinifiesTreeViewByZoneParentIdSkipping(int parentId, string lang_code, int locationId, int skip, int size)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductTreeviewByZoneParentShowLayout_Skipping";
            p.Add("@parentId", parentId);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@skip", skip);
            p.Add("@size", size);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public ProductDetail GetProductInfomationDetail(string alias, string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInfomationDetail";
            p.Add("@alias", alias);
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<ProductDetail>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }

        public ProductDetail GetProductInfomationDetail(int id, string lang_code)
        {

            var keyCache = string.Format("JT_{0}_{1}_{2}", "GetProductInRegionByZoneIdMinify", id, lang_code);
            var result = new ProductDetail();

            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductDetail>(Encoding.UTF8.GetString(result_after_cache));
            }
            else
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetProductInfomationDetailV2";
                p.Add("@id", id);
                p.Add("@lang_code", lang_code);
                result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<ProductDetail>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                
                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }

            return result;

        }

        public List<ProductSpectificationDetail> GetProductSpectificationDetail(int id, string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductSpecificationDetail";
            p.Add("@id", id);
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductSpectificationDetail>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public List<ProductMinify> GetProductInZoneByZoneIdMinify(int zone_id, int locationId, string lang_code, int pageNumber, int pageSize, out int total)
        {

            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInZoneByZoneId_Minify";
            p.Add("@zone_id", zone_id);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageNumber);
            p.Add("@pageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total = p.Get<int>("@total");
            return result;

        }

        public List<ProductMinify> GetProductInZoneByZoneIdMinifyV2(int zone_id, int locationId, string lang_code, int pageNumber, int pageSize, out int total)
        {

            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInZoneByZoneId_Minify_V2";
            p.Add("@zone_id", zone_id);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageNumber);
            p.Add("@pageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total = p.Get<int>("@total");
            return result;

        }

        public List<ProductMinify> GetProductComponent(int zone_id, int zone_childId, int productCpnId, int locationId, string lang_code, string filter, int pageNumber, int pageSize, out int total)
        {

            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductComponent";
            p.Add("@zone_id", zone_id);
            p.Add("@zone_id_child", zone_childId);
            p.Add("@productComponentId", productCpnId);
            p.Add("@lang_code", lang_code);
            p.Add("@filter", filter);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageNumber);
            p.Add("@pageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total = p.Get<int>("@total");
            return result;

        }
        public List<ProductMinify> GetProductInZoneByZoneParentIdMinify(int zone_parent_id, int locationId, string lang_code, int pageNumber, int pageSize, out int total)
        {

            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInZoneByZoneParentId_Minify";
            p.Add("@zone_parent_id", zone_parent_id);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageNumber);
            p.Add("@pageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total = p.Get<int>("@total");
            return result;

        }

        public List<ProductPriceInLocationDetail> GetProductPriceInLocationDetail(int product_id, string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductPriceInLocations";
            p.Add("@productId", product_id);
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductPriceInLocationDetail>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public List<ProductMinify> GetProductsInProductById(string productIds, string type, int locationId, string lang_code, int pageIndex, int pageSize, out int total_row)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInProductMinify";
            p.Add("@productIds", productIds);
            p.Add("@locationId", locationId);
            p.Add("@type", type);
            p.Add("@lang_code", lang_code);
            p.Add("@pageIndex", pageIndex);
            p.Add("@pageSize", pageSize);
            p.Add("@total_row", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total_row = p.Get<int>("@total_row");
            return result;
        }

        public List<ProductMinify> GetProductsInProductById(int productId, string type, int locationId, string lang_code, int pageIndex, int pageSize, out int total_row)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInProductMinify_ComboWithCofigMoney";
            p.Add("@productId", productId);
            p.Add("@locationId", locationId);
            p.Add("@type", type);
            p.Add("@lang_code", lang_code);
            p.Add("@pageIndex", pageIndex);
            p.Add("@pageSize", pageSize);
            p.Add("@total_row", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total_row = p.Get<int>("@total_row");
            return result;
        }

        public List<PromotionInProduct> GetPromotionInProduct(int productId, int locationId, string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetPromotionsInProduct";
            p.Add("@productId", productId);
            p.Add("@locationId", locationId);
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<PromotionInProduct>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public List<ProductMinify> GetProductInListProductsMinify(string productIds, int location_id, string lang_code, int page_index, int page_size, out int total_row)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInListProductsMinify";
            p.Add("@productIds", productIds);
            p.Add("@locationId", location_id);
            p.Add("@lang_code", lang_code);
            p.Add("@pageIndex", page_index);
            p.Add("@pageSize", page_size);
            p.Add("@total_row", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total_row = p.Get<int>("@total_row");
            return result;
        }

        public List<ProductMinify> GetProductInListProductsMinify(string productIds, string keyword, int location_id, string lang_code, int page_index, int page_size, out int total_row)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInListProductsMinifyV2";
            p.Add("@keyword", keyword);
            p.Add("@locationId", location_id);
            p.Add("@lang_code", lang_code);
            p.Add("@pageIndex", page_index);
            p.Add("@pageSize", page_size);
            p.Add("@total_row", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total_row = p.Get<int>("@total_row");
            return result;
        }

        public int GetTotalPriceInListProductsMinify(string productIds, int location_id, string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetTotalPriceProductInListProductsMinify";
            p.Add("@productIds", productIds);
            p.Add("@locationId", location_id);
            p.Add("@lang_code", lang_code);
            p.Add("@total_price", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            var total = p.Get<int>("@total_price");
            return total;
        }

        public List<ProductMinify> GetProductInRegionByZoneIdMinify(int zone_id, int locationId, string lang_code, int pageNumber, int pageSize, out int total)
        {
            var a = pageSize;
            var keyCache = string.Format("JT_{0}_{1}_{2}_{3}_{4}_{5}", "GetProductInRegionByZoneIdMinify", zone_id, locationId, lang_code, pageNumber, pageSize);
            var result = new List<ProductMinify>();

            var result_after_cache = _distributedCache.Get(keyCache);
            if(result_after_cache != null)
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductMinify>>(Encoding.UTF8.GetString(result_after_cache));
                total = 10;
            }
            else
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetProductInRegionByZoneId_Minify";
                p.Add("@zone_id", zone_id);
                p.Add("@lang_code", lang_code);
                p.Add("@locationId", locationId);
                p.Add("@pageNumber", pageNumber);
                p.Add("@pageSize", pageSize);
                p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                a = p.Get<int>("@total");

                total = a;
                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }

            return result;
        }

        public List<ProductMinify> GetProductBySearchdZone(List<int> zone_ids,  string lang_code, int pageNumber, int pageSize, out int total)
        {
            var a = pageSize;
            var r = new List<ProductMinify>();
            //get in cache
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductBySearchdZone_Minify";
            DataTable dt = new DataTable();
            dt.Columns.Add("zoneId", typeof(int));
            foreach(var item in zone_ids)
            {
                dt.Rows.Add(item);
            }
            p.Add("@zone_ids", dt.AsTableValuedParameter("type_selecedSearchZone"));
            p.Add("@lang_code", lang_code);
            p.Add("@PageNumber", pageNumber);
            p.Add("@PageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            r = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            a = p.Get<int>("@total");
            total = a;
            return r;
        }

        public List<ProductMinify> GetProductByKeywords(List<string> keywords, List<string> zoneUrl, string lang_code, int pageNumber, int pageSize, out int total, string sortBy = "TOP_VIEW", decimal startPrice = 0, decimal endPrice= 10000000)
        {
            var a = pageSize;
            var r = new List<ProductMinify>();
            //get in cache
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductByKeywords_Minify";
            DataTable dt = new DataTable();
            dt.Columns.Add("zoneUrl", typeof(string));
            foreach (var item in zoneUrl)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    dt.Rows.Add(item);
                }
                
            }
            p.Add("@zone_urls", dt.AsTableValuedParameter("type_selectedSearchZoneUrl"));
            p.Add("@lang_code", lang_code);
            p.Add("@PageNumber", pageNumber);
            p.Add("@SortBy", sortBy);
            p.Add("@startPrice", startPrice);
            p.Add("@endPrice", endPrice);
            p.Add("@PageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            r = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            a = p.Get<int>("@total");
            total = a;
            return r;
        }

        public List<ProductMinify> GetProductByKeywords_V2(List<string> keywords, List<int> selectedZoneDestinations, List<int> selectedZoneServices, List<int> selectedZoneRegions, string lang_code, int pageNumber, int pageSize, out int total, string sortBy = "TOP_VIEW", decimal startPrice = 0, decimal endPrice = 10000000)
        {
            var a = pageSize;
            var r = new List<ProductMinify>();
            //get in cache
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductByKeywords_Minify_V2";
            DataTable dtDestinations = new DataTable();
            dtDestinations.Columns.Add("id", typeof(int));
            foreach (var item in selectedZoneDestinations)
            {
                if (item > 0)
                {
                    dtDestinations.Rows.Add(item);
                }

            }
            DataTable dtServices = new DataTable();
            dtServices.Columns.Add("id", typeof(int));
            foreach (var item in selectedZoneServices)
            {
                if (item > 0)
                {
                    dtServices.Rows.Add(item);
                }

            }
            DataTable dtZoneRegion = new DataTable();
            dtZoneRegion.Columns.Add("id", typeof(int));
            foreach (var item in selectedZoneRegions)
            {
                if (item > 0)
                {
                    dtZoneRegion.Rows.Add(item);
                }

            }
            p.Add("@zone_destinations", dtDestinations.AsTableValuedParameter("type_selectedSearchZoneId"));
            p.Add("@zone_services", dtServices.AsTableValuedParameter("type_selectedSearchZoneId"));
            p.Add("@zone_regions", dtZoneRegion.AsTableValuedParameter("type_selectedSearchZoneId"));
            p.Add("@lang_code", lang_code);
            p.Add("@PageNumber", pageNumber);
            p.Add("@SortBy", sortBy);
            p.Add("@startPrice", startPrice);
            p.Add("@endPrice", endPrice);
            p.Add("@PageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            r = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            a = p.Get<int>("@total");
            total = a;
            return r;
        }

        public List<ProductMinify> GetProductInDiemDenByZoneIdMinify(int zone_id, DateTime? tuNgay, DateTime? denNgay, int locationId, string lang_code, int pageNumber, int pageSize, out int total)
        {
            var a = pageSize;
            var keyCache = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", "productInDiemDen", lang_code, zone_id, locationId, pageNumber, pageSize);
            var r = new List<ProductMinify>();
            //get in cache
            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                r = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductMinify>>(Encoding.UTF8.GetString(result_after_cache));
            }
            if (result_after_cache == null)
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetProductInDiemDenByZoneId_Minify";
                p.Add("@zone_id", zone_id);
                p.Add("@lang_code", lang_code);
                p.Add("@locationId", locationId);
                p.Add("@tuNgay", tuNgay);
                p.Add("@denNgay", denNgay);
                p.Add("@pageNumber", pageNumber);
                p.Add("@pageSize", pageSize);
                p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                r = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                a = p.Get<int>("@total");
                //Add cache
                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }
            total = a;
            return r;
        }

        public List<ProductMinify> GetProductsInRegionByZoneParentIdSkipping(int parentId, string lang_code, int locationId, int skip, int size)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductsInRegionByZoneParentMinify_Skipping";
            p.Add("@parentId", parentId);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@skip", skip);
            p.Add("@size", size);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public List<FilterAreaCooked> GetFilterProductByZoneId(int zone_id, string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetFilterAreaByZoneId";
            p.Add("@zoneId", zone_id);
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<FilterArea>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();

            //start cooking
            var q = from r in result
                    group r by new
                    {
                        r.SpectificationId,
                        r.Name
                    } into r_after
                    select new FilterAreaCooked()
                    {
                        SpectificationId = r_after.Key.SpectificationId,
                        Name = r_after.Key.Name,
                        Values = r_after.ToList()
                    };
            //var result_cooked = new List<FilterAreaCooked>();
            //foreach(var item in result)
            //{
            //    var r = new FilterAreaCooked();
            //    r.Name = item.Name;
            //    r.Values = new List<string>();
            //    if (result_cooked.Count() == 0 || result_cooked.Where(x => x.Name.Equals(item.Name)).Count()==0 )
            //        result_cooked.Add(r);
            //}
            //foreach(var item in result)
            //{
            //    var f = result_cooked.Where(r => r.Name.Equals(item.Name)).FirstOrDefault();
            //    if (f != null)
            //        f.Values.Add(item.Value);
            //}
            return q.ToList();

        }

        public List<ProductMinify> GetProductInListProductsMinify_WithTotalPrice(string productIds, int location_id, string lang_code, int page_index, int page_size, out int total_row, out int total_price)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInListProductsMinify_WithTotalPrice";
            p.Add("@productIds", productIds);
            p.Add("@locationId", location_id);
            p.Add("@lang_code", lang_code);
            p.Add("@pageIndex", page_index);
            p.Add("@pageSize", page_size);
            p.Add("@total_row", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            p.Add("@total_price", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total_row = p.Get<int>("@total_row");
            total_price = p.Get<int>("@total_price");
            return result;
        }

        public List<SpectificationEstimatesCooked> GetSpectificationByMaterialType(int materialType, string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetSpectificationByMaterialType";
            p.Add("@materialType", materialType);
            p.Add("@lang_code", lang_code);
            var result_query = _executers.ExecuteCommand(_connStr, conn => conn.Query<SpectificationEstimates>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            var q = from r in result_query
                    group r by new
                    {
                        r.SpectificationId,
                        r.Name
                    } into r_after
                    select new SpectificationEstimatesCooked()
                    {
                        SpectificationId = r_after.Key.SpectificationId,
                        Name = r_after.Key.Name,
                        Values = r_after.ToList()
                    };
            return q.ToList();
        }


        //public List<ProductMinify> FilterProductBySpectifications(int parentId, string lang_code, int locationId, int manufacture_id, int min_price, int max_price, int sort_price, int sort_rate, string color_code, List<FilterSpectification> filter,string filter_text, int material_type, int pageNumber, int pageSize, out int total)
        public List<ProductMinify> FilterProductBySpectifications(FilterProductBySpectification fp, out int total)
        {
            DataTable fillter_cooked = new DataTable();
            fillter_cooked.Columns.Add("SpectificationId", typeof(int));
            fillter_cooked.Columns.Add("Value", typeof(string));

            if (fp.filter != null)
            {
                foreach (var item in fp.filter)
                {
                    fillter_cooked.Rows.Add(item.SpectificationId, item.Value);
                }
            }
            var p = new DynamicParameters();
            var commandText = "usp_Web_FilterProductBySpectifications_NotZone";
            //p.Add("@parentId", fp.parentId);
            p.Add("@lang_code", fp.lang_code);
            p.Add("@locationId", fp.locationId);
            p.Add("@manufacture_id", fp.manufacture_id);
            p.Add("@min_price", fp.min_price);
            p.Add("@max_price", fp.max_price);
            p.Add("@sort_price", fp.sort_price);
            p.Add("@sort_rate", fp.sort_rate);
            p.Add("@color_code", fp.color_code == null ? "" : fp.color_code);
            p.Add("@filter_text", fp.filter_text == null ? "" : fp.filter_text);
            p.Add("@material_type", fp.material_type);
            p.Add("@filter", fillter_cooked.AsTableValuedParameter("type_filterProductBySpectification"));
            p.Add("@pageNumber", fp.pageNumber);
            p.Add("@pageSize", fp.pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total = p.Get<int>("@total");
            return result;
        }

        public List<ProductMinify> GetAllProductSortBy(int sort_rate, int locationId, string lang_code, int page_index, int page_size, out int total_row)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetAllProductSortBy";
            p.Add("@sort_rate", sort_rate);
            p.Add("@locationId", locationId);
            p.Add("@lang_code", lang_code);
            p.Add("@pageIndex", page_index);
            p.Add("@pageSize", page_size);
            p.Add("@total_row", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total_row = p.Get<int>("@total_row");
            return result;
        }

        public List<ProductMinify> FilterProductBySpectificationsInZone(FilterProductBySpectification fp, out int total)
        {
            DataTable fillter_cooked = new DataTable();
            fillter_cooked.Columns.Add("SpectificationId", typeof(int));
            fillter_cooked.Columns.Add("Value", typeof(string));




            if (fp.filter != null)
            {
                foreach (var item in fp.filter)
                {
                    if (item.Value != null)
                        fillter_cooked.Rows.Add(item.SpectificationId, item.Value);
                }
            }
            var p = new DynamicParameters();
            var commandText = "usp_Web_FilterProductBySpectifications_v3s";
            p.Add("@parentId", fp.parentId);
            p.Add("@lang_code", fp.lang_code);
            p.Add("@locationId", fp.locationId);
            p.Add("@manufacture_id", fp.manufacture_id == null ? "" : fp.manufacture_id);
            p.Add("@min_price", fp.min_price);
            p.Add("@max_price", fp.max_price);
            p.Add("@sort_price", fp.sort_price);
            p.Add("@sort_rate", fp.sort_rate);
            p.Add("@color_code", fp.color_code == null ? "" : fp.color_code);
            p.Add("@filter_text", fp.filter_text == null ? "" : fp.filter_text);
            p.Add("@material_type", fp.material_type);
            p.Add("@filter", fillter_cooked.AsTableValuedParameter("type_filterProductBySpectification"));
            p.Add("@pageNumber", fp.pageNumber);
            p.Add("@pageSize", fp.pageSize);
            p.Add("@fromPrice", fp.fromPrice);
            p.Add("@toPrice", fp.toPrice);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total = p.Get<int>("@total");
            return result;
        }

        public List<TagViewModel> GetTagsInProduct(string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetAllTag";
            p.Add("@lang_code", lang_code);
            try
            {
                var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<TagViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<TagViewModel>();
            }

        }

        public List<ProductMinify> GetProductInTagMinify(string tag, int locationId, string lang_code, int pageNumber, int pageSize, out int total)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductInTag_Minify";
            p.Add("@tag", tag);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageNumber);
            p.Add("@pageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total = p.Get<int>("@total");
            return result;
        }

        public List<ProductOldRenewalViewModel> GetProductOldRenewal(int pageIndex, int pageSize, string lang_code, int locationId, int ZoneId, out int total_row)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductOldRenewal_Minify";
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageIndex);
            p.Add("@pageSize", pageSize);
            p.Add("@ZoneId", ZoneId);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductOldRenewalViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total_row = p.Get<int>("@total");
            return result;
        }

        public List<ProductOldRenewalViewModel> GetProductOldRenewalV2(int pageIndex, int pageSize, string lang_code, int locationId, int ZoneId, out int total_row)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductOldRenewal_Minify_V2";
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageIndex);
            p.Add("@pageSize", pageSize);
            p.Add("@ZoneId", ZoneId);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductOldRenewalViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            total_row = p.Get<int>("@total");
            return result;
        }

        public List<ProductOldRenewalViewModel> GetProductOldRenewalDetail(int productId, string lang_code, int locationId)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductOldRenewal_Detail";
            p.Add("@productId", productId);
            p.Add("@langCode", lang_code);
            p.Add("@locationId", locationId);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductOldRenewalViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public int InsertCustomerProductOldRenewal(CustomerProductOldRenewal product)
        {
            var result = 0;
            try
            {
                var parameters = new DynamicParameters();
                var commandText = "usp_Web_InsertToCustomerProductOldRenewal";

                parameters.Add("@productId", product.productId);
                parameters.Add("@fullName", product.fullName);
                parameters.Add("@phoneNumber", product.phoneNumber);
                parameters.Add("@email", product.email);
                parameters.Add("@locationId", product.locationId);
                parameters.Add("@type", product.type);
                parameters.Add("@departmentId", product.departmentId);
                parameters.Add("@productIdToExchange", product.productIdToExchange);
                parameters.Add("@dealPrice", product.dealPrice);
                parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                _executers.ExecuteCommand(_connStr, conn => conn.Query<int>(commandText, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                result = parameters.Get<int>("@outId");
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public List<MI.Entity.Models.ProductComponent> GetAllProductComponent(int zoneId)
        {
            var p = new DynamicParameters();
            var parameters = new DynamicParameters();
            var commandText = "usp_Web_GetAllProductComponent";
            parameters.Add("@zoneId", zoneId);

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<MI.Entity.Models.ProductComponent>(commandText, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public List<ProductMinify> GetProductByListId(List<int> products, string lang_code, int locationId)
        {
            //usp_Web_GetProductByListProductId
            var parameters = new DynamicParameters();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId", typeof(int));
            foreach (var item in products)
            {
                dt.Rows.Add(item);
            }
            parameters.Add("@productIds", dt.AsTableValuedParameter("type_product_id_list"));
            parameters.Add("@lang_code", lang_code);
            parameters.Add("@locationId", locationId);

            var commandText = "usp_Web_GetProductByListProductId";
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, parameters, commandType: CommandType.StoredProcedure)).ToList();
            return result;



        }

        public List<ProductMinify> GetProductMinifiesTreeViewByZoneParentId(int parentId, string lang_code, int locationId, int pageNumber, int pageSize, int? discountPercent, out int total)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductTreeviewByZoneParentShowLayout";
            p.Add("@parentId", parentId);
            p.Add("@lang_code", lang_code);
            p.Add("@locationId", locationId);
            p.Add("@pageNumber", pageNumber);
            p.Add("@pageSize", pageSize);
            p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            if (discountPercent != null)
            {
                if (discountPercent.HasValue)
                {
                    foreach (var item in result)
                    {
                        item.Price = Math.Round(item.Price * (100 - discountPercent.Value) / 100);
                    }
                }
            }
            total = p.Get<int>("@total");
            return result;
        }

        public List<EsSearchItem> GetEsSearchResult()
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_ElasticAll";
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<EsSearchItem>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();

            return result;
        }

        public List<ProductMinify> GetProductByListIdVersionSearch(List<int> products, string lang_code, int locationId, string sortBy = "TOP_VIEW", decimal startPrice = 0, decimal endPrice = 10000000)
        {
            //usp_Web_GetProductByListProductId
            var parameters = new DynamicParameters();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId", typeof(int));
            foreach (var item in products)
            {
                dt.Rows.Add(item);
            }
            parameters.Add("@productIds", dt.AsTableValuedParameter("type_product_id_list"));
            parameters.Add("@lang_code", lang_code);
            parameters.Add("@locationId", locationId);
            parameters.Add("@SortBy", sortBy);
            parameters.Add("@startPrice", startPrice);
            parameters.Add("@endPrice", endPrice);

            var commandText = "usp_Web_GetProductByListProductIdVersionSearch";
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandText, parameters, commandType: CommandType.StoredProcedure)).ToList();
            return result;
        }

        public List<ProductMapLocation> GetProductByKeywordsALLMAP(List<string> keywords, List<string> zoneUrl, string lang_code, int pageNumber, int pageSize, out int total, string sortBy = "TOP_VIEW", decimal startPrice = 0, decimal endPrice = 10000000)
        {
            total = 0;
            var r = new List<ProductMapLocation>();
            //get in cache
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductByKeywords_Minify_ALLMAP";
            DataTable dt = new DataTable();
            dt.Columns.Add("zoneUrl", typeof(string));
            foreach (var item in zoneUrl)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    dt.Rows.Add(item);
                }

            }
            p.Add("@zone_urls", dt.AsTableValuedParameter("type_selectedSearchZoneUrl"));
            p.Add("@lang_code", lang_code);
            p.Add("@PageNumber", pageNumber);
            p.Add("@SortBy", sortBy);
            p.Add("@startPrice", startPrice);
            p.Add("@endPrice", endPrice);
            p.Add("@PageSize", pageSize);
            r = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMapLocation>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return r;
        }

        public ProductMinify GetProductMinifyById(int productId, string cultureCode)
        {
            var r = new ProductMinify();
            //get in cache
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetProductMinifyById";
            
            p.Add("@productId", productId);
            p.Add("@lang_code", cultureCode);
            r = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirst<ProductMinify>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return r;
        }

        public List<ProductBookingNote> GetProductBookingNote(int productId, string lang_code)
        {
            //get in cache


            var keyCache = string.Format("JT_{0}_{1}_{2}", "GetProductBookingNote", productId, lang_code);
            var result = new List<ProductBookingNote>();

            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductBookingNote>>(Encoding.UTF8.GetString(result_after_cache));
            }
            else
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetBookingNoteZoneByProductId";

                p.Add("@productId", productId);
                p.Add("@lang_code", lang_code);
                result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductBookingNote>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                
                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }

            return result;

        }

        public List<ProductCommentFeedback> GetProductCommentFeedback(RequestGetProductCommentFeedback request, out int total)
        {


            var keyCache = string.Format("JT_{0}_{1}_{2}_{3}", "GetProductCommentFeedback", request.productId, request.index, request.size);
            var result = new List<ProductCommentFeedback>();
            var _t = 0;
            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductCommentFeedback>>(Encoding.UTF8.GetString(result_after_cache));
            }
            else
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetProductFeedbackByProductId";
                p.Add("@productId", request.productId);
                p.Add("@index", request.index);
                p.Add("@size", request.size);
                p.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductCommentFeedback>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                _t = p.Get<int>("@total");

                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }

            foreach(var item in result)
            {
                if (!string.IsNullOrEmpty(item.commentImages))
                {
                    item.images = item.commentImages.Split(",").ToList();
                }
                item.ratingStars = new List<int>();
                for(int i = 0; i < item.rating; i++)
                {
                    item.ratingStars.Add(i);
                }
            }
            total = _t;
            
            return result;
        }

        public ResponsePromotionDetail GetPromotionDetail(RequestPromotionDetail request)
        {

            //Get promotionZone
            var p = new DynamicParameters();
            var commandTextFirstQuery = "usp_Web_GetMinifyZonePromotion";
            var commandTextSecondQuery = "usp_Web_GetProductMinifyByPromotionZoneId";
            p.Add("@promotionId", request.promotionId);
            p.Add("@cultureCode", request.cultureCode);
            
            var resultPromotion = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirst<ResponsePromotionDetail>(commandTextFirstQuery, p, commandType: System.Data.CommandType.StoredProcedure));
            if(resultPromotion != null)
            {
                var resultProductMinify = _executers.ExecuteCommand(_connStr, conn => conn.Query<ProductMinify>(commandTextSecondQuery, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                if(resultProductMinify != null)
                {
                    if(resultProductMinify.Count > 0)
                    {
                        resultPromotion.isHaveProduct = true;
                        //Bat dau phan tich tinh toan o day
                        //Group truoc cac danh sach services
                        var services = from r in resultProductMinify
                                       group r by new { r.zServiceId, r.zServiceName, r.zServiceAvatar, r.zServiceBanner, r.zServiceIcon } into groupted
                                       select new ResponseZoneInPromotion
                                       {
                                           zoneId = groupted.Key.zServiceId,
                                           zoneName = groupted.Key.zServiceName,
                                           zoneAvatar = groupted.Key.zServiceAvatar,
                                           zoneBanner = groupted.Key.zServiceBanner,
                                           zoneIcon = groupted.Key.zServiceIcon,
                                           products = groupted.ToList()
                                       };
                        var destinations = from r in resultProductMinify
                                           group r by new { r.zDestinationId, r.zDestinationName, r.zDestinationAvatar, r.zDestinationBanner, r.zDestinationIcon } into groupted
                                           select new ResponseZoneInPromotion
                                           {
                                               zoneId = groupted.Key.zDestinationId,
                                               zoneName = groupted.Key.zDestinationName,
                                               zoneAvatar = groupted.Key.zDestinationAvatar,
                                               zoneBanner = groupted.Key.zDestinationBanner,
                                               zoneIcon = groupted.Key.zDestinationIcon,
                                               products = groupted.ToList()
                                           };
                        resultPromotion.services = services.ToList();
                        resultPromotion.destinations = destinations.ToList();
                        resultPromotion.destinations = resultPromotion.destinations.Where(r => r.zoneId > 0).ToList();

                        //Xay dung tiep phan destination

                        var count = 0;

                        foreach (var item in resultPromotion.destinations)
                        {
                            if(item.zoneId > 0)
                            {
                                var servicesInDestinations = from r in item.products
                                                             group r by new { r.zServiceId, r.zServiceName, r.zServiceAvatar, r.zServiceBanner, r.zServiceIcon } into groupted
                                                             select new ResponseZoneInPromotion
                                                             {
                                                                 zoneId = groupted.Key.zServiceId,
                                                                 zoneName = groupted.Key.zServiceName,
                                                                 zoneAvatar = groupted.Key.zServiceAvatar,
                                                                 zoneBanner = groupted.Key.zServiceBanner,
                                                                 zoneIcon = groupted.Key.zServiceIcon,
                                                                 products = groupted.ToList(),
                                                                 filterdProducts = new List<ProductMinify>(),
                                                                 isActive = false

                                                             };
                                item.subZones = servicesInDestinations.ToList();
                                item.subZones = item.subZones.Where(r => r.zoneId > 0).ToList();
                                var _firstSubZone = item.subZones.FirstOrDefault();
                                if (_firstSubZone != null)
                                {
                                    _firstSubZone.isActive = true;
                                }
                                var filterdProductsDefault = item.subZones.Where(r => r.isActive == true).FirstOrDefault();
                                if (filterdProductsDefault != null)
                                {
                                    item.filterdProducts = filterdProductsDefault.products;
                                }
                            }
                            else
                            {
                                resultPromotion.destinations.Remove(item);
                            }
                            

                            count++;
                        }
                    }
                    else
                    {
                        resultPromotion.isHaveProduct = false;
                    }
                }
                return resultPromotion;
            }

            


            
            return null;
        }
    }
}
