using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("Message")]
    public class Message : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MessageId { get; set; }
        public string MessageName { get; set; }
        public string MessageTxt { get; set; }
        public DateTime MessageDate { get; set; }
        public string url { get; set; }
        public bool MessageRead { get; set; }
        public bool MessageTag { get; set; }
        public bool MessageTrash { get; set; } 
        public bool State { get; set; }  
        public string SenderUserName { get; set; }

        public User SenderUser { get; set; }
        [Required]
        public long SenderUserId { get; set; } 

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
