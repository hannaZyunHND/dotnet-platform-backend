using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformCMS.ViewModel
{
    public class OrderVM
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public string Src { get; set; }
        public string CreateDate { get; set; }
        public string Price { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public MI.Entity.Models.Customer Customer { get; set; }
        public List<OrderDetailVM> OdderDetail { get; set; }

        //public InstallmentDetail InstallmentDetail { get; set; } = null;
    }


    public class OrderDetailVM
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Voucher { get; set; }
        public string VoucherPrice { get; set; }
        public string VoucherMeta { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string strPromotion { get; set; }
        public string strVoucher { get; set; }
        public decimal OderPrice { get; set; }
        public string Url { get; set; }
        public string ActiveStatus { get; set; }
    }
    public class InstallmentDetail
    {
        public string NganHang { get; set; } = "";
        public int SoThangTraGop { get; set; } = 0;
        public string TraTruoc { get; set; } = "";
        public string TraGopMoiThang { get; set; } = "";
        public string TongSoTien { get; set; } = "";
        public string ChenhLech { get; set; } = "";
    }
    public class InstallmentData
    {
        public string NganHang { get; set; } = "";
        public int SoThangTraGop { get; set; } = 0;
        public decimal TraTruoc { get; set; } = 0;
        public decimal TraGopMoiThang { get; set; } = 0;
        public decimal TongSoTien { get; set; } = 0;
        public decimal ChenhLech { get; set; } = 0;
    }
}
