using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Services.BannerAds.ViewModel
{
    public class BannerAdsViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string MetaData { get; set; }
        public int Type { get; set; }
        public string LanguageCode { get; set; }
    }

    public class MultipleBannerAdsViewModal
    {
        public string bannerAdsCode { get; set; }
        public List<DetailBanerAds> details { get; set; }
    }

    public class RequestBannerAdsByCodes
    {
        public List<string> codes { get; set; }
        public string cultureCode { get; set; }
    }
}
