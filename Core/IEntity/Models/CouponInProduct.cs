using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MI.Entity.Models
{
    public partial class CouponInProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CouponId { get; set; }
        public int DiscountOption { get; set; }
        public decimal DiscountValue { get; set; }

    }
}
