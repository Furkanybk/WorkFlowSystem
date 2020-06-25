using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.web.Models
{
    public class NotificationDetail
    {
        public NotificationDetail() { }

        public long NotificationId { get; set; } 
        public string Title { get; set; }
        public string Message { get; set; } 
        public string DetailUrl { get; set; }
        public string CreateDate { get; set; }
        public string State { get; set; }
        public bool IsSeen { get; set; }
        public bool Status { get; set; }
        public bool NotifStatus { get; set; }
    }
}