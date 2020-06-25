using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("Personal")]
    public class Personal : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PersonalId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PRole { get; set; }
        public string BirthDay { get; set; }
        public string Contact { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; } 
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateTime Register_Date { get; set; }
        public DateTime Login_Date { get; set; }
        public bool Status { get; set; }

        //1-1 with Firm
        public Firm OwnFirm { get; set; }
        [Required]
        public long OwnFirmId { get; set; }

        public User personalUser { get; set; }
        [Required]
        public long personalUserId { get; set; } 
        //1-n bir personelin birden fazla not u olabilir
        public ICollection<Note> Notes { get; set; }

        //1-n bir personelin birden fazla chat i olabilir
        public ICollection<Chat>Chats { get; set; }

        //1-n bir personelin birden fazla work ü olabilir
        public ICollection<Work>Works { get; set; }
         
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