using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class G_PushNotification
    {
        [Key]
        public int Id { get; set; }

        public string NotificationName { get; set; }

        public string NotificationIcon { get; set; }

        public string NotificationDescription { get; set; }

        public string NotificationPushLink { get; set; }

        public string NotificationDetail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedByUsername { get; set; }

    }
}
