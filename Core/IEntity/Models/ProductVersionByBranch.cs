using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Models
{
    public partial class ProductVersionByBranch
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ProductBranchID { get; set; }
        public string VersionName { get; set; }
        public string ColoName { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public int? PercentPromotion { get; set; }
    }
}
