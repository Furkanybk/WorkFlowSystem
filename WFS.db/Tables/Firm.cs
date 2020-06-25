using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("Firm")]
    public class Firm : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FirmId { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Contact { get; set; }
        public string Fax { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Url { get; set; } 
        public string Logo { get; set; }
        public DateTime Register_Date { get; set; }
        public bool Status { get; set; }
        public bool showMain { get; set; }

        //1-n CustomerFirmManager
        public ICollection<CustomerFirmManager> CustomerFirmManagers { get; set; } // bir Firmada bırden fazla müşteri firma olabılır
        //1-n Department
        public ICollection<Department> Departments { get; set; } // bir Firmada bırden fazla departman olabılır

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
