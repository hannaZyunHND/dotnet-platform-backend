using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Services.Promotion.ViewModel
{
    public class PromotionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descripton { get; set; }
        public int IsDiscountPrice { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
    }

    public class RequestCheckValuePromotionCodeByProductId {
        public int productId { get; set; }
        public List<string> discountCode { get; set; }
    }

    public class ResponseCheckValuePromotionCodeByProductId {
        public int productId { get; set; }
        public string discountCode { get; set; }
        public bool isCanUse { get; set; }
        public decimal discountValue { get; set; }
        public int discountType { get; set; }
    }

}
