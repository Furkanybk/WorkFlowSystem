using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFS.db.Tables;
using WFS.db.WFScontext;

namespace WFS.business.Management
{
    public class TicketManagement : IDisposable
    {

        public class TicketFunctions : IDisposable
        {
            public List<Ticket> ListTickets()
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.Ticket.ToList();
                }
            }

            public bool addTicket(Ticket param)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    { 
                        db.Ticket.Add(param);
                        db.SaveChanges();
                        return true;   
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            } 

            public bool deleteTicket(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var Ticket = db.Ticket.Find(id);

                        if (Ticket != null)
                        {
                            db.Ticket.Remove(Ticket);
                            db.SaveChanges();
                        }
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public Ticket findTicket(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var Ticket = db.Ticket.Find(id);
                        if (Ticket != null)
                        {
                            return Ticket;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }


            #region Disposee
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
        #region Disposee
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
