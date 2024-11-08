using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using PlatformWEBAPI.Services.Article.ViewModel;
using PlatformWEBAPI.Services.BannerAds.Repository;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System;
using Utils;

namespace PlatformWEBAPI.Utility
{
    public interface IConfigSEOUtility
    {
        string ConfigWebsite(string domain, string page_name);
        string ConfigWebsite(string domain, string page_name, bool isMain);
        string ConfigMeta(string title, string description, string keyword);
        string ConfigMeta(string name, string value);
        string ConfigLink(string name, string value);
        string ConfigSocialMeta(string fbAppId = "default fbAddId", string url = "default url", string title = "default title", string description = "default description", string image = "default image");
        string ConfigShemaOrg(string domain_full, string title, string avatar, DateTime createdDate, DateTime modifiedDate, string metaTitle, string web_name, string logo, string description);
        string ConfigShemaOrgProduct(ProductDetail product);
        string ConfigShemaOrgArticle(ArticleDetail article, string domain_full, string domain);
        string ConfigOrganization();
        string ConfigLocalBusiness();
    }
    public class ConfigSEOUtility : IConfigSEOUtility
    {
        private readonly IBannerAdsRepository _bannerAds;
        private IHttpContextAccessor _httpContext;
        public ConfigSEOUtility(IBannerAdsRepository bannerAds, IHttpContextAccessor httpContext)
        {
            _bannerAds = bannerAds;
            _httpContext = httpContext;
        }
        private string _currentLanguage;
        private string _currentLanguageCode;

        //cache 
        //private readonly IDistributedCache _distributedCache;
        //private readonly IConfiguration _configuration;
        //private readonly IConnectionMultiplexer _multiplexer;
        // cache end
        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                    return _currentLanguage;

                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = _httpContext.HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                }

