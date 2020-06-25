using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("Notification")]
    public class Notification : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long notificationid { get; set; } 
        public string title { get; set; }
        public string message { get; set; } 
        public string detailUrl { get; set; }
        public DateTime createdate { get; set; }
        public string state { get; set; } 
        public bool isseen { get; set; }
        public bool status { get; set; }
        public bool notifstatus { get; set; } 

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
