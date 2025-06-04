using System.Collections.Generic;
using System;

namespace PlatformCMS.DTOModels
{
    public class GNotificationCreateModel
    {
        public string NotificationName { get; set; }
        public string NotificationIcon { get; set; }
        public string NotificationDescription { get; set; }
        public string NotificationPushLink { get; set; }
        public string NotificationDetail { get; set; }
        public List<GNotificationDetailModel> Details { get; set; }
    }

    public class GNotificationDetailModel
    {
        public int CustomerGroupDetailId { get; set; }
        public DateTime? PusingSceduleTime { get; set; }
    }
}
