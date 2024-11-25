using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using PlatformWEBAPI.Services.Product.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Services.Order.ViewModal
{


    public class RequestCreateMultipleItemOrder
    {
        public List<Pay> pays { get; set; } = new List<Pay>();
        public CustomerAuth auth { get; set; } = new CustomerAuth();
        public string orderCode { get; set; } = "";
        public string i18Code { get; set; } = "en";
        public string paymentMethod { get; set; } = "TEST";
        public OrderNote orderNotes { get; set; } = new OrderNote();
    }

    public class ResponseCreateMultipleItemOrder
    {
        public CustomerAuth auth { get; set; } = new CustomerAuth();
        public string orderCode { get; set; } = string.Empty;

    }
    public class CustomerAuth
    {
        public int id { get; set; } = 0;
        public string email { get; set; } = "";
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string country { get; set; } = "";
        public string phonePrefix { get; set; } = "";
        public string phoneNumber { get; set; } = "";
        public bool isNewUser { get; set; } = true;
        public string pcname { get; set; }
    }
    public class Pay
    {
        public string choosenDate { get; set; } = "";
        public combination combination { get; set; } = new combination();
        public List<currentPickOption> currentPickOption { get; set; } = new List<currentPickOption>();
        public int numberOfAldut { get; set; } = 0;
        public int numberOfChildrend { get; set; } = 0;
        public int couponValue { get; set; } = 0;
        public int couponType { get; set; } = 0;
        public int totalPrice { get; set; } = 0;
        public string avatar { get; set; } = string.Empty;
        public string bookingName { get; set; } = string.Empty;
        public int productId { get; set; } = 0;
        public int productChildId { get; set; } = 0;
        public string bookingParentName { get; set; } = string.Empty;
        public List<ProductBookingNoteGroup> productBookingNoteGroups { get; set; } = new List<ProductBookingNoteGroup>();
        public BookingDiscountSelected discountSelected { get; set; } = new BookingDiscountSelected();
    }

    public class OrderNote
    {
        public string noteSpecial { get; set; }
        public string useAppContact { get; set; }
        public string useAppContactValue { get; set; }
        public string useDiffrenceNumber { get; set; }
    }
    public class BookingDiscountSelected
    {
        public string couponCode { get; set; } = "";
        public string couponDescription { get; set; } = "";
        public decimal couponPrice { get; set; } = 0;
    }
    public class combination
    {
        public int id { get; set; } = 0;
        public string zoneList { get; set; } = "";
        public int priceEachNguoiLon { get; set; } = 0;
        public int priceEachTreEm { get; set; } = 0;
        public int netEachNguoiLon { get; set; } = 0;
        public int netEachTreEm { get; set; } = 0;
        public int productId { get; set; } = 0;
        public List<int> convertedZoneList { get; set; } = new List<int>();
    }

    public class currentPickOption
    {
        public string parentGroup { get; set; } = "";
        public int pickItem { get; set; } = 0;
        public string pickItemName { get; set; } = "";
    }


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

    public class RequestGetOrdersByCustomerId
    {
        public int customerId { get; set; }
        public string cultureCode { get; set; }
    }
    public class ResponseGetOrdersByCustomerId
    {
        public int OrderId { get; set; } = 0;
        public string OrderCode { get; set; } = string.Empty;
        public int OrderDetailId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public string QuantityAdult { get; set; } = string.Empty;
        public string QuantityChildren { get; set; } = string.Empty;
        public string LogPrice { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
        public string ActiveStatus { get; set; } = string.Empty;
        public string MetaData { get; set; } = string.Empty;
        public int ProductChildId { get; set; } = 0;
        public string ProductChildTitle { get; set; } = string.Empty;
        public int ProductParentId { get; set; } = 0;
        public string ProductParentTitle { get; set; } = string.Empty;
        public string ProductParentUrl { get; set; } = string.Empty;
        public string ProductParentAvatar { get; set; } = string.Empty;
    }

    public class RequestGetOrderItemFullDetail
    {
        public int customerId { get; set; }
        public string orderCode { get; set; }
        public string orderDetailId { get; set; }
        public string cultureCode { get; set; }
    }
    public class ResponseGetOrderItemFullDetail
    {
        public int OrderId { get; set; } = 0;
        public string OrderCode { get; set; } = string.Empty;
        public int OrderDetailId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public int quantityAldut { get; set; } = 0;
        public int QuantityChildren { get; set; } = 0;
        public decimal LogPrice { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
        public string ActiveStatus { get; set; } = string.Empty;
        public string MetaData { get; set; } = string.Empty;
        public int ProductChildId { get; set; } = 0;
        public string ProductChildTitle { get; set; } = string.Empty;
        public int ProductParentId { get; set; } = 0;
        public string ProductParentTitle { get; set; } = string.Empty;
        public string ProductParentUrl { get; set; } = string.Empty;
        public string ProductParentAvatar { get; set; } = string.Empty;
        public string productParentLichTour { get; set; } = string.Empty;
        public string productParentThuTucVisa { get; set; } = string.Empty;
        public string productThongTinTour { get; set; } = string.Empty;


        //[cc].Fullname, [cc].Email, [cc].PhoneNumber
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string ZoneTitles { get; set; } = "";
        public DateTime? PickingDate { get; set; } = null;
        public string unit { get; set; }
    }
    public class ResponseGetOrderItemFullDetailForSupplier
    {
        public int OrderId { get; set; } = 0;
        public string OrderCode { get; set; } = string.Empty;
        public int OrderDetailId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public int quantityAldut { get; set; } = 0;
        public int QuantityChildren { get; set; } = 0;
        public decimal LogPrice { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
        public string ActiveStatus { get; set; } = string.Empty;
        public string MetaData { get; set; } = string.Empty;
        public int ProductChildId { get; set; } = 0;
        public string ProductChildTitle { get; set; } = string.Empty;
        public int ProductParentId { get; set; } = 0;
        public string ProductParentTitle { get; set; } = string.Empty;
        public string ProductParentUrl { get; set; } = string.Empty;
        public string ProductParentAvatar { get; set; } = string.Empty;
        public string productParentLichTour { get; set; } = string.Empty;
        public string productParentThuTucVisa { get; set; } = string.Empty;
        public string productThongTinTour { get; set; } = string.Empty;


        //[cc].Fullname, [cc].Email, [cc].PhoneNumber
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string ZoneTitles { get; set; } = string.Empty;
        public string EmailSupplier { get; set; } = string.Empty;
    }

    public class RequestCancelOrdersOrderDetail
    {
        public int customerId { get; set; }
        public string orderCode { get; set; }
        public int orderDetailId { get; set; }
        public string cultureCode { get; set; }
        public decimal rollbackValue { get; set; } = 0;
        public int rollbackOption { get; set; } = 0;
    }


    public class RequestUpdateOrderDetailCommentWithRating
    {
        public int Id { get; set; } = 0;
        public int OrderDetailId { get; set; } = 0;
        public string TitleComment { get; set; } = "";
        public string ContentComment { get; set; } = "";
        public int Rating { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<string> OldFileUpload { get; set; } = new List<string>();
        public List<string> NewFileUpload { get; set; } = new List<string>();
    }

    public class ResponseOrderDetailCommentWithRating
    {
        public int Id { get; set; } = 0;
        public int OrderDetailId { get; set; } = 0;
        public string TitleComment { get; set; } = "";
        public string ContentComment { get; set; } = "";
        public int Rating { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<string> OldFileUpload { get; set; } = new List<string>();
        public List<string> NewFileUpload { get; set; } = new List<string>();
    }

    public class ResponseGetLastChatDetailBySessionForCustomer
    {
        public string CustomerEmail { get; set; }
        public string OrderCode { get; set; }
        public string Avatar { get; set; }
        public string Content { get; set; }
        public string DefaultLanguage { get; set; }
        public int OrderChatSessionDetailId { get; set; }
        public string Fullname { get; set; }
    }

    public class RequestCheckOrderDetailByEmail
    {
        public string customerEmail { get; set; }
        public int orderDetailId { get; set; }
    }
    public class ResponseCheckOrderDetailByEmail
    {
        public bool isAuthendicated { get; set; }
    }
    public class ResponseOrderNotificationFeedback
    {
        public string OrderCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public DateTime PickingDate { get; set; }
        public string DefaultLanguage { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public int OrderDetailId { get; set; }
    }
    public class RequestSendNewUserEmail
    {
        public int customerId { get; set; }
        public string customerEmail { get; set; }
        public string culture_code { get; set; } = "vi-VN";
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public string customerName { get; set; } = "";
        public string orderCode { get; set; } = "";
    }

    public class RequestSendNewOrderEmail
    {
        public int customerId { get; set; }
        public string customerEmail { get; set; }
        public string culture_code { get; set; } = "vi-VN";
        public string customerName { get; set; } = "";
        public string orderCode { get; set; } = "";
        public string activeStatus { get; set; } = "TAO_MOI";
        public OrderNote orderNotes { get; set; } = new OrderNote();

    }

    //public class OrderDetailMetaData
    //{
    //    public string choosenDate { get; set; }
    //    public Combination combination { get; set; }
    //    public Currentpickoption[] currentPickOption { get; set; }
    //    public int numberOfAldut { get; set; }
    //    public int numberOfChildrend { get; set; }
    //    public int couponValue { get; set; }
    //    public int couponType { get; set; }
    //    public int totalPrice { get; set; }
    //    public string avatar { get; set; }
    //    public string bookingName { get; set; }
    //    public int productId { get; set; }
    //    public int productChildId { get; set; }
    //    public string bookingParentName { get; set; }
    //    public List<NoteOptions> noteOptions { get; set; } = new List<NoteOptions>();
    //}
    public class NoteOptions
    {
        public string title { get; set; }
        public string note { get; set; }
    }
    //public class Combination
    //{
    //    public int id { get; set; }
    //    public string zoneList { get; set; }
    //    public int priceEachNguoiLon { get; set; }
    //    public int priceEachTreEm { get; set; }
    //    public int netEachNguoiLon { get; set; }
    //    public int netEachTreEm { get; set; }
    //    public int productId { get; set; }
    //    public int[] convertedZoneList { get; set; }
    //}

    public class Currentpickoption
    {
        public string parentGroup { get; set; }
        public int pickItem { get; set; }
        public string pickItemName { get; set; }
    }


    public class ResponseGetCouponByProductId
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public int DiscountOption { get; set; }
        public int ValueDiscount { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string ZoneDescription { get; set; }
        public string ZoneContent { get; set; }
        public bool isActive { get; set; } = false;
    }

    public class OrderDetailMetaData
    {
        public string choosenDate { get; set; }
        public Combination combination { get; set; }
        public Currentpickoption[] currentPickOption { get; set; }
        public int numberOfAldut { get; set; }
        public int numberOfChildrend { get; set; }
        public int couponValue { get; set; }
        public int couponType { get; set; }
        public int totalPrice { get; set; }
        public string avatar { get; set; }
        public string bookingName { get; set; }
        public int productId { get; set; }
        public int productChildId { get; set; }
        public string bookingParentName { get; set; }
        public Productbookingnotegroup[] productBookingNoteGroups { get; set; }
        public Discountselected discountSelected { get; set; }
    }

    public class Combination
    {
        public int id { get; set; }
        public string zoneList { get; set; }
        public int priceEachNguoiLon { get; set; }
        public int priceEachTreEm { get; set; }
        public int netEachNguoiLon { get; set; }
        public int netEachTreEm { get; set; }
        public int productId { get; set; }
        public int[] convertedZoneList { get; set; }
    }

    public class Discountselected
    {
        public string couponCode { get; set; }
        public string couponDescription { get; set; }
        public decimal couponPrice { get; set; }
    }

    //public class Currentpickoption
    //{
    //    public string parentGroup { get; set; }
    //    public int pickItem { get; set; }
    //    public string pickItemName { get; set; }
    //}

    public class Productbookingnotegroup
    {
        public int ZoneParentId { get; set; }
        public string ZoneParentName { get; set; }
        public Notelist[] NoteList { get; set; }
    }

    public class Notelist
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int ZoneParentId { get; set; }
        public string ZoneParentName { get; set; }
        public string bookingNoteType { get; set; }
        public string noteOptions { get; set; }
        public Noteoptionitem[] noteOptionItems { get; set; }
        public string notePlaceHolder { get; set; }
        public string noteValue { get; set; }
        public bool bookingNoteRequired { get; set; }
        public bool bookingNoteSendWithMail { get; set; } = false;
    }

    public class Noteoptionitem
    {
        public string label { get; set; }
        public string value { get; set; }
    }

}
