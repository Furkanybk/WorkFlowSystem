using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("UserLog")]
    public class UserLog : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public const string SESSION_ACTIVITY_LOG_ID = "__SessionActivityLog";

        public long UserLogId { get; set; }

        [Required]
        public long _UserId { get; set; }
        public User _User { get; set; } 

        public string Controller { get; set; }
        public string Action { get; set; }  

        public DateTime LogDate { get; set; } 

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