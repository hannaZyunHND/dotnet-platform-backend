using MI.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PlatformWEB.ExecuteCommand;
using PlatformWEB.Services.Config.Repository;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace PlatformWEB.Middleware
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {

        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

        }

        public static IApplicationBuilder ClearCacheMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomClearCacheMiddleware>();
        }
        public static IApplicationBuilder RedirectMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RedirectMiddleware>();
        }

    }


    public class CustomClearCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        //cache 
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _multiplexer;
        public CustomClearCacheMiddleware(RequestDelegate next, IConfiguration configuration, IDistributedCache distributedCache, IConnectionMultiplexer multiplexer)
        {
            _next = next;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                RedisUtils.DeleteAllCacheAsynForce(_multiplexer, _configuration);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            await _next(httpContext);
        }
    }

    public class RedirectMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        //cache 
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly IConfigRepository _configRepository;
        public RedirectMiddleware(RequestDelegate next, IConfiguration configuration, IDistributedCache distributedCache, IConnectionMultiplexer multiplexer, IConfigRepository configRepository)
        {
            _next = next;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;
            _configRepository = configRepository;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            bool redrect = false;
            var data = httpContext.Request.Path.ToString();
            if (!String.IsNullOrEmpty(data))
            {
                var obj = _configRepository.GetUrlRedrect(data);
                if (obj != null && !String.IsNullOrEmpty(obj.UrlOld))
                {
                    if (obj.UrlType == 301)
                    {
                        httpContext.Response.Redirect(obj.UrlNew, true);
                        await _next(httpContext);
                        redrect = true;
                    }
                    else
                    {
                        httpContext.Response.Redirect(obj.UrlNew, false);
                        await _next(httpContext);
                        redrect = true;
                    }
                }
            }
            await _next(httpContext);

        }
    }
}
