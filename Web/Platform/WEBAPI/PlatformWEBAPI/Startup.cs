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
                .AddSessionStateTempDataProvider()
                .AddViewLocalization(
                    options => { options.ResourcesPath = "Resources"; })
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
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
            app.UseStaticFiles();

            #region register localization
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            //app.UseRequestLocalization();
            #endregion
            app.UseCookiePolicy();
            //app.UseResponseCaching();
            app.UseResponseCompression();
            app.UseHangfireServer();
            app.UseSession();
            app.ClearCacheMiddleware();
            // Áp dụng chính sách CORS
            app.UseCors("AllowAll");
            //app.ClearCacheMiddleware();
            //app.RedirectMiddleware();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "LocalizedDefault",
                    template: "{controller=Home}/{action=IndexPublic}/{id?}"
                );
                //routes.MapRoute(
                //    name: "LocalizedDefault",
                //    template: "{lang:lang}/{controller=Home}/{action=IndexPublic}/{id?}"
                //);
                routes.MapRoute(
                    name: "SiteMap",
                    template: "sitemap.xml",
                    defaults: new { controller = "Extra", action = "SiteMapGenerate" }
                );
                routes.MapRoute(
                    name: "PickupPoint",
                    template: "pickup-points",
                    defaults: new { controller = "Blog", action = "PickupPoint" }
                );
                routes.MapRoute(
                    name: "QuestAndAnswer",
                    template: "frequently-asked-questions",
                    defaults: new { controller = "Blog", action = "QandA" }
                );
                routes.MapRoute(
                    name: "TermAndCondition",
                    template: "terms-and-conditions",
                    defaults: new { controller = "Blog", action = "TermAndCondition" }
                );
                routes.MapRoute(
                    name: "TermAndConditions",
                    template: "terms-and-conditions",
                    defaults: new { controller = "Blog", action = "TermAndCondition" }
                );
                routes.MapRoute(
                    name: "BecomeAPartner",
                    template: "become-a-partner",
                    defaults: new { controller = "Blog", action = "BecomeAPartner" }
                );
                routes.MapRoute(
                    name: "RobotsTxt",
                    template: "robots.xml",
                    defaults: new { controller = "Extra", action = "RobotsTxt" }
                );
                routes.MapRoute(
                    name: "OrdersRemaster",
                    template: "gio-hang",
                    defaults: new { controller = "Order", action = "Orders" }
                );
                routes.MapRoute(
                    name: "HomeSearchRemaster",
                    template: "tim-kiem-nhanh",
                    defaults: new { controller = "Product", action = "ProductFilterInHome" }
                );
                routes.MapRoute(
                    name: "MySim",
                    template: "my-sim",
                    defaults: new { controller = "Customer", action = "Index" }
                );
                routes.MapRoute(
                    name: "BuyAllCart",
                    template: "buy-all-cart",
                    defaults: new { controller = "Customer", action = "BuyAllCart" }
                );
                routes.MapRoute(
                    name: "cart",
                    template: "my-cart",
                    defaults: new { controller = "Customer", action = "Cart" }
                );
                routes.MapRoute(
                    name: "EsSearchRemaster",
                    template: "tim-kiem",
                    defaults: new { controller = "Product", action = "ESSearchResult" }
                );
                //routes.MapRoute(
                //    name: "HomeSearchRemaster",
                //    template: "tim-kiem-nhanh",
                //    defaults: new { controller = "Product", action = "ProductFilterInHome" }
                //);
                routes.MapRoute(
                    name: "EsimatesRemaster",
                    template: "du-toan-cong-trinh",
                    defaults: new { controller = "Estimates", action = "ContructionEstimates" }
                );
                routes.MapRoute(
                    name: "StoreRemaster",
                    template: "chi-nhanh",
                    defaults: new { controller = "Store", action = "StoreList" }
                );
                routes.MapRoute(
                    name: "GioiThieu",
                    template: "gioi-thieu",
                    defaults: new { controller = "Blog", action = "GioiThieu" }
                );
                routes.MapRoute(
                    name: "About-Us",
                    template: "about-us",
                    defaults: new { controller = "Blog", action = "GioiThieu" }
                );

                //routes.MapRoute(
                //    name: "ProductComponentList",
                //    template: "sua-chua",
                //    defaults: new { controller = "ProductComponent", action = "ProductComponentList" }
                //    );
                routes.MapRoute(
                    name: "ProductComponentList_Zone",
                    template: "sua-chua/{alias}",
                    defaults: new { controller = "ProductComponent", action = "ProductComponentList_Zone" }
                    );

                routes.MapRoute(
                    name: "KiemTraDonHang",
                    template: "kiem-tra-don-hang",
                    defaults: new { controller = "Order", action = "KiemTraDonHang" }
                );
                routes.MapRoute(
                    name: "FlashSaleRemaster",
                    template: "flash-sale",
                    defaults: new { controller = "FlashSale", action = "FlashSaleList" }
                    );
                routes.MapRoute(
                    name: "ContactRemaster",
                    template: "lien-he",
                    defaults: new { controller = "Contact", action = "Index" }
                    );
                routes.MapRoute(
                    name: "ContactRemasterEn",
                    template: "contact-us",
                    defaults: new { controller = "Contact", action = "Index" }
                    );
                routes.MapRoute(
                    name: "LuckySpin",
                    template: "{alias}",
                    defaults: new { controller = "LuckySpin", action = "LuckySpin" },
                    constraints: new
                    {
                        alias = "vong-quay-may-man.html$"
                    });
                routes.MapRoute(
                    name: "P404",
                    template: "{alias}",
                    defaults: new { controller = "P404", action = "P404" },
                    constraints: new
                    {
                        alias = "p404.html"
                    });
                routes.MapRoute(
                    name: "ProductChangeNew",
                    template: "{alias}.pn{id}.html",
                    defaults: new { controller = "Product", action = "ProductOldRenewal" }
                    );
                routes.MapRoute(
                    name: "ArticleRemaster",
                    template: "{alias}",
                    defaults: new { controller = "Blog", action = "RedirectAction" },
                    constraints: new { alias = ".*\\.html$" }
                );
                routes.MapRoute(
                    name: "ArticleAMPRemaster",
                    template: "{alias}",
                    defaults: new { controller = "Blog", action = "AMPRedirectAction" },
                    constraints: new { alias = ".*\\.amp.html$" }
                );
                routes.MapRoute(
                    name: "ArticleUrlRemaster",
                    template: "tin-tong-hop/{alias}",
                    defaults: new { controller = "Zone", action = "RedirectAction" }
                    //constraints: new { alias = "^((?!.html).)*$" }
                );
                routes.MapRoute(
                    name: "ZoneUrlRemaster",
                    template: "{alias}",
                    defaults: new { controller = "Zone", action = "RedirectAction" }
                    //constraints: new { alias = "^((?!.html).)*$" }
                );
                routes.MapRoute(
                   name: "ZoneUrlRemasterWithPaging",
                   template: "{alias}/page/{pageIndex}",
                   defaults: new { controller = "Zone", action = "RedirectAction" }
               //constraints: new { alias = "^((?!.html).)*$" }
               );
                routes.MapRoute(
                    name: "ProductInRegion",
                    template: "danh-muc/{region}",
                    defaults: new { controller = "FilterProduct", action = "FilterProductByRegion" }
                    //constraints: new { alias = "^((?!.html).)*$" }
                );
                routes.MapRoute(
                    name: "ProductInDiemDen",
                    template: "diem-den/{region}",
                    defaults: new { controller = "FilterProduct", action = "FilterProductByDiemDen" }
                    //constraints: new { alias = "^((?!.html).)*$" }
                );
                routes.MapRoute(
                    name: "ProductInTag",
                    template: "the/{tag}",
                    defaults: new { controller = "FilterProduct", action = "FilterProductByTag" }
                    //constraints: new { alias = "^((?!.html).)*$" }
                );
                routes.MapRoute(
                    name: "ProductCategory",
                    template: "{lang}/{alias}.c{zoneId}.html",
                    defaults: new { controller = "Product", action = "ProductList" }
                    //constraints : new { alias = "[^\\.]",id="1" , lang = "vi" }
                    );
                routes.MapRoute(
                    name: "ProductDetail",
                    template: "{lang}/{alias}.p{product_id}.html",
                    defaults: new { controller = "Product", action = "ProductDetail" }
                    );

                routes.MapRoute(
                    name: "Orders",
                    template: "{lang}/cart.html",
                    defaults: new { controller = "Order", action = "Orders" }
                    );
                //routes.MapRoute(
                //    name: "Blogs",
                //    template: "{lang}/blog.html",
                //    defaults: new { controller = "Blog", action = "BlogList1" }
                //    );
                routes.MapRoute(
                    name: "Blogs",
                    template: "{lang}/{alias}.bp{zone_id}.html",
                    defaults: new { controller = "Blog", action = "BlogList1" }
                    );
                //routes.MapRoute(
                //    name: "Promotions",
                //    template: "{lang:lang}/{controller=Promotion}/{action=PromotionList}/{pageIndex?}",
                //    defaults: new { controller = "Promotion", action = "PromotionList" }
                //    );
                routes.MapRoute(
                    name: "Promotions",
                    template: "{lang}/promotion.html",
                    defaults: new { controller = "Promotion", action = "PromotionList" }
                    );
                routes.MapRoute(
                    name: "Recruitment",
                    template: "{lang}/recruitment.html",
                    defaults: new { controller = "Recruitment", action = "RecruitmentList" }
                    );
                routes.MapRoute(
                    name: "Quotation",
                    template: "{lang}/quotation.html",
                    defaults: new { controller = "Quotation", action = "QuotationList" }
                    );
                routes.MapRoute(
                    name: "DownloadCategory",
                    template: "{lang}/download-category.html",
                    defaults: new { controller = "Categories", action = "CategoriesList" }
                    );
                routes.MapRoute(
                    name: "Store",
                    template: "{lang}/store.html",
                    defaults: new { controller = "Store", action = "StoreList" }
                    );
                routes.MapRoute(
                    name: "FlashSale",
                    template: "{lang}/flash-sale.html",
                    defaults: new { controller = "FlashSale", action = "FlashSaleList" }
                    );
                routes.MapRoute(
                    name: "ArticleByTag",
                    template: "{lang}/filter-by-{tag}.html",
                    defaults: new { controller = "Blog", action = "FilterArticleByTag" }
                    );
                routes.MapRoute(
                    name: "BlogCategory",
                    template: "{lang}/{alias}.b{zoneId}.html",
                    defaults: new { controller = "Blog", action = "BlogList2" }
                    );
                routes.MapRoute(
                    name: "DownloadCategoryId",
                    template: "{lang}/{alias}.dc{zoneId}.html",
                    defaults: new { controller = "Categories", action = "CategoriesList1" }
                    );
                routes.MapRoute(
                    name: "ConstructionEstimates",
                    template: "{lang}/estimates.html",
                    defaults: new { controller = "Estimates", action = "ContructionEstimates" }
                    );
                routes.MapRoute(
                    name: "ArticleDetail",
                    template: "{lang}/{alias}.a{articleId}.html",
                    defaults: new { controller = "Blog", action = "BlogDetail" }
                    );

                routes.MapRoute(
                    name: "RedirectToParentZone",
                    template: "{lang}/{alias}.redirect{parent_id}.html",
                    defaults: new { controller = "Extra", action = "RedirectToParentZone" }
                    );
                // Trả góp
                routes.MapRoute(
                  name: "InstallmentIndex",
                  template: "/tra-gop/{alias}",
                  defaults: new { controller = "Installment", action = "Index" }
                );

                routes.MapRoute(
                name: "InstallmentIndex1",
                template: "/cong-ty-tai-chinh/{alias}",
                defaults: new { controller = "Installment", action = "Index" }
            );

                //routes.MapRoute(
                //    name: "default",
                //    template: "{*catchall}",
                //    defaults: new { controller = "Home", action = "RedirectToDefaultCulture", lang = "vi" });
            });

        }
    }
}
