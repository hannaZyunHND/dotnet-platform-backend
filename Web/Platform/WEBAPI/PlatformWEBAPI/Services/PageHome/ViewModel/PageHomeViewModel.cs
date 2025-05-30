﻿using MI.Entity.Models;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System.Collections.Generic;

namespace PlatformWEBAPI.Services.PageHome.ViewModel
{
    public class ResponseHomeRegionViewModel
    {
        public ZoneByTreeViewMinify region { get; set; } = new ZoneByTreeViewMinify();
        public List<ProductMinify> products { get; set; } = new List<ProductMinify>();
    }

    public class RequestHomeRegionViewModel
    {
        public int parentId { get; set; } = 0;
        public string cultureCode { get; set; }
    }
    public class RequestGetListProductInRegionViewModel
    {
        public int regionId { get; set; }
        public string cultureCode { get; set; }
    }



}
