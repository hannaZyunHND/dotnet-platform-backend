﻿using Dapper;
using MI.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PlatformWEB.ExecuteCommand;
using PlatformWEB.Services.BannerAds.ViewModel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformWEB.Services.BannerAds.Repository
{
    public interface IBannerAdsRepository
    {
        List<BannerAdsViewModel> GetBannerAds(string langCode);
        BannerAdsViewModel GetBannerAds_By_Code(string langCode, string code);
        string GetConfigByName(string lang_code, string name);
    }
    public class BannerAdsRepository : IBannerAdsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecuters _executers;
        //cache 
        private readonly IDistributedCache _distributedCache;

        private readonly IConnectionMultiplexer _multiplexer;
        // cache end
        public BannerAdsRepository(IConfiguration configuration, IExecuters executers, IDistributedCache distributedCache, IConnectionMultiplexer multiplexer)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;

        }

        //public BannerAdsRepository(IConfiguration configuration, IExecuters executers)
        //{
        //    _configuration = configuration;
        //    _connStr = _configuration.GetConnectionString("DefaultConnection");
        //    _executers = executers;
        //    //_distributedCache = distributedCache;
        //    //_multiplexer = multiplexer;

        //}

        public List<BannerAdsViewModel> GetBannerAds(string langCode)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_Get_BannerAds";
            p.Add("@langCode", langCode);

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<BannerAdsViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            //var keyCache = string.Format("", 1, 2, 2);
            //var result_after_cached = _distributedCache.GetOrSetCache(keyCache, () => result, _configuration);
            return result;
        }
        public BannerAdsViewModel GetBannerAds_By_Code(string langCode, string code)
        {
            var keyCache = string.Format("banner_{0}_{1}", code, langCode);
            var r = new BannerAdsViewModel();
            //Get in cache
            var result_after_cache = _distributedCache.Get(keyCache);
            if (result_after_cache != null)
            {
                r = Newtonsoft.Json.JsonConvert.DeserializeObject<BannerAdsViewModel>(Encoding.UTF8.GetString(result_after_cache));
            }
            if (result_after_cache == null)
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_Get_BannerAds_By_Code";
                p.Add("@langCode", langCode);
                p.Add("@code", code);
                r = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<BannerAdsViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                //Add cache
                var add_to_cache = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                result_after_cache = Encoding.UTF8.GetBytes(add_to_cache);
                var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                _distributedCache.Set(keyCache, result_after_cache, cache_options);
            }
            //var p = new DynamicParameters();
            //var commandText = "usp_Web_Get_BannerAds_By_Code";
            //p.Add("@langCode", langCode);
            //p.Add("@code", code);
            //var r = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<BannerAdsViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));

            return r;
        }

        public string GetConfigByName(string lang_code, string name)
        {
            try
            {
                var r = "";
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetConfigByName";
                p.Add("@lang_code", lang_code);
                p.Add("@configName", name);
                r = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<string>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                //var keyCache = string.Format("config_{0}_{1}", name, lang_code);
                //var r = ""; ;
                ////Get in cache
                //var result_after_cache = _distributedCache.Get(keyCache);
                //if (result_after_cache != null)
                //{
                //    r = Encoding.UTF8.GetString(result_after_cache);
                //}
                //if (result_after_cache == null)
                //{
                //    var p = new DynamicParameters();
                //    var commandText = "usp_Web_GetConfigByName";
                //    p.Add("@lang_code", lang_code);
                //    p.Add("@configName", name);
                //    r = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<string>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                //    //Add cache
                //    result_after_cache = Encoding.UTF8.GetBytes(r);
                //    var cache_options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(DateTime.Now.AddMinutes(int.Parse(_configuration["Redis:CachingExpireMinute"])));
                //    _distributedCache.Set(keyCache, result_after_cache, cache_options);
                //}
                return r;
            }

            //try
            //{
            //    var p = new DynamicParameters();
            //    var commandText = "usp_Web_GetConfigByName";
            //    p.Add("@lang_code", lang_code);
            //    p.Add("@configName", name);
            //    var r = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<string>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            //    if(r == null)
            //    {
            //        r = "";
            //    }
            //    return r;
            //}
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}
