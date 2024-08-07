using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class ProductPriceInZoneList
    {
        [Key]
        public int Id { get; set; }
        public string ZoneList { get; set; }
        public decimal PriceEachNguoiLon { get; set; }
        public decimal PriceEachTreEm { get; set; }
        public decimal NetEachNguoiLon { get; set; }
        public decimal NetEachTreEm { get; set; }
        public decimal PriceEachNguoiGia { get; set; }
        public decimal NetEachNguoiGia { get; set; }
        public int ProductId { get; set; }
        public int MinimumNguoiLon { get; set; } = 1;
        public int MinimumTreEm { get; set; } = 1;
        public int MinimumNguoiGia { get; set; } = 1;
        public string EmailSupplier { get; set; } = "";
        public string ConfirmOption { get; set; } = "";
        public int? LastMinuteSetupDay { get; set; } = 0;
        public string LastMinuteSetupTime { get; set; } = "";

    }
}
