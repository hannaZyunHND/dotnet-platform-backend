using System;
using System.Collections.Generic;

namespace MI.Entity.Models
{
    public partial class CustomerRequestProductOldRenewal
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string FullName { get; set; }
        public string PriceRefer { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? LocationId { get; set; }
        public int? Type { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? IsSupported { get; set; }
        public int? DepartmentId { get; set; }
        public string Image { get; set; }
        public Product NewProductExchange { get; set; } = new Product();
        public int? ProductIdToExchange { get; set; }
        public decimal? DealPrice { get; set; }
        public string DealPriceStr { get; set; }
        public string NewProductPriceSTR { get; set; }
        public string SupportPriceSTR { get; set; }

    }
}
