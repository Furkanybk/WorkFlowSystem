using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("CustomerFirmManager")]
    public class CustomerFirmManager : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CustomerFirmManagerId { get; set; }
        public string Token { get; set; }

        //1-1 with ClientManager
        public ClientManager Client { get; set; }
        //[Required]
        public long ClientId { get; set; }
         

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
