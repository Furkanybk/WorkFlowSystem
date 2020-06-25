using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("Note")]
    public class Note : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NoteId { get; set; }
        public string Title { get; set; }
        public string NoteTxt { get; set; } 
        public DateTime Register_Date { get; set; }
        public DateTime Update_Date { get; set; }

        //1-n with Personnel

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