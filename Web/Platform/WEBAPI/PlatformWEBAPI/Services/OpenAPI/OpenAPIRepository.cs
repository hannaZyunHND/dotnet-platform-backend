using Dapper;
using Elasticsearch.Net;
using MI.Dal.IDbContext;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PlatformWEBAPI.ExecuteCommand;
using StackExchange.Redis;
using System.Text;
using System;
using System.Threading.Tasks;
using PlatformWEBAPI.Services.OpenAPI.Model;
using System.Collections.Generic;
using System.Linq;

namespace PlatformWEBAPI.Services.OpenAPI
{
    public interface IOpenAPIRepository
    {
        Task<List<ResponseProductsInServicesByCouponCode>> GetProductsInServicesByCouponCode(RequestGetProductsInServicesByCouponCode request);
        Task<List<ResponseGetServicesByCouponId>> GetServicesByCouponId(RequestGetServicesByCouponId request);

    }
    public class OpenAPIRepository : IOpenAPIRepository
    {
        //usp_Web_GetProductsInServicesByCouponCode
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecuters _executers;
        //cache 
        private readonly IDistributedCache _distributedCache;

        private readonly IConnectionMultiplexer _multiplexer;
        // cache end
        public OpenAPIRepository(IConfiguration configuration, IExecuters executers, IDistributedCache distributedCache, IConnectionMultiplexer multiplexer)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;

        }
        public async Task<List<ResponseProductsInServicesByCouponCode>> GetProductsInServicesByCouponCode(RequestGetProductsInServicesByCouponCode request)
        {
            var keyCache = string.Format("openAPI_GetProductsInServicesByCouponCode_{0}_{1}_{2}_{3}", request.couponId, request.serviceId, request.discountValue, request.cultureCode);
            var r = new List<ResponseProductsInServicesByCouponCode>();
            //Get in cache
            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                r = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseProductsInServicesByCouponCode>>(Encoding.UTF8.GetString(result_after_cache));
            }
            if (result_after_cache == null)
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetProductsInServicesByCouponCode";
                p.Add("@couponId", request.couponId);
                p.Add("@serviceId", request.serviceId);
                p.Add("@discountValue", request.discountValue);
                p.Add("@cultureCode", request.cultureCode);
                p.Add("@index", 1);
                p.Add("@size", 12);
                r = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseProductsInServicesByCouponCode>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                //Add cache
                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }
            return r;

        }

        public async Task<List<ResponseGetServicesByCouponId>> GetServicesByCouponId(RequestGetServicesByCouponId request)
        {
            var keyCache = string.Format("openAPI_GetServicesByCouponId_{0}_{1}_{2}", request.couponId, request.discountValue, request.cultureCode);
            var r = new List<ResponseGetServicesByCouponId>();
            //Get in cache
            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                r = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseGetServicesByCouponId>>(Encoding.UTF8.GetString(result_after_cache));
            }
            if (result_after_cache == null)
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetServicesByCouponId";
                p.Add("@couponId", request.couponId);
                p.Add("@discountValue", request.discountValue);
                p.Add("@cultureCode", request.cultureCode);
                r = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseGetServicesByCouponId>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                //Add cache
                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }
            return r;
        }
    }
}
