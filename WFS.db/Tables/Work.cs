using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("Work")]
    public class Work : IDisposable
    {
        public Work()
        {
            ProgressBar = 0;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long WorkId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Definition { get; set; }
        public DateTime Register_Date { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime Expected_Date { get; set; }
        public DateTime Finish_Date { get; set; }
        public string State { get; set; }
        public bool Priority { get; set; }
        public bool Status { get; set; } 
        public float ProgressBar { get; set; }
        //1-n bir Work un birden fazla worklist i olabilir 
        public ICollection<WorkList> WorkLists { get; set; }
        public ICollection<Files> UploadFiles { get; set; }


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
