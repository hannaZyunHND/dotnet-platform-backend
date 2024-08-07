using Dapper;
using MI.Entity.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PlatformWEBAPI.ExecuteCommand;
using PlatformWEBAPI.Services.Locations.ViewModal;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Services.Locations.Repository
{
    public interface ILocationsRepository
    {
        List<LocationViewModel> GetLocations(string lang_code);
        LocationViewModel GetLocationFirst(string lang_code);
    }
    public class LocationsRepository : ILocationsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecuters _executers;
        //cache 
        private readonly IDistributedCache _distributedCache;

        private readonly IConnectionMultiplexer _multiplexer;
        // cache end

        public LocationsRepository(IConfiguration configuration, IExecuters executers, IDistributedCache distributedCache, IConnectionMultiplexer multiplexer)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;
        }

        public LocationViewModel GetLocationFirst(string lang_code)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetLocations";
            p.Add("@lang_code", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<LocationViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }

        public List<LocationViewModel> GetLocations(string lang_code)
        {
            var keyCache = string.Format("{0}_{1}", "location", lang_code);
            var r = new List<LocationViewModel>();
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetLocations";
            p.Add("@lang_code", lang_code);
            r = _executers.ExecuteCommand(_connStr, conn => conn.Query<LocationViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            ////Add cache
            //var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(r);
            //result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
            //var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
            //_distributedCache.Set(keyCache, result_after_cache, cache_options);
            //get in cache
            //var result_after_cache = _distributedCache.Get(keyCache);
            //if (result_after_cache != null)
            //{
            //    r = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocationViewModel>>(Encoding.UTF8.GetString(result_after_cache));
            //}
            //if (result_after_cache == null)
            //{
            //    var p = new DynamicParameters();
            //    var commandText = "usp_Web_GetLocations";
            //    p.Add("@lang_code", lang_code);
            //    r = _executers.ExecuteCommand(_connStr, conn => conn.Query<LocationViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            //    //Add cache
            //    var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(r);
            //    result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
            //    var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
            //    _distributedCache.Set(keyCache, result_after_cache, cache_options);
            //}

            return r;

        }



    }
}
