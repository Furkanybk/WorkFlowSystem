using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.web.Models
{
    public class NotificationStatus
    { 
        public long userId { get; set; }
        public List<NotificationDetail> NotifDetailsList;
        public NotificationStatus()
        {
            NotifDetailsList = new List<NotificationDetail>();
        }
    }
}