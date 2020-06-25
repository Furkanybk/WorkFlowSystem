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
    public class PartnerManagement : IDisposable
    {

        public class PartnerFunctions : IDisposable
        {
            #region CREATE
            public bool addPartner(CustomerFirmManager param)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.CustomerFirmManager.Add(param);
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false; 
                }
            }
            #endregion
            #region READ
            public List<CustomerFirmManager> ListCustomerFirm()
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.CustomerFirmManager.ToList();
                }
            }
            #endregion
            #region UPDATE
            public bool updatePartner(CustomerFirmManager param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var partner = db.CustomerFirmManager.Find(Id);

                        if (partner != null)
                        {
                            //partner.Firm_Name = param.Firm_Name;
                            //partner.Name = param.Name;
                            //partner.Surname = param.Surname;
                            //partner.Email = param.Email;
                            //partner.Password = param.Password;
                            //partner.Mail = param.Mail;
                            //partner.Contact = param.Contact;
                            //partner.Fax = param.Fax;
                            //partner.City = param.City;
                            //partner.State = param.State;
                            //partner.Address = param.Address;

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
            public bool deletePartner(long partnerId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var partner = db.CustomerFirmManager.Include("Client").Include("Client.ManagerFirm")
                            .Include("Client.ManagerFirm.Departments").Include("Client.ManagerFirm.Departments.WorkFlows")
                            .Include("Client.ManagerFirm.Departments.WorkFlows.Works").Include("Client.ManagerFirm.Departments.WorkFlows.Works.WorkLists")
                            .FirstOrDefault(q => q.CustomerFirmManagerId == partnerId);
                        if (partner != null)
                        { 
                            var Departments = partner.Client.ManagerFirm.Departments.ToList();
                            var WorkFlows = Departments.SelectMany(p => p.WorkFlows).ToList();
                            var Works = WorkFlows.SelectMany(a => a.Works).ToList();
                            var WorkLists = Works.SelectMany(r => r.WorkLists).ToList();

                            foreach (var item in Departments)
                            {
                                db.Department.Remove(item);
                            }
                            foreach (var item in WorkFlows)
                            {
                                db.WorkFlow.Remove(item);
                            }
                            foreach (var item in Works)
                            {
                                db.Work.Remove(item);
                            }
                            foreach (var item in WorkLists)
                            {
                                db.WorkList.Remove(item);
                            }

                            db.CustomerFirmManager.Remove(partner);
                            db.SaveChanges();
                            return true;
                        }
                        else { return false; }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            
            #endregion

            #region FIND
            public List<CustomerFirmManager> getPartners(long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        List<CustomerFirmManager> partners  = db.Firm.Include("CustomerFirmManagers").FirstOrDefault(r => r.FirmId == Id).CustomerFirmManagers.ToList(); 
                        if (partners != null)
                        {
                            return partners;
                        }
                        else
                        {
                            return null;
                        } 
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            #endregion

            public ClientManager selectManager(long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var manager = db.ClientManager.Include("ManagerFirm").FirstOrDefault(x => x.ManagerFirmId == Id);
                        if (manager != null)
                        {
                            return manager;
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

            public CustomerFirmManager findPartner(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var customer = db.CustomerFirmManager.Include("Client").Include("Client.ManagerFirm").Include("Client.ManagerFirm.Departments").Include("Client.ManagerFirm.Departments.WorkFlows").Include("Client.managerUser").FirstOrDefault(r => r.CustomerFirmManagerId == id);
                        if(customer != null)
                        {
                            return customer;
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

            public bool isPartnerExist(long partnerFirmId,long currentFirmId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var fm = new FirmManagement.FirmFunctions();
                        
                        var currentFirm = fm.findFirm(currentFirmId);
                        var managers = db.Firm
                            .Include("CustomerFirmManagers").Include("CustomerFirmManagers.Client").Include("CustomerFirmManagers.Client.ManagerFirm")
                            .FirstOrDefault(q => q.FirmId == currentFirmId).CustomerFirmManagers.ToList();

                        var customerFirm = managers.FirstOrDefault(w => w.Client.ManagerFirm.FirmId == partnerFirmId);

                        if (customerFirm != null)
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
