﻿using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using MI.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using PlatformWEBAPI.ExecuteCommand;
using PlatformWEBAPI.LanguageConfig;
using PlatformWEBAPI.Middleware;
using PlatformWEBAPI.Services.ApiJoyTel.Repository;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.BankInstallment.Repository;
using PlatformWEBAPI.Services.BannerAds.Repository;
using PlatformWEBAPI.Services.Config.Repository;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.FlashSale.Repository;
using PlatformWEBAPI.Services.Locations.Repository;
using PlatformWEBAPI.Services.LuckySpin.Repository;
using PlatformWEBAPI.Services.OpenAPI;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Product.Repository;
using PlatformWEBAPI.Services.Promotion.Repository;
using PlatformWEBAPI.Services.Store.Repository;
using PlatformWEBAPI.Services.Zone.Repository;
using PlatformWEBAPI.Utility;
using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Utils.Settings;

namespace PlatformWEBAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();
            //Configuration = builder.Build();
            Configuration = configuration;

            // ✅ Khởi tạo Firebase một lần duy nhất
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile("./serviceAccount.json") // đường dẫn tùy bạn
                });
            }



        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // config cache
            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingSection);

            //var emailSection = Configuration.GetSection("Email");
            //services.Configure<AppSettings>(emailSection);

            services.InitRedisCache(Configuration);

            MI.ES.ConfigES.Start();


            //services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>();
            // cache end
            services.AddSingleton<IConfiguration>(Configuration);
            #region HangfireJob
            services.AddHostedService<RecurringJobsService>();
            services.AddHangfire(config =>
                config.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));

            #endregion
            #region Register Connection Dappter
            services.AddTransient<IExecuters, Executers>();
            #endregion
            #region Register Transient
            services.AddTransient<IZoneRepository, ZoneRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ILocationsRepository, LocationsRepository>();
            services.AddTransient<IPromotionRepository, PromotionRepository>();
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IBannerAdsRepository, BannerAdsRepository>();
            services.AddTransient<IExtraRepository, ExtraRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IConfigSEOUtility, ConfigSEOUtility>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddTransient<IFlashSaleRepository, FlashSaleRepository>();
            services.AddTransient<IBankInstallmentRepository, BankInstallmentRepository>();
            services.AddTransient<ILuckySpinRepository, LuckySpinRepository>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            services.AddTransient<ISiteMapUtility, SiteMapUtility>();
            services.AddTransient<IApiJoyTelRepository, ApiJoyTelRepository>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IOpenAPIRepository, OpenAPIRepository>();

            #endregion

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });


            #region Register Localization
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("vi-VN"),
                    new CultureInfo("en-US"),
                    new CultureInfo("Fr"),
                    new CultureInfo("ko-KR"),
                    new CultureInfo("zh-CN"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                //options.RequestCultureProviders = new[]{ new RouteDataRequestCultureProvider{
                //    IndexOfCulture = 1,
                //    IndexofUICulture = 1
                //}};
                options.RequestCultureProviders = new[] { new CookieRequestCultureProvider() };
            });
            services.AddSingleton<HtmlEncoder>(
            HtmlEncoder.Create(allowedRanges: new[] {
                UnicodeRanges.All
            }));
            #endregion
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #region Register cookie
            services.AddTransient<ICookieUtility, CookieUtility>();
            #endregion
            services.AddResponseCaching();
            // Đăng ký Swagger generator, xác định một hoặc nhiều tài liệu Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My API", Version = "v1" });
            });
            //gzip
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                            {
                               // General
                                "text/plain",
                                // Static files
                                "text/css",
                                "text/javascript",
                                "application/javascript",
                                // MVC
                                "text/html",
                                "application/xml",
                                "text/xml",
                                "application/json",
                                "text/json",
                                // image
                                "image/svg+xml",
                                "image/png",
                                "image/jpeg"

                            }); ;
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(365);//You can set Time   
                //options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = ".MyApplication";
            });
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<SmtpClient>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClient()
                {
                    Host = config.GetValue<String>("Email:Smtp:Host"),
                    Port = config.GetValue<int>("Email:Smtp:Port"),
                    Credentials = new NetworkCredential(
                        config.GetValue<String>("Email:Smtp:Username"),
                        config.GetValue<String>("Email:Smtp:Password")
                    )
                };
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            //services.Configure<RouteOptions>(options =>
            //{
            //    options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            //});

            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 443;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseRewriter(new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 443));
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                if (!context.Request.Path.StartsWithSegments("/css") || !context.Request.Path.StartsWithSegments("/js") || !context.Request.Path.StartsWithSegments("/design") || !context.Request.Path.StartsWithSegments("/fonts")
                    || !context.Request.Path.StartsWithSegments("/hanhchinhvn-master") || !context.Request.Path.StartsWithSegments("/images") || !context.Request.Path.StartsWithSegments("/mail-templates") || !context.Request.Path.StartsWithSegments("/sound")
                    || !context.Request.Path.StartsWithSegments("/vendor") || !context.Request.Path.StartsWithSegments("/top-ten-travel")
                )
                {
                    await next();
                }
                //await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/p404.html";
                    await next();
                }
                if (context.Request.Path == "/term-and-condition")
                {
                    context.Response.Redirect("/terms-and-conditions", permanent: true);  // Sử dụng `permanent: true` để đánh dấu là một chuyển hướng vĩnh viễn (301)

                    await next();
                }

            });

            //app.UseHttpsRedirection();
            //app.UseStaticFiles(); 

            #region register localization
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            //app.UseRequestLocalization(options.Value);
            //app.UseRequestLocalization();
            #endregion
            app.UseCookiePolicy();
            //app.UseResponseCaching();
            app.UseResponseCompression();


            app.UseHangfireServer();
            app.UseSession();
            //app.ClearCacheMiddleware();
            // Áp dụng chính sách CORS
            app.UseCors("AllowAll");
            //app.ClearCacheMiddleware();
            //app.RedirectMiddleware();
            // Phục vụ middleware Swagger và thiết lập endpoint cho JSON và UI của Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;  // Đặt Swagger UI là trang chủ
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Extra}/{action=GetFirstLevelSiteMap}/{id?}");
                routes.MapRoute(
                    name: "SiteMap",
                    template: "sitemap.xml",
                    defaults: new { controller = "Extra", action = "GetFirstLevelSiteMap" }
                );
                routes.MapRoute(
                    name: "SiteMapSecondLevel",
                    template: "{culture_code}/sitemap.xml",
                    defaults: new { controller = "Extra", action = "GetSecondLevelSiteMap" }
                );
                routes.MapRoute(
                    name: "GetThirdLevelSiteMapStaticPage",
                    template: "{culture_code}/static_page.xml",
                    defaults: new { controller = "Extra", action = "GetThirdLevelSiteMapStaticPage" }
                );
                routes.MapRoute(
                    name: "GetThirdLevelSiteMapBlogCategory",
                    template: "{culture_code}/blog_category.xml",
                    defaults: new { controller = "Extra", action = "GetThirdLevelSiteMapBlogCategory" }
                );
                routes.MapRoute(
                    name: "GetThirdLevelSiteMapProductCategory",
                    template: "{culture_code}/product_category.xml",
                    defaults: new { controller = "Extra", action = "GetThirdLevelSiteMapProductCategory" }
                );
                routes.MapRoute(
                    name: "GetThirdLevelSiteMapBlogs",
                    template: "{culture_code}/blogs.xml",
                    defaults: new { controller = "Extra", action = "GetThirdLevelSiteMapBlogs" }
                );
                routes.MapRoute(
                    name: "GetThirdLevelSiteMapProducts",
                    template: "{culture_code}/products.xml",
                    defaults: new { controller = "Extra", action = "GetThirdLevelSiteMapProducts" }
                );
            });
            app.UseStaticFiles();

        }
    }
}
