using MI.Entity.Models;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System.Collections.Generic;

namespace PlatformWEBAPI.Services.PageSearch.ViewModel
{
    public class RequestGetSearchableZone
    {
        public string cultureCode { get; set; }
    }
    

    public class RequestGetProductBySearchdZone
    {
        public List<int> selectedZones { get; set; }
        public string cultureCode { get; set; }
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 12;
    }

    public class RequestGetProductByKeywords
    {
        public List<string> keywords { get; set; } = new List<string>();
        public List<string> selectedZones { get; set; } = new List<string>();
        public string cultureCode { get; set; } = "vi-VN";
        public string sortBy { get; set; } = "TOP_VIEW";
        public decimal startPrice { get; set; } = 0;
        public decimal endPrice { get; set; } = 10000000;
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 12;
    }
    public class RequestGetProductByKeywords_V2
    {
        public List<string> keywords { get; set; } = new List<string>();
        public List<int> selectedZoneDestinations { get; set; } = new List<int>();
        public List<int> selectedZoneServices { get; set; } = new List<int>();
        public List<int> selectedZoneRegions { get; set; } = new List<int>();
        public List<int> selectedIdProduct { get; set; } = new List<int>();
        public string cultureCode { get; set; } = "en-US";
        public string sortBy { get; set; } = "TOP_VIEW";
        public decimal startPrice { get; set; } = 0;
        public decimal endPrice { get; set; } = 10000000;
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 12;
    }

    public class RequestGetProductMinifyById
    {
        public int productId { get; set; }
        public string cultureCode { get; set; }
    }

    public class ResponseGetProductBySearchdZone
    {
        public List<ProductMinify> products { get; set; }
        public int total { get; set; }
    }
    public class ResponseGetProductByKeywords
    {
        public List<ProductMinify> products { get; set; }

        public List<ProductMapLocation> itemMaps { get; set; }
        public int total { get; set; }
    }

    public class ResponseGetSearchableZone
    {
        public List<ZoneByTreeViewMinify> zones { get; set; } = new List<ZoneByTreeViewMinify>();
    }

    public class RequestElasticFilter
    {
        /*string keyword, string culture_code, int index = 1, int size = 50*/
        public string keyword { get; set; }
        public string culture_code { get; set; }
        public int index { get; set; }
        public int size { get; set; }
    }
}
