using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("Files")]
    public class Files : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fileId { get; set; }
        public string fileName { get; set; }
        public string fileUrl { get; set; } 

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
