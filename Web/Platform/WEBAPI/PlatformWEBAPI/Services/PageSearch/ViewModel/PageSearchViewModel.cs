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
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 12;
    }

    public class ResponseGetProductBySearchdZone
    {
        public List<ProductMinify> products { get; set; }
        public int total { get; set; }
    }
    public class ResponseGetProductByKeywords
    {
        public List<ProductMinify> products { get; set; }
        public int total { get; set; }
    }

    public class ResponseGetSearchableZone
    {
        public List<ZoneByTreeViewMinify> zones { get; set; } = new List<ZoneByTreeViewMinify>();
    }
}
