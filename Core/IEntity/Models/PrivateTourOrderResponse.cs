using MI.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class PrivateTourOrderResponse
    {
        [Key]
        public int Id { get; set; }
        public int? PrivateTourOrderId { get; set; }
        public string PlanDetail { get; set; }
        public string CustomerPlanUpdate { get; set; }
        public int? Status { get; set; } = (int)PrivateTourOrderResponseStatusType.TAO_MOI;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedDate { get; set; }
    }
}
