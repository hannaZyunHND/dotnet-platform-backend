using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Services.Product.ViewModel
{
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

        //,[o].OrderCode, [od].ICCID, [od].LogPrice, [o].CreatedDate [OrderCreatedDate]
    }

    public class ProductDetail
    {
        public int Id { get; set; }
        public string AvatarArray { get; set; }
        public string Warranty { get; set; }
        public int ManufacturerId { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
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
        public double RateAVG { get; set; }
        public int TotalRate { get; set; }
        public int ZoneId { get; set; }
        public string ZoneUrl { get; set; }
        public string Avatar { get; set; }
        public int Five_Star { get; set; }
        public int Four_Star { get; set; }
        public int Three_Star { get; set; }
        public int Two_Star { get; set; }
        public int One_Star { get; set; }
        public int FlashSaleId { get; set; }
        public DateTime FlashSaleStartTime { get; set; }
        public DateTime FlashSaleEndTime { get; set; }
        public decimal ProductPriceInFlashSale { get; set; }
        public int ProductQuantityInFlashSale { get; set; }
        public string MetaFile { get; set; }
        public int MaterialType { get; set; }
        public int ViewCount { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int ParentId { get; set; }
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
        public DateTime NgayBatDau { get; set; } = DateTime.Now;
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


        //public DateTime? NgayBatDau { get; set; }
        //public DateTime? NgayKetThuc { get; set; }
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
