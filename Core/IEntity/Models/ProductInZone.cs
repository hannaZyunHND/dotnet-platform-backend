using System;
using System.Collections.Generic;

namespace MI.Entity.Models
{
    public partial class ProductInZone
    {
        public int ZoneId { get; set; }
        public int ProductId { get; set; }
        public bool IsPrimary { get; set; }
        public int? IsHot { get; set; } = 999;
        public string BigThumb { get; set; }

        public Product Product { get; set; }
        public Zone Zone { get; set; }
    }
}
