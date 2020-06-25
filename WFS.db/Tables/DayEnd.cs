using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("DayEnd")]
    public class DayEnd : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DayEndId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public DateTime Expected_Date { get; set; }
        public bool Status { get; set; }

        //1-1 with Department
        public Firm DepDayEnd { get; set; }
        [Required]
        public long DepDayEndId { get; set; }

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