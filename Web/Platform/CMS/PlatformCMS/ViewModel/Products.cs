
using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformCMS.ViewModel
{
    public class Products
    {
        public Product Data { get; set; }
        public List<int> ListCat { get; set; }
        public List<int> ListDiemDen { get; set; }
        public List<int> ListProductOption { get; set; }
        public List<int> ListTagOption { get; set; }
        public List<int> ListNoteOption { get; set; }

        public List<int> ListCouponZoneOptions { get; set; }

        public List<int> ListProp { get; set; }
        public List<string> ListColor { get; set; }
        public List<KeyValuePair<int, string>> ListArticle { get; set; }
    }
}
