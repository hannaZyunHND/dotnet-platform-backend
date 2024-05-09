using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Dapper.Data.Models
{
    public class CustomerProductOldRenewal
    {
        public int productId { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public int locationId { get; set; }
        public int type { get; set; }
        public int departmentId { get; set; }
        public int? productIdToExchange { get; set; }
        public decimal? dealPrice { get; set; }
    }
}
