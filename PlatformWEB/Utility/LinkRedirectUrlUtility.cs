﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.Utility
{
    public static class LinkRedirectUrlUtility
    {
        public static string ProductInDiemDenUrl(string alias)
        {
            return string.Format("/diem-den/{0}", alias);
        }
        public static string ProductCategoryUrl(string alias)
        {
            return string.Format("/{0}", alias);
        }
        public static string BlogLinkUrl(string alias)
        {
            return string.Format("/{0}", alias);
        }
        public static string BlogDetailUrl(string alias)
        {
            return string.Format("/{0}.html", alias);
        }
        public static string FixinglUrl(string alias)
        {
            return string.Format("/sua-chua/{0}", alias);
        }
        public static string ProductDetailUrl(string alias)
        {
            return string.Format("/{0}.html", alias);
        }
        public static string RegionUrl(string alias)
        {
            return string.Format("/danh-muc/{0}", alias);
        }
    }
}
