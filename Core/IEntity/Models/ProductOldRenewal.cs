namespace MI.Entity.Models
{
    public partial class ProductOldRenewal
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string LanguageCode { get; set; }
        public int LocationId { get; set; }
        public decimal? PriceRefer { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string DescriptionType { get; set; }
        public int? Status { get; set; }
    }
}
