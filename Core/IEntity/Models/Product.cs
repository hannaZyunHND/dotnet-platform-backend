using System;
using System.Collections.Generic;

namespace MI.Entity.Models
{
    public partial class Product
    {
       

        public int Id { get; set; }
        public int? Status { get; set; }
        public string Url { get; set; }
        public string Avatar { get; set; }
        public string AvatarArray { get; set; }
        public double? Price { get; set; }
        public double? DiscountPrice { get; set; }
        public string Warranty { get; set; }
        public int? ManufacturerId { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public string Unit { get; set; }
        public int? Quantity { get; set; }
        public string PropertyId { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public string Guarantee { get; set; }
        public int? MaterialType { get; set; }
        public double DiscountPercent { get; set; }
        public string MetaFile { get; set; }
        public string Voucher { get; set; }
        public int? ViewCount { get; set; }
        public DateTime? ExprirePromotion { get; set; }
        public bool? IsInstallment { get; set; }
        public bool? Vat { get; set; }
        public int SortOrder { get; set; }
        public string ArticleId { get; set; }
        public int? ProductComboParentId { get; set; }
        public int? ParentId { get; set; } = 0;
        public int? ProductCpnId { get; set; } = 0;
        public decimal? GiaNguoiLon { get; set; } = 0;
        public decimal? GiaTreEm { get; set; } = 0;
        public decimal? GiaEmBe { get; set; } = 0;

        public string NgayDem { get; set; } = "";
        public DateTime? NgayBatDau { get; set; } = null;
        public DateTime? NgayKetThuc { get; set; } = null;
        public string PhuongTien { get; set; } = string.Empty;
        public string googleMapCrood { get; set; } = "";
        public string ConfirmOption { get; set; } = "";



        //// From Way2Go
        //public string SimType { get; set; } = "";
        //public string GradientColor { get; set; } = "";
        //public string Coverage { get; set; } = "";



        //public string DataLimit { get; set; } = "";
        //public string Validity { get; set; } = "";
        //public string SmsNumber { get; set; } = "";
        //public string PhoneMinute { get; set; } = "";
        //public string phoneMinuteInNetwork { get; set; } = "";
        //public string phoneMinuteOutNetwork { get; set; } = "";
        //public string phoneMinuteInRegion { get; set; } = "";
        //public string phoneMinuteOutRegion { get; set; } = "";
        //public string simPack { get; set; } = "";
        //public string TopUpsAvalible { get; set; } = string.Empty;
        //public string joytelProductCode { get; set; } = "";

    }
}
