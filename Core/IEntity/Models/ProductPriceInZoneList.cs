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
        public int ProductId { get; set; }
    }
}
