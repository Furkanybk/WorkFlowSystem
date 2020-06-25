using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.db.Tables;

namespace WFS.web.Models
{
    public class MainPageViewModel : IDisposable
    {
        // gün sonu raporları

        //Partnerler
        public long partnerCount { get; set; } 
        //Açılan iş sayısı toplam
        public long onGoingWorkCount { get; set; }  
        //Tamamlanan iş sayısı 
        public long completedWorkCount { get; set; }
        //Mesajlar
        public List<Message> messages { get; set; }
        //Son eklenen işler 
        public List<Work> lastWorks { get; set; }

        #region dispose
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
        #endregion
    }
}