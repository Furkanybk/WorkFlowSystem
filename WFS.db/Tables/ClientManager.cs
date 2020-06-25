using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("ClientManager")]
    public class ClientManager : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ClientManagerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public string Contact { get; set; }
        public DateTime Register_Date { get; set; }
        public DateTime Login_Date { get; set; }
        public bool Status { get; set; }
        public bool IsWFSuser { get; set; }

        //1-1 with Firm
        public Firm ManagerFirm { get; set; }
        [Required]
        public long ManagerFirmId { get; set; }

        public User managerUser { get; set; }
        [Required]
        public long managerUserId { get; set; }

        public ICollection<Note> Notes { get; set; } 

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
