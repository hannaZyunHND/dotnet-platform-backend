using MI.Entity.Models;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using PlatformWEBAPI.Services.Zone.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Services.Product.ViewModel
{
    public class ProductPriceInZoneListByDateWithIsPass
    {
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
        public bool isPast { get; set; } = false;
    }
    public class RequestGetProductDetail
    {
        public int id { get; set; } = 0;
        public string alias { get; set; }
        public string cultureCode { get; set; }
    }
    public class RequestGetProductPriceOptions
    {
        public int id { get; set; }
        public string cultureCode { get; set; }
    }

    public class RequestGetProductOptionsPriceByDate
    {
        public int id { get; set; }
        public string combination { get; set; }
        public int month { get; set; }
        public int year { get; set; } = DateTime.Now.Year;
    }

    public class RequestGetProductLastSeen
    {
        public List<int> ids { get; set; }
        public string cultureCode { get; set; }
    }

    public class RequestPriceFromProductOptionCombination
    {
        public int productId { get; set; }
        public List<int> Combinations { get; set; }
    }

    public class RequestAddViewCount
    {
        public int productId { get; set; }
    }

    public class RequestGetCouponByProductId
    {
        public int productId { get; set; }
        public string culture_code { get; set; }
    }
    public class RequestCheckCouponCode
    {
        public int productId { get; set; }
        public string culture_code { get; set; }
        public string couponCode { get; set; }
        public string customerEmail { get; set; } = "";
    }

    public class RequestCheckChatSessionByCustomerEmail
    {
        public string customerEmail { get; set; }

    }
    public class ResponseGetProductPriceOptions
    {
        public ZoneByTreeViewMinify zoneParent { get; set; } = new ZoneByTreeViewMinify();
        public List<ZoneByTreeViewMinify> zoneChilds { get; set; } = new List<ZoneByTreeViewMinify>();
        public List<ProductPriceInZoneList> combinations { get; set; } = new List<ProductPriceInZoneList>();

    }
    public class ResponseGetProductDetail
    {
        public ProductDetail productDetail { get; set; }
    }

    public class ProductMapLocation
    {
        public int Id { get; set; }
        public string googleMapCrood { get; set; } = "0-0";
        public decimal lat { get; set; } = 0;
        public decimal lng { get; set; } = 0;
        public bool showInfo { get; set; } = false;
    }
    public class ProductMinify
    {
        public int ZoneId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public int ViewCount { get; set; }
        public string Url { get; set; }
        public string PropertyId { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public int IsHot { get; set; }
        public string BigThumb { get; set; }
        public decimal Rate { get; set; }
        public string SpecName { get; set; }
        public string SpecValue { get; set; }
        public int CountRate { get; set; }
        public string Color { get; set; }
        public string PromotionIds { get; set; }
        public int FlashSaleId { get; set; }
        public DateTime FlashSaleStartTime { get; set; }
        public DateTime FlashSaleEndTime { get; set; }
        public decimal ProductPriceInFlashSale { get; set; }
        public int ProductQuantityInFlashSale { get; set; }
        public string Unit { get; set; }
        public int SortOrder { get; set; }
        public decimal DiscountPercent { get; set; }
        public string ZoneName { get; set; }
        public decimal ConfigPrice { get; set; }
        public string ConfigNote { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Name { get; set; }
        public int ProductOldRenewalId { get; set; }
        public string DescriptionType { get; set; }
        public int? ProductLocationId { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal PriceRefer { get; set; }
        public List<PromotionInProduct> promotionInProducts { get; set; } = null;
        public string PriceStr { get; set; }
        public string Linktar { get; set; }
        public string JsonProduct { get; set; }
        public string PercentStr { get; set; }
        public string DiscountStr { get; set; }
        public string DifferenceStr { get; set; }
        public string DefaultPriceStr { get; set; }
        public string PriceReferStr { get; set; }
        public int OldProductId { get; set; }
        public string LinkImageAvatar { get; set; }
        public decimal GiaNguoiLon { get; set; }
        public decimal GiaTreEm { get; set; }
        public decimal GiaEmBe { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string NgayDem { get; set; }
        public string Code { get; set; }
        public int IsTraGop { get; set; }

        public string GradientColor { get; set; }
        public string Coverage { get; set; }
        public string DataLimit { get; set; }
        public string Validity { get; set; }
        public string SimType { get; set; }
        public string SmsNumber { get; set; }
        public string PhoneMinute { get; set; }
        public int OrderId { get; set; }
        public string ICCID { get; set; }
        public DateTime OrderCreatedDate { get; set; }
        public decimal LogPrice { get; set; }
        public string stringCoverage { get; set; }
        public string phoneMinuteInNetwork { get; set; }
        public string phoneMinuteOutNetwork { get; set; }
        public string phoneMinuteInRegion { get; set; }
        public string phoneMinuteOutRegion { get; set; }
        public string simPack { get; set; }
        public bool isActive { get; set; } = false;
        public string tagCombineds { get; set; }
        public bool isShowDetail { get; set; } = false;
        public string Description { get; set; }
        public string googleMapCrood { get; set; } = "0-0";
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
        public int? TotalSale { get; set; } = 0;
        // Zone Service properties
        public int zServiceId { get; set; }
        public string zServiceName { get; set; }
        public string zServiceAvatar { get; set; }
        public string zServiceIcon { get; set; }
        public string zServiceBanner { get; set; }

        // Zone Destination properties
        public int zDestinationId { get; set; }
        public string zDestinationName { get; set; }
        public string zDestinationAvatar { get; set; }
        public string zDestinationIcon { get; set; }
        public string zDestinationBanner { get; set; }
        //,[o].OrderCode, [od].ICCID, [od].LogPrice, [o].CreatedDate [OrderCreatedDate]
    }

    public class ResponsePromotionDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Avatar { get; set; }
        public string Banner { get; set; }
        public string Background { get; set; }
        public string Icon { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public bool isHaveProduct { get; set; } = false;
        public List<ResponseZoneInPromotion> destinations { get; set; } = new List<ResponseZoneInPromotion>();
        public List<ResponseZoneInPromotion> services { get; set; } = new List<ResponseZoneInPromotion>();
        public DateTime? endingTime { get; set; }
        public string discountCode { get; set; }
    }

    public class ResponseZoneInPromotion
    {
        public int zoneId { get; set; }
        public string zoneName { get; set; }
        public string zoneAvatar { get; set; }
        public string zoneBanner { get; set; }
        public string zoneIcon { get; set; }
        public List<ProductMinify> products { get; set; }
        public List<ProductMinify> filterdProducts { get; set; }
        public List<ResponseZoneInPromotion> subZones { get; set; } = new List<ResponseZoneInPromotion>();
        public bool isActive { get; set; } = false;

    }

    public class ProductDetail
    {
        public int Id { get; set; }
        public string AvatarArray { get; set; }
        public string Warranty { get; set; }
        public int? ManufacturerId { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public int? Quantity { get; set; }
        public string PropertyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string PromotionInfo { get; set; }
        public string Catalog { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string SocialTitle { get; set; }
        public string SocialDescription { get; set; }
        public string SocialImage { get; set; }
        public string LanguageCode { get; set; }
        public double? RateAVG { get; set; }
        public int? TotalRate { get; set; }

        public int? ZoneId { get; set; }
        public string ZoneUrl { get; set; }
        public string ZoneName { get; set; }

        public int? ZoneCategoryId { get; set; }
        public string ZoneCategoryUrl { get; set; }
        public string ZoneCategoryName { get; set; }

        public string Avatar { get; set; }
        public int? Five_Star { get; set; }
        public int? Four_Star { get; set; }
        public int? Three_Star { get; set; }
        public int? Two_Star { get; set; }
        public int? One_Star { get; set; }
        public int? FlashSaleId { get; set; }
        public DateTime? FlashSaleStartTime { get; set; }
        public DateTime? FlashSaleEndTime { get; set; }
        public decimal? ProductPriceInFlashSale { get; set; }
        public int? ProductQuantityInFlashSale { get; set; }
        public string MetaFile { get; set; }
        public int? MaterialType { get; set; }
        public int? ViewCount { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? ParentId { get; set; }
        public int? ProductCpnId { get; set; }
        public string ArticleId { get; set; }
        public string NgayDem { get; set; }
        public string PhuongTien { get; set; }
        public string LichTour { get; set; }
        public string ThongTinTour { get; set; }
        public string ThuTucVisa { get; set; }
        public decimal? GiaNguoiLon { get; set; }
        public decimal? GiaTreEm { get; set; }
        public decimal? GiaEmBe { get; set; }
        public DateTime? NgayBatDau { get; set; } = DateTime.Now;
        public string GradientColor { get; set; }
        public string Coverage { get; set; }
        public string DataLimit { get; set; }
        public string Validity { get; set; }
        public string TopUpsAvalible { get; set; }
        public string SimType { get; set; }
        public string SmsNumber { get; set; }
        public string PhoneMinute { get; set; }

        //[p].PhoneMinute, [p].SmsNumber, [cov].stringCoverage
        public string stringCoverage { get; set; }
        public string phoneMinuteInNetwork { get; set; }
        public string phoneMinuteOutNetwork { get; set; }
        public string phoneMinuteInRegion { get; set; }
        public string phoneMinuteOutRegion { get; set; }
        public string simPack { get; set; }
        public string joytelProductCode { get; set; }

        public List<ResponseReviewModel> reviews { get; set; } = new List<ResponseReviewModel>();

        public List<ProductMinify> productChilds { get; set; } = new List<ProductMinify>();
        public List<ProductMinify> productSameZones { get; set; } = new List<ProductMinify>();

        public List<ProductBookingNoteGroup> productBookingNoteGroups { get; set; } = new List<ProductBookingNoteGroup>();
        public string location { get; set; }
        public string locationIframe { get; set; }
        public string tagCombineds { get; set; }
        public int? TotalSale { get; set; } = 0;
        public string NoteOptions { get; set; } = "";
        public List<ProductCommentFeedback> feedbacks { get; set; } = new List<ProductCommentFeedback>();
        public int? totalFeedback { get; set; } = 0;
        //public string zoneCategoryUrl { get; set; }
        //public string zoneCategoryName { get; set; }
        //public int zoneCategoryId { get; set; }




    }
    public class ProductBookingNoteGroup
    {
        public int ZoneParentId { get; set; }
        public string ZoneParentName { get; set; }
        public List<ProductBookingNote> NoteList { get; set; } = new List<ProductBookingNote>();
    }
    public class ProductBookingNote
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int ZoneParentId { get; set; }
        public string ZoneParentName { get; set; }
        public string bookingNoteType { get; set; }
        public string noteOptions { get; set; }
        public List<NoteOptionItem> noteOptionItems { get; set; } = new List<NoteOptionItem>();
        public string notePlaceHolder { get; set; }
        public string noteValue { get; set; } = "";
        public bool? bookingNoteRequired { get; set; } = false;
        public bool? bookingNoteSendWithMail { get; set; } = false;
        public int? addHours { get; set; } = 0;
    }
    public class RequestGetProductCommentFeedback
    {
        public int productId { get; set; }
        public int index { get; set; }
        public int size { get; set; }
    }
    public class RequestPromotionDetail
    {
        public int promotionId { get; set; }
        public string cultureCode { get; set; }
    }
    public class ProductCommentFeedback
    {
        public string customerId { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public decimal? rating { get; set; }
        public string commentContent { get; set; }
        public DateTime? createdDate { get; set; }
        public string commentTitle { get; set; }
        public string commentImages { get; set; }
        public List<string> images { get; set; } = new List<string>();
        public List<int> ratingStars { get; set; } = new List<int>();
    }

    public class NoteOptionItem
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    public class ProductTopUpAvalible
    {
        public string title { get; set; }
        public string data { get; set; }
        public string validaty { get; set; }
        public string price { get; set; }
    }

    public class ProductSpectificationDetail
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Url { get; set; }

    }

    public class ProductPriceInLocationDetail
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }

    }
    public class PromotionInProduct
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public int IsDiscountPrice { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }

    }

    public class SpectificationEstimates
    {
        public int SpectificationId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int TotalProduct { get; set; }
    }
    public class SpectificationEstimatesCooked
    {
        public int SpectificationId { get; set; }
        public string Name { get; set; }
        public List<SpectificationEstimates> Values { get; set; }
    }

    public class FilterArea
    {
        public int SpectificationId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Value { get; set; }
        public int ZoneId { get; set; }
    }
    public class FilterAreaCooked
    {
        public int SpectificationId { get; set; }
        public string Name { get; set; }
        public List<FilterArea> Values { get; set; }
    }
    public class FilterSpectification
    {
        public int SpectificationId { get; set; }
        public string Value { get; set; }
    }

    public class FilterProductBySpectification
    {
        public int parentId { get; set; }
        public string lang_code { get; set; }
        public int locationId { get; set; }
        public string manufacture_id { get; set; }
        public int min_price { get; set; }
        public int max_price { get; set; }
        public int sort_price { get; set; }
        public int sort_rate { get; set; }
        public string color_code { get; set; } = "";
        //List<FilterSpectification> filter
        public List<FilterSpectification> filter { get; set; }
        public string filter_text { get; set; } = "";
        public int material_type { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public double fromPrice { get; set; }
        public double toPrice { get; set; }

    }
    public class TagViewModel
    {
        public int Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool Invisibled { get; set; }
        public bool IsHotTag { get; set; }
        public int Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        public string LanguageCode { get; set; }
    }
    public partial class ProductOldRenewalViewModel
    {
        public decimal DiscountAmount { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercent { get; set; }
        public int ProductOldRenewalId { get; set; }
        public int ProductId { get; set; }
        public string Avatar { get; set; }
        public string Url { get; set; }
        public string DescriptionType { get; set; }
        public int? ProductLocationId { get; set; }
        public int? ZoneId { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal PriceRefer { get; set; }
        public string PriceStr { get; set; }
        public string Linktar { get; set; }
        public string JsonProduct { get; set; }
        public string PercentStr { get; set; }
        public string DiscountStr { get; set; }
        public string DifferenceStr { get; set; }
        public string DefaultPriceStr { get; set; }
        public string Title { get; set; }
    }
    public class TourItem
    {
        public string tieuDe { get; set; }
        public string noiDung { get; set; }
    }
    public class ResponseReviewModel
    {
        public string userName { get; set; }
        public string country { get; set; }
        public string content { get; set; }
        public List<string> images { get; set; }
        public int startNumber { get; set; }
        public string avatar { get; set; }
    }

    public class EsSearchItemResult
    {
        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemAvatar { get; set; }
        public string itemType { get; set; }
        public string itemDescription { get; set; }
        public decimal itemPrice { get; set; }
        public string itemSearchKeyword { get; set; }
        public string itemSearchKeywordNorm { get; set; } = "";
        public string itemSearchKeywordUpper { get; set; } = "";
        public string LanguageCode { get; set; }
    }
}
