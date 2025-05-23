﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MI.Entity.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            OrderPromotionDetail = new HashSet<OrderPromotionDetail>();
        }
        [Key]
        public int Id { get; set; }
        public int? OrderId { get; set; } = 0;
        public int? ProductId { get; set; } = 0;//
        public decimal? Quantity { get; set; } = 0;
        public decimal? LogPrice { get; set; } = decimal.Zero;
        public int? OrderSourceType { get; set; } = 0;
        public int? OrderSourceId { get; set; } = 0;
        public string Voucher { get; set; } = string.Empty;
        public byte? VoucherType { get; set; } = 0;
        public double? VoucherPrice { get; set; } = 0;
        public string VoucherMeta { get; set; } = string.Empty;
        public bool? Vat { get; set; } = false;
        public double? VatPrice { get; set; } = 0;
        public string ICCID { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string Type { get; set; } = string.Empty;
        public string JoyTelOrderCoupon { get; set; } = string.Empty;
        public string JoyTelOrderTid { get; set; } = string.Empty;
        public string JoyTelOrderCode { get; set; } = string.Empty;
        public string QRCode { get; set; } = string.Empty;
        public string ActiveStatus { get; set; } = string.Empty;

        public int ProductParentId { get; set; } = 0;
        public int QuantityChildren { get; set; } = 0;
        public string MetaData { get; set; } = "";
        public string CombinationZoneList { get; set; } = string.Empty;
        public decimal PriceEach { get; set; } = 0;
        public decimal PriceEachChildren { get; set; } = 0;
        public string SupplierStatus { get; set; } = "";
        public string FileETicket { get; set; } = "";
        public DateTime? PickingDate { get; set; } = null;
        public string CancelResponse { get; set; } = "";
        public string DefaultLanguage { get; set; } = "vi-VN";
        public string PaymentMethod { get; set; } = "";
        public string OnepayRef { get; set; } = "";
        public decimal? LogPriceGross { get; set; } = 0;
        public decimal? PriceGross { get; set; } = 0;

        public decimal? PriceGrossTreEm { get; set; } = 0;

        public Orders Order { get; set; }
        public ICollection<OrderPromotionDetail> OrderPromotionDetail { get; set; }
        public string noteSpecial { get; set; }
        public string useAppContact { get; set; }
        public string useAppContactValue { get; set; }
        public string useDiffrenceNumber { get; set; }
        public decimal? rollbackValue { get; set; }
        public int? rollbackOption { get; set; }
        public DateTime? RollbackRequestDate { get; set; }
        public bool? IsNotiFeedback { get; set; }
    }
}
