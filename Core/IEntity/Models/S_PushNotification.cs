using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class S_PushNotification
    {
        [Key]
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public string EmailReceiver { get; set; }

        public string NotificationBannerCode { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationDescription { get; set; }
        public string OrderCode { get; set; }
        public int? OrderDetailId { get; set; }

        public bool? IsPushToClient { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? PushingTime { get; set; }

        public bool? IsReaded { get; set; }
    }
}
