using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformCMS.ViewModel
{
    public class ProductInPromotions
    {
        public int Id { get; set; }
        public List<ProductInPromotion> Promotions { get; set; }
        public int LocationId { get; set; }
    }
}
