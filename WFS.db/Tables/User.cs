using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("User")]
    public class User : IDisposable
    { 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }
        public string Token { get; set; } 
        public string UserName { get; set; }
        public string EncryptedPassword { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
        public bool EmailVeryfied { get; set; }

        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Message> Messages { get; set; }

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
