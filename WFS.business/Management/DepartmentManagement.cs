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
    public class DepartmentManagement : IDisposable
    {
        public class DepartmentFunctions : IDisposable
        {
            #region CREATE
            public bool addDepartment(Department param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var partner = db.CustomerFirmManager.Include("Client").Include("Client.ManagerFirm").Include("Client.ManagerFirm").FirstOrDefault(r => r.CustomerFirmManagerId == Id); 

                        if (partner.Client.ManagerFirm.Departments == null)
                        {
                            partner.Client.ManagerFirm.Departments = new List<Department>();
                        }
                        if (partner.Client.ManagerFirm.Departments.FirstOrDefault(r => r.Name.ToLower().TrimEnd().Contains(param.Name.ToLower().TrimEnd())) == null)
                        {
                            partner.Client.ManagerFirm.Departments.Add(param);
                        }
                        else
                        {
                            return false;
                        }
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            #endregion
            #region READ
            public List<Department> ListDepartment(long id)
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.Firm.Include("CustomerFirmManagers").Include("CustomerFirmManagers.Client").Include("CustomerFirmManagers.Client.ManagerFirm").Include("CustomerFirmManagers.Client.ManagerFirm.Departments")
                            .FirstOrDefault(q => q.FirmId == id).CustomerFirmManagers.ToList().SelectMany(w => w.Client.ManagerFirm.Departments).ToList(); 
                }
            }
             
            #endregion
            #region UPDATE
            public bool updateDepartment(Department param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var department = db.Department.Find(Id);

                        if (department != null)
                        {
                            department.Name = param.Name;
                            department.Title = param.Title;
                            department.Definition = param.Definition;
                            department.Status = param.Status;
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
            public bool deleteDepartment(long depId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var department = db.Department.Include("WorkFlows").Include("WorkFlows.Works").Include("WorkFlows.Works.WorkLists").FirstOrDefault(q => q.DepartmentId == depId);
                        if (department != null)
                        {
                            var WorkFlows = department.WorkFlows.ToList();
                            var Works = WorkFlows.SelectMany(a => a.Works).ToList();
                            var WorkLists = Works.SelectMany(r => r.WorkLists).ToList();
                             
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

                            db.Department.Remove(department);

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
            public Department findDepartment(long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var department = db.Department.Include("WorkFlows").FirstOrDefault(r => r.DepartmentId == Id);
                        if (department != null)
                        {
                            return department;
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
