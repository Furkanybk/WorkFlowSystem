﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFS.db.Tables
{
    [Table("WorkList")]
    public class WorkList : IDisposable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long WorkListId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Definition { get; set; } 
        public DateTime Register_Date { get; set; } 
        public string State { get; set; }
        public bool Status { get; set; } 

        //1-1 with Personal
        public Personal WLpersonal { get; set; }
        [Required]
        public long WLpersonalId { get; set; }


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
