using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFS.db.Tables;
using WFS.db.WFScontext;

namespace WFS.business.Management
{
    public class Management : IDisposable
    {
       
        public class ManualAdd : IDisposable
        {
            public void add()
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {

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

        public class Login : IDisposable
        {
            public bool createRoot(Root param)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.Root.Add(param);  db.SaveChanges();  return true;
                    }
                }
                catch (Exception)
                { 
                    return false;
                }
            }

            public bool checkLogin(string username, string pass)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var root = db.Root.FirstOrDefault(r => r.Username.ToLower().TrimEnd().Contains(username.ToLower().TrimEnd()) && r.Password.Equals(pass)); 

                        if(root != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public async Task<Root> GetRoot(string UserName, string Password)
            {
                using (cfgContext db = new cfgContext())
                { 
                    return await db.Root.FirstOrDefaultAsync(r => r.Username.ToLower().TrimEnd().Contains(UserName.ToLower().TrimEnd()) && r.Password.Equals(Password));
                }
            }

            public void updateLoginDate(long id, DateTime DT)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.Root.Find(id).Login_Date = DT;
                        db.SaveChanges();
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

        public class Functions : IDisposable
        {
             
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