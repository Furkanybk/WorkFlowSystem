using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.web.Models
{
    public class FirmManagerVM : IDisposable
    {
        public long firmId { get; set; }
        public string firmLogo { get; set; }
        public string firmName { get; set; }
        public string firmCity { get; set; }
        public DateTime firmRegDate { get; set; }
        public string firmMail { get; set; }
        public string firmContact { get; set; }
        public string firmFax { get; set; }
        public string firmUrl { get; set; } 

        public long managerId { get; set; }
        public string managerName { get; set; }
        public string managerUsername { get; set; }
        public string managerEmail { get; set; }
        public string managerContact { get; set; }
        public DateTime managerLogin_Date { get; set; }
        public bool managerIsWFSuser { get; set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected bool Disposed { get; private set; }
        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        }
    }
}