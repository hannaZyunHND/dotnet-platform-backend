using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class ProductPriceInZoneListByDate
    {
        [Key]
        public int Id { get; set; }
        public string ZoneList { get; set; }
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public decimal PriceEachNguoiLon { get; set; }
        public decimal PriceEachTreEm { get; set; }
        public decimal PriceEachNguoiGia { get; set; }
        public decimal NetEachNguoiLon { get; set; }
        public decimal NetEachTreEm { get; set; }
        public decimal NetEachNguoiGia { get; set; }
    }
}