                return _currentLanguage;
            }
        }
        private string CurrentLanguageCode
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguageCode))
                    return _currentLanguageCode;

                if (string.IsNullOrEmpty(_currentLanguageCode))
                {
                    IRequestCultureFeature feature = _httpContext.HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguageCode = feature.RequestCulture.Culture.ToString();
                }

                return _currentLanguageCode;
            }


        }

        public string ConfigMeta(string title, string description, string keyword)
        {

            var r = "";
            r += "<title>" + title + "</title>";
            r += "<meta name=\"description\" content=\"" + description + "\">";
            r += "<meta name=\"keywords\" content=\"" + keyword + "\">";
            return r;
        }

        public string ConfigLink(string name, string value)
        {
            var r = string.Empty;
            r += "<link rel=\"" + name + "\" href=\"" + value + "\" />\r\n"; ;
            return r;
        }

        public string ConfigMeta(string name, string value)
        {
            var r = string.Empty;
            r += "<meta name=\"" + name + "\" content=\"" + value + "\" />\r\n";
            return r;
        }

        public string ConfigShemaOrg(string domain_full, string title, string avatar, DateTime createdDate, DateTime modifiedDate, string metaTitle, string web_name, string logo, string description)
        {
            var r = "";
            r += "<script type=\"application/ld+json\">";
            r += "{";
            r += "\"@context\":\"http://schema.org\",";
            r += "\"@type\":\"Corporation\",";
            r += "\"alternateName\": \"Di Động Xanh\",";
            r += "\"name\": \"Công ty TNHH Di Động Xanh\",";
            r += "\"url\": \"https://didongxanh.com.vn/\",";
            r += "\"logo\": \"https://cms.didongxanh.com.vn/uploads/thumb/2021/10/26/logoDDX11.png?v=1.0.0\",";
            r += "\"mainEntityOfPage\":{\"@type\":\"WebPage\",\"@id\":\"" + domain_full + "\"},";
            r += "\"headline\":\"" + title + "\",";
            r += "\"image\":{\"@type\":\"ImageObject\",\"url\":\"" + avatar + "\",\"height\":600,\"width\":800},";
            r += "\"datePublished\":\"" + createdDate + "\",";
            r += "\"dateModified\":\"" + modifiedDate + "\",";
            r += "\"author\":{  \"@type\":\"Person\",\"name\":\"" + web_name + "\"},";
            r += "\"publisher\":{\"@type\":\"Organization\",\"name\":\"" + web_name + "\",\"logo\":{\"@type\":\"ImageObject\",\"url\":\"" + logo + "\",\"width\":35,\"height\":34}},";
            r += "\"description\":\"" + description + "\"";
            r += "  \"contactPoint\": [{";
            r += "    \"@type\": \"ContactPoint\",";
            r += "    \"telephone\": \"0901176222\",";
            r += "    \"contactType\": \"customer service\",";
            r += "    \"contactOption\": \"TollFree\",";
            r += "    \"areaServed\": \"VN\",";
            r += "    \"availableLanguage\": \"Vietnamese\"";
            r += "  },{";
            r += "    \"@type\": \"ContactPoint\",";
            r += "    \"telephone\": \"0931139531\",";
            r += "    \"contactType\": \"customer service\",";
            r += "    \"contactOption\": \"TollFree\",";
            r += "    \"areaServed\": \"VN\",";
            r += "    \"availableLanguage\": \"Vietnamese\"";
            r += "  }],";
            r += "  \"sameAs\": [";
            r += "    \"https://www.facebook.com/Di-Động-Xanh-Store-107524988407772/\",";
            r += "    \"https://www.youtube.com/channel/UCIHgZUXjcnOubwrQFb3sfNQ\"";
            r += "  ]";
            r += "}";
            r += "</script>";
            return r;
        }
        public string ConfigShemaOrgProduct(ProductDetail product)
        {
            var r = "";
            r += "<script type=\"application/ld+json\">";
            r += "{";
            r += "\"@context\":\"http://schema.org\",";
            r += "\"@type\":\"Product\",";
            r += "\"name\":\"" + product.Title + "\",";
            r += "\"image\":\"" + UIHelper.StoreFilePath(product.Avatar) + "\",";
            r += "\"description\":\"" + product.Description + "\",";
            r += "\"mpn\":\"" + product.Id + "\",";
            r += "\"brand\":{\"@type\":\"Brand\",\"name\":\"" + product.Title + "\"},";
            r += "\"aggregateRating\":{\"@type\":\"AggregateRating\",\"ratingValue\":\"" + Math.Round((decimal)product.RateAVG, 1) + "\", \"reviewCount\":\"" + product.TotalRate + "\"},";
            r += "\"offers\":{\"@type\":\"Offer\",\"priceCurrency\":\"VND\",\"price\":\"" + product.Price + "\",\"priceValidUntil\":\"" + product.DiscountPrice + "\",\"itemCondition\":\"http://schema.org/UsedCondition \",\"availability\":\"http://schema.org/InStock \",\"logo\":{\"@type\":\"Organization\",\"name\":\"Executive Objects\"}},";
            r += "}";
            r += "</script>";
            return r;
        }
        public string ConfigShemaOrgArticle(ArticleDetail article, string domain_full, string domain)
        {
            var r = "";
            r += "<script type=\"application/ld+json\">";
            r += "{";
            r += "\"@context\":\"http://schema.org\",";
            r += "\"@type\":\"Article\",";
            r += "\"mainEntityOfPage\":{\"@type\":\"WebPage\",\"@id\":\"" + domain_full + "\"},";
            r += "\"headline\":\"" + article.Title + "\",";
            r += "\"image\":{\"@type\":\"ImageObject\",\"url\":\"" + UIHelper.StoreFilePath(article.Avatar) + "\",\"height\":600,\"width\":800},";
            r += "\"datePublished\":\"" + article.CreatedDate + "\",";
            r += "\"dateModified\":\"" + article.CreatedDate + "\",";
            r += "\"author\":{  \"@type\":\"Person\",\"name\":\"" + article.Author + "\"},";
            r += "\"publisher\":{\"@type\":\"Organization\",\"name\":\"" + domain + "\",\"logo\":{\"@type\":\"ImageObject\",\"url\":\"" + UIHelper.StoreFilePath(article.Avatar) + "\",\"width\":35,\"height\":34}},";
            r += "\"description\":\"" + article.Description + "\"";
            r += "}";
            r += "</script>";
            return r;
        }

        public string ConfigSocialMeta(string fbAppId = "default fbAddId", string url = "default url", string title = "default title", string description = "default description", string image = "default image")
        {
            var r = "";
            r += "<meta property=\"fb:app_id\" content=\"" + "125831801610404" + "\" />\r\n";
            r += "<meta property=\"og:url\" content=\"" + url + "\" />\r\n";
            r += "<meta property=\"og:type\" content=\"article\" />\r\n";
            r += "<meta property=\"og:title\" content=\"" + title + "\" />\r\n";
            r += "<meta property=\"og:description\" content=\"" + UIHelper.ClearHtmlTag(description) + "\" />\r\n";
            r += "<meta property=\"og:image\" content=\"" + UIHelper.StoreFilePath(image) + "\" />\r\n";
            return r;
        }

        public string ConfigWebsite(string domain, string page_name, bool isMain)
        {
            var favicon = _bannerAds.GetConfigByName(CurrentLanguageCode, "Favicon");
            var r = "";
            r += "<meta charset=\"utf-8\">\r\n";
            r += "<meta name=\"viewport\" content=\"width = device-width, initial-scale = 1\">\r\n";
            r += "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n";
            r += "<meta name=\"theme-color\" content=\"#081b62\" />\r\n";
            r += "<meta name=\"msapplication-navbutton-color\" content=\"#081b62\">\r\n";
            r += "<meta name=\"apple-mobile-web-app-capable\" content=\"yes\">\r\n";
            r += "<meta name=\"apple-mobile-web-app-status-bar-style\" content=\"black\">\r\n";
            r += "<meta http-equiv=\"Content-Language\" content=\"vi\" />\r\n";
            r += "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n";
            r += "<meta http-equiv=\"audience\" content=\"general\" />\r\n";
            r += "<meta name=\"resource-type\" content=\"document\" />\r\n";
            r += "<meta name=\"abstract\" content=\"" + page_name + "\" />\r\n";
            r += "<meta name=\"classification\" content=\"" + page_name + "\" />\r\n";
            r += "<meta name=\"area\" content=\"" + page_name + "\" />\r\n";
            r += "<meta name=\"placename\" content=\"Việt Nam\" />\r\n";
            r += "<meta name=\"author\" content=\"" + domain + "\" />\r\n";
            r += "<meta name=\"copyright\" content=\"©2020" + domain + "\" />\r\n";
            r += "<meta name=\"owner\" content=\"" + domain + "\" />\r\n";
            r += "<meta name=\"distribution\" content=\"Global\" />\r\n";
            r += "<link rel=\"alternate\" href=\"" + domain + "\" />\r\n";
            r += isMain == true ? "<link rel=\"canonical\" href=\"" + domain + "\" />\r\n" : "";
            r += "<link rel=\"prev\" href=\"" + domain + "\" />\r\n";
            r += "<link rel=\"shortcut icon\" type=\"image / png\" href=\"" + UIHelper.StoreFilePath(favicon, false) + "\"/>";
            return r;
        }


        public string ConfigSeoBreadcrumb(ZoneByTreeViewMinify bread, string page)
        {
            var r = "";

            return r;
        }

        public string ConfigWebsite(string domain, string page_name)
        {
            var favicon = _bannerAds.GetConfigByName(CurrentLanguageCode, "Favicon");
            var r = "";
            r += "<meta charset=\"utf-8\">\r\n";
            r += "<meta name=\"viewport\" content=\"width = device-width, initial-scale = 1\">\r\n";
            r += "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n";
            r += "<meta name=\"theme-color\" content=\"#081b62\" />\r\n";
            r += "<meta name=\"msapplication-navbutton-color\" content=\"#081b62\">\r\n";
            r += "<meta name=\"apple-mobile-web-app-capable\" content=\"yes\">\r\n";
            r += "<meta name=\"apple-mobile-web-app-status-bar-style\" content=\"black\">\r\n";
            r += "<meta http-equiv=\"Content-Language\" content=\"vi\" />\r\n";
            r += "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n";
            r += "<meta http-equiv=\"audience\" content=\"general\" />\r\n";
            r += "<meta name=\"resource-type\" content=\"document\" />\r\n";
            r += "<meta name=\"abstract\" content=\"" + page_name + "\" />\r\n";
            r += "<meta name=\"classification\" content=\"" + page_name + "\" />\r\n";
            r += "<meta name=\"area\" content=\"" + page_name + "\" />\r\n";
            r += "<meta name=\"placename\" content=\"Việt Nam\" />\r\n";
            r += "<meta name=\"author\" content=\"" + domain + "\" />\r\n";
            r += "<meta name=\"copyright\" content=\"©2020" + domain + "\" />\r\n";
            r += "<meta name=\"owner\" content=\"" + domain + "\" />\r\n";
            r += "<meta name=\"distribution\" content=\"Global\" />\r\n";
            r += "<link rel=\"alternate\" href=\"" + domain + "\" />\r\n";
            r += "<link rel=\"canonical\" href=\"" + domain + "\" />\r\n";
            r += "<link rel=\"prev\" href=\"" + domain + "\" />\r\n";
            r += "<link rel=\"shortcut icon\" type=\"image / png\" href=\"" + UIHelper.StoreFilePath(favicon, false) + "\"/>";
            return r;
        }

        public string ConfigOrganization()
        {
            var r = "";
            r += "<script type=\"application/ld+json\">";
            r += "{";
            r += "  \"@context\": \"https://schema.org\",";
            r += "  \"@type\": \"Corporation\",";
            r += "  \"name\": \"Công ty TNHH Di Động Xanh\",";
            r += "  \"alternateName\": \"Di Động Xanh\",";
            r += "  \"url\": \"https://didongxanh.com.vn/\",";
            r += "  \"logo\": \"https://cms.didongxanh.com.vn/uploads/thumb/2021/10/26/logoDDX11.png?v=1.0.0\",";
            r += "  \"contactPoint\": [{";
            r += "    \"@type\": \"ContactPoint\",";
            r += "    \"telephone\": \"0901176222\",";
            r += "    \"contactType\": \"customer service\",";
            r += "    \"contactOption\": \"TollFree\",";
            r += "    \"areaServed\": \"VN\",";
            r += "    \"availableLanguage\": \"Vietnamese\"";
            r += "  },{";
            r += "    \"@type\": \"ContactPoint\",";
            r += "    \"telephone\": \"0931139531\",";
            r += "    \"contactType\": \"customer service\",";
            r += "    \"contactOption\": \"TollFree\",";
            r += "    \"areaServed\": \"VN\",";
            r += "    \"availableLanguage\": \"Vietnamese\"";
            r += "  }],";
            r += "  \"sameAs\": [";
            r += "    \"https://www.facebook.com/Di-Động-Xanh-Store-107524988407772/\",";
            r += "    \"https://www.youtube.com/channel/UCIHgZUXjcnOubwrQFb3sfNQ\"";
            r += "  ]";
            r += "}";
            r += "</script>";
            return r;
        }

        public string ConfigLocalBusiness()
        {
            var r = "";
            r += "<script type=\"application/ld+json\">";
            r += "{";
            r += "  \"@context\": \"https://schema.org\",";
            r += "  \"@type\": \"LocalBusiness\",";
            r += "  \"name\": \"Di Động Xanh\",";
            r += "  \"image\": \"https://cms.didongxanh.com.vn/uploads/thumb/2022/04/12/he-thong-dien-thoai-di-dong-xanh-1.jpg\",";
            r += "  \"@id\": \"https://didongxanh.com.vn\",";
            r += "  \"url\": \"https://didongxanh.com.vn\",";
            r += "  \"telephone\": \"090117222\",";
            r += "  \"priceRange\": \"150000-50000000\",";
            r += "\"hasMap\":\"https://goo.gl/maps/1PcFkAwbUuXQ3C76A\",";
            r += "  \"address\": {";
            r += "    \"@type\": \"PostalAddress\",";
            r += "    \"streetAddress\": \"106 Hàm Nghi\",";
            r += "    \"addressLocality\": \"Đà Nẵng\",";
            r += "    \"postalCode\": \"50000\",";
            r += "    \"addressCountry\": \"VN\"";
            r += "  },";
            r += "  \"geo\": {";
            r += "    \"@type\": \"GeoCoordinates\",";
            r += "    \"latitude\": 16.0615833,";
            r += "    \"longitude\": 108.2085656";
            r += "  },";
            r += "  \"openingHoursSpecification\": {";
            r += "    \"@type\": \"OpeningHoursSpecification\",";
            r += "    \"dayOfWeek\": [";
            r += "      \"Monday\",";
            r += "      \"Tuesday\",";
            r += "      \"Wednesday\",";
            r += "      \"Thursday\",";
            r += "      \"Friday\",";
            r += "      \"Saturday\",";
            r += "      \"Sunday\"";
            r += "    ],";
            r += "    \"opens\": \"08:00\",";
            r += "    \"closes\": \"21:00\"";
            r += "  },";
            r += "  \"sameAs\": [";
            r += "    \"https://www.facebook.com/Di-%C4%90%E1%BB%99ng-Xanh-didongxanhcomvn-107930604352537\",";
            r += "    \"https://www.instagram.com/didongxanhcomvn/\",";
            r += "    \"https://www.youtube.com/channel/UC0ybSduEhIEhH_q0A90_4RA/about\",";
            r += "    \"https://www.linkedin.com/in/didongxanh/\",";
            r += "    \"https://www.pinterest.com/didongxanhcomvn/_saved/\",";
            r += "    \"https://didongxanh.tumblr.com/\",";
            r += "    \"https://github.com/didongxanh\",";
            r += "    \"https://www.bienphong.com.vn/di-dong-xanh-cua-hang-mua-ban-iphone-chinh-hang-gia-tot-post447273.html\",";
            r += "    \"https://giadinhvaphapluat.vn/di-dong-xanh-da-nang-dia-chi-tin-cay-cho-nhung-ifan-p96610.html\",";
            r += "    \"https://doanhnghiepvn.vn/thi-truong/luu-y-khi-mua-dien-thoai-iphone/20220105123316426\",";
            r += "    \"https://thuathienhue.org/giai-ma-suc-hut-iphone-chinh-hang-gia-tot-tai-cua-hang-di-dong-xanh-da-nang\",";
            r += "    \"https://baotuyenquang.com.vn/khoa-hoc/cong-nghe/mua-iphone-chinh-hang-gia-tot-tai-cua-hang-di-dong-xanh-da-nang-153510.html\",";
            r += "    \"https://baothainguyen.vn/tin-tuc/thong-tin-quang-cao/bat-mi-dia-chi-mua-iphone-chinh-hang-gia-tot-tai-di-dong-xanh-da-nang-296706-38.html\",";
            r += "    \"https://baoquangnam.vn/ban-can-biet/rinh-ngay-iphone-chinh-hang-gia-tot-ngay-tai-di-dong-xanh-da-nang-121994.html\",";
            r += "    \"https://baodongkhoi.vn/cua-hang-di-dong-xanh-da-n-ng-dia-chi-mua-ban-iphone-chinh-hang-gia-tot-04012022-a95345.html\"";
            r += "  ] ";
            r += "}";
            r += "</script>";
            return r;
        }
    }
}
