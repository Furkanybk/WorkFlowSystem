using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.web.Models
{
    public class CurrentRootViewModel : IDisposable
    {
        public long Id { get; set; }  
        public string Name { get; set; }  
        public string Mail { get; set; }
        public string Password { get; set; } 


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