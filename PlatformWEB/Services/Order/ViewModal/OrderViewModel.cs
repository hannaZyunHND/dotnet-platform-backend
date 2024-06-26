﻿using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PlatformWEB.Services.Order.ViewModal
{
    public class OrderViewModel
    {
        public string OrderCode { get; set; }
        public CustomerInOrderViewModel Customer { get; set; }
        public List<ProductInOrderViewModel> Products { get; set; }
        public List<string> Extras { get; set; }
        public string CodePromotion { get; set; }
        public string ValuePromotion { get; set; }
        public InstallmentDetail installmentDetail { get; set; } = null;
    }

    public class ExtraInOrderViewModel
    {
        public string Extra { get; set; }
    }
    public class CustomerInOrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string Gender { get; set; }
    }
    public class ProductInOrderViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal LogPrice { get; set; }
        public double Quantity { get; set; }
        public int OrderSourceType { get; set; }
        public int OrderSourceId { get; set; }
        public string Voucher { get; set; }
        public float VoucherPrice { get; set; }
        public int VoucherType { get; set; }

        public List<PromotionsInProductViewModel> Promotions { get; set; }

    }

    public class PromotionsInProductViewModel
    {
        public int PromotionId { get; set; }
        public string LogType { get; set; }
        public string LogName { get; set; }
        public decimal LogValue { get; set; }
    }
    public class OrderDetail
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float Quantity { get; set; }
        public decimal LogPrice { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ActiveStatus { get; set; }
        public string ICCID { get; set; }
        public int OrderId { get; set; }
        public string PickupPoint { get; set; }
        public string CustomerEmail { get; set; }
        //THEM
        public string Type { get; set; }
        public string JoyTelOrderCoupon { get; set; }
        public string JoyTelOrderTid { get; set; }
        public string JoyTelOrderCode { get; set; }


    }

    public class OrderGeneratedQRResponse
    {
        public string orderCode { get; set; }
        public int orderDetailId { get; set; }
    }


    public class InstallmentDetail
    {
        public string NganHang { get; set; } = "";
        public int SoThangTraGop { get; set; } = 0;
        public decimal TraTruoc { get; set; } = 0;
        public decimal TraGopMoiThang { get; set; } = 0;
        public decimal TongSoTien { get; set; } = 0;
        public decimal ChenhLech { get; set; } = 0;
    }

    public class OrderVersionMinifyRequest
    {
        public string customerRandomId { get; set; }
        public string orderCode { get; set; }
        public string email { get; set; }
        public double totalPrice { get; set; }
        public double productId { get; set; }
        public int orderId { get; set; } = 0;
        public string serialNumber { get; set; } = "";
        public List<OrderProductCartInfo> productsInfo { get; set; } = new List<OrderProductCartInfo>();
        public bool isCreatedAccount { get; set; } = false;
        public string orderNote { get; set; } = "";
        public string returnUrl { get; set; } = "";
        public int discountOption { get; set; } = -1;
        public int discountValue { get; set; } = -1;
        public string discountCode { get; set; } = "";



    }



    public class OrderPaypalRequest
    {
        public int productId { get; set; }
        public string amount { get; set; }
    }

    public class OrderVersionMinifyResponse
    {
        public string productName { get; set; }
        public int customerId { get; set; }
        public string iccid { get; set; }
        public string pickupPoint { get; set; }
        public string orderCode { get; set; }
        public string email { get; set; }
        public int isCreatedAccount { get; set; }
    }

    public class OrderProductCartInfo
    {
        //productId: element.id,
        //quantity: element.quantity,

        public int productId { get; set; }
        public int quantity { get; set; }
        public string serialNumber { get; set; }
        public decimal totalPrice { get; set; }
        public string type { get; set; }
    }


    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PCName { get; set; }
        public string Email { get; set; }
    }

    public class CustomerAuthViewModel
    {
        public string email { get; set; } = "";
        public string password { get; set; } = "";
        public string oldPassword { get; set; } = "";
    }

    public class SuccessEmailRequest
    {
        public string email { get; set; }
        public string productName { get; set; }
        public string orderCode { get; set; }
        public string iccid { get; set; }
        public string pickupPoint { get; set; }
        public List<OrderProductCartInfo> productsInfo { get; set; } = new List<OrderProductCartInfo>();

    }

    public class OrderDetailViewModel
    {
        public int Id { get; set; } = 0;
        public int OrderId { get; set; } = 0;
        public int ProductId { get; set; } = 0;
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public DateTime? CreatedDate { get; set; } = null;
    }
    public class BlankSerialNumberViewModel
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
    }
}
