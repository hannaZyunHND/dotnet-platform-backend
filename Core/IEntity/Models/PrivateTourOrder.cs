using MI.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class PrivateTourOrder
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CurrentRequest { get; set; }
        public string CurrentRequestCultureCode { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? Status { get; set; } = (int)PrivateTourOrderStatusType.TAO_MOI;
    }
}
