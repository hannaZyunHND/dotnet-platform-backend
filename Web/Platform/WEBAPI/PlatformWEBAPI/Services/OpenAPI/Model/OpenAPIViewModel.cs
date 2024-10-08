namespace PlatformWEBAPI.Services.OpenAPI.Model
{
    public class RequestGetProductsInServicesByCouponCode
    {
        public int couponId { get; set; }
        public int serviceId { get; set; }
        public decimal discountValue { get; set; }
        public string cultureCode { get; set; }
        public int index { get; set; } = 1;
        public int size { get; set; } = 12;
    }

    public class RequestGetServicesByCouponId
    {
        public int couponId { get; set; }
        public decimal discountValue { get; set; }
        public string cultureCode { get; set; }
    }

    public class ResponseProductsInServicesByCouponCode
    {
        // Product Information
        public int Id { get; set; }           // Product ID
        public string Title { get; set; }      // Product title
        public string Url { get; set; }        // Product URL
        public string Avatar { get; set; }     // Product avatar (image)
        public decimal Price { get; set; }     // Original price
        public decimal DiscountPrice { get; set; } // Discounted price

        // Zone Information
        public int ZoneId { get; set; }        // Zone ID
        public string ZoneName { get; set; }   // Zone name
        public string ZoneUrl { get; set; }    // Zone URL
    }


    public class ResponseGetServicesByCouponId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
