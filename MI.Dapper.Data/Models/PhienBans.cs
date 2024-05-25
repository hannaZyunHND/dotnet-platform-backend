using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Dapper.Data.Models
{
    public class PhienBans
    {
        public decimal priceEachNguoiLon { get; set; } = 0;
        public decimal priceEachTreEm { get; set; } = 0;
        public decimal netEachNguoiLon { get; set; } = 0;
        public decimal netEachTreEm { get; set; } = 0;
        public List<int> selectedOptions { get; set; }
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
