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
    public class FirmManagement : IDisposable
    {
        public class FirmFunctions : IDisposable
        {
            #region CREATE
            public long addFirm(Firm param)
            {
                try
                {
                   
                    using (cfgContext db = new cfgContext())
                    {
                        var um = new UserManagement.UserFunctions();
                        um.addEmail(param.Mail);
                        db.Firm.Add(param);
                        db.SaveChanges();
                        return param.FirmId;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            public void addCustomerFirm(long id, CustomerFirmManager param)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var firmV = db.Firm.Include("CustomerFirmManagers").FirstOrDefault(r => r.FirmId == id);
                        if (firmV.CustomerFirmManagers == null)
                        {
                            firmV.CustomerFirmManagers = new List<CustomerFirmManager>();
                        }
                        firmV.CustomerFirmManagers.Add(param);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            #endregion
            #region READ
            public  List<Firm> FirmList()
            {
                using (cfgContext db = new cfgContext())
                {
                    return  db.Firm.ToList();
                }
            }
            #endregion
            #region UPDATE
            public bool updateFirm(Firm param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var firm = db.Firm.Find(Id);

                        if (firm != null)
                        {
                            firm.Name = param.Name;
                            firm.Mail = param.Mail;
                            firm.Contact = param.Contact;
                            firm.Fax = param.Fax;
                            firm.City = param.City;
                            firm.State = param.State;
                            firm.Address = param.Address;
                            firm.Url = param.Url;
                            firm.Logo = param.Logo;

                            db.SaveChanges();
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            #endregion
            #region DELETE
            public bool deleteFirm(long firmId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var firm = db.Firm.Find(firmId);
                        if (firm != null)
                        {
                            var um = new UserManagement.UserFunctions();
                            um.dellEmail(firm.Mail);
                            db.Firm.Remove(firm);
                            db.SaveChanges();
                            return true;
                        }
                        else { return false; }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            #endregion

            #region FIND

            public long returnedFirmId { get; set; }
 

            public Firm Firmalar
            {

                get {
                    return new cfgContext().Firm.Include("CustomerFirmManagers").Include("CustomerFirmManagers.Client").Include("CustomerFirmManagers.Client.ManagerFirm").FirstOrDefault(x => x.FirmId == returnedFirmId);
                }
            }

            public Firm findFirm(long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var firm = db.Firm.Include("CustomerFirmManagers")
                            .Include("CustomerFirmManagers.Client")
                            .Include("CustomerFirmManagers.Client.ManagerFirm")
                            .Include("CustomerFirmManagers.Client.ManagerFirm.Departments")
                            .Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows")
                            .Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows.Works")
                            .FirstOrDefault(x => x.FirmId == Id);

                        return firm; 
                    }
                }
                catch (Exception)
                {

                    return null;
                }
            }

            public Firm selectFirm(long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var firm = db.Firm.FirstOrDefault(x => x.FirmId == Id);
                        if (firm != null)
                        {
                            return firm;
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

            
            #endregion

            #region CheckFirm
            public bool isFirmExist(string mail)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        if(db.Email.FirstOrDefault(r=> r.MailAdres.ToLower().TrimEnd().Contains(mail.ToLower().TrimEnd())) == null) 
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {

                    throw;
                }

            }
            #endregion


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
