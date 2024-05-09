using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Dapper.Data.Models
{
    public class PhienBans
    {
        public int id { get; set; } = 0;
        public int parentId { get; set; } = 0;
        public decimal giaGiam { get; set; } = 0;
        public int percentGiaGiam { get; set; } = 0;
        public decimal giaPhienBan { get; set; } = 0;
        public string tenPhienBan { get; set; } = "";
        public decimal giaNguoiLon { get; set; } = 0;
        public decimal giaTreEm { get; set; } = 0;
        public decimal giaEmBe { get; set; } = 0;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public List<PhienBans> mauSac { get; set; } = new List<PhienBans>();
    }

    public class SimTopUp
    {
        public int? id { get; set; } = 0;
        public int? parentId { get; set; } = 0;
        public string title { get; set; }
        public decimal? price { get; set; } = 0;
        // From Way2Go
        public string simType { get; set; } = "";
        public string gradientColor { get; set; } = "";
        public string coverage { get; set; } = "";
        public string dataLimit { get; set; } = "";
        public string validaty { get; set; } = "";
        public string smsNumber { get; set; } = "";
        public string phoneMinute { get; set; } = "";

    }
}
