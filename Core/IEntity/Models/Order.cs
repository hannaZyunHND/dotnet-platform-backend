using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MI.Entity.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }
        [Key]
        public int Id { get; set; }
        public string OrderCode { get; set; } = "";
        public int CustomerId { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
        public string Status { get; set; } = "TAO_MOI";
        public string InstallmentData { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string MetaData { get; set; } = "";
        public string Note { get; set; } = "";
        public string Address { get; set; } = "";
        public int OrderSourceType { get; set; } = 0;
        public int OrderSourceId { get; set; } = 0;
        public bool Vat { get; set; } = false;
        public string MetaOrder { get; set; } = string.Empty;
        public Customer Customer { get; set; }

        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
