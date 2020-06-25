using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web; 
using WFS.db.Tables;

namespace WFS.web.Models
{
    public class WFS_Model : IDisposable
    {  
        public long wfsId { get; set; }
        public CustomerFirmManager Partner { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<WorkFlow> WorkFlows { get; set; }
        public ICollection<Work> Works { get; set; }

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