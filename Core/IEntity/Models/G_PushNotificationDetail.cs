using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class G_PushNotificationDetail
    {
        [Key]
        public int Id { get; set; }

        public int G_PushNotificationId { get; set; }

        public int CustomerGroupDetailId { get; set; }

        public DateTime? PusingSceduleTime { get; set; }

        public bool? IsPushed { get; set; }

        public DateTime? PushedTime { get; set; }
    }
}
