using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("ChatRoom")]
    public class ChatRoom : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ChatRoomId { get; set; }
        public string Name { get; set; }
        public DateTime Register_Date { get; set; }
        public string Status { get; set; }

        //1-n Chat

        public ICollection<Chat>Chats { get; set; } // bir odada bırden fazla chat olabılır

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
