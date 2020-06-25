using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("Chat")]
    public class Chat : IDisposable
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ChatId { get; set; }

        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string State { get; set; }
        public DateTime State_Date { get; set; }
        public string Status { get; set; }

        //1-n ChatRoom
        
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