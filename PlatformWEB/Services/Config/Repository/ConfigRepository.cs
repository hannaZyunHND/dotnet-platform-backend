using Dapper;
using MI.Entity.Models;
using Microsoft.Extensions.Configuration;
using PlatformWEB.ExecuteCommand;
using PlatformWEB.Services.Config.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.Services.Config.Repository
{
    public interface IConfigRepository
    {
        ConfigViewModel GetArticlesInZoneId_Minify(string configName, string langCode);
        ConfigViewModel GetConfigByName(string configName, string langCode);
        Redirect GetUrlRedrect(string url);
    }
    public class ConfigRepository : IConfigRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecuters _executers;
        public ConfigRepository(IConfiguration configuration, IExecuters executers)
        {
            _configuration = configuration;
            _executers = executers;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
        }

        public ConfigViewModel GetArticlesInZoneId_Minify(string configName, string langCode)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetConfigByName";
            p.Add("@configName", configName);
            p.Add("@lang_code", langCode);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<ConfigViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        public ConfigViewModel GetConfigByName(string configName, string langCode)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetConfigByName";
            p.Add("@configName", configName);
            p.Add("@lang_code", langCode);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<ConfigViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            if (result != null)
            {
                return result;
            }

            return new ConfigViewModel();
        }
        public Redirect GetUrlRedrect(string url)
        {
            var p = new DynamicParameters();
            var commandText = "SP_Get_UrlRediect";
            p.Add("@Url", url);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<Redirect>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));

            if (result != null && result.Count() > 0)
                return result.FirstOrDefault();
            else return new Redirect();
        }
    }


}
