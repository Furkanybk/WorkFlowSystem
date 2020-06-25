using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.db.Tables;

namespace WFS.web.Models
{
    public class CurrentUserViewModel :IDisposable
    {
        public long Id { get; set; } 
        public Firm ownFirm { get; set; }
        public long userId { get; set; }
        public string Role { get; set; }
        public string PRole { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Birthday { get; set; }
        public string Contact { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool IsWFSuser { get; set; }
        public List<Notification> Notifys { get; set; }


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