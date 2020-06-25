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
    public class WorkFlowManagement :IDisposable
    {
        public class WorkFlowFunctions :IDisposable
        {
            #region CREATE
            public bool addWorkFlow(WorkFlow param, long dId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var department = db.Department.FirstOrDefault(r => r.DepartmentId == dId);

                        if (department.WorkFlows == null)
                        {
                            department.WorkFlows = new List<WorkFlow>();
                        }
                        if (department.WorkFlows.FirstOrDefault(r => r.Name.ToLower().TrimEnd().Contains(param.Name.ToLower().TrimEnd())) == null)
                        {
                            department.WorkFlows.Add(param);
                        }
                        else
                        {
                            return false;
                        }
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

            public List<WorkFlow> ListWorkFlow(long id)
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.Firm
                        .Include("CustomerFirmManagers").Include("CustomerFirmManagers.Client").Include("CustomerFirmManagers.Client.ManagerFirm")
                        .Include("CustomerFirmManagers.Client.ManagerFirm.Departments").Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows")
                            .FirstOrDefault(q => q.FirmId == id).CustomerFirmManagers.ToList().SelectMany(w => w.Client.ManagerFirm.Departments.SelectMany(t => t.WorkFlows)).ToList();
                }
            } 
            #endregion
            #region UPDATE
            public bool updateWorkFlow(WorkFlow param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var workflow = db.WorkFlow.Find(Id);

                        if (workflow != null)
                        {
                            workflow.Name = param.Name;
                            workflow.Title = param.Title;
                            workflow.Definition = param.Definition; 
                            workflow.State = param.State; 
                            db.SaveChanges();
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            #endregion
            #region DELETE
            public bool deleteWorkFlow(long wfId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var workflow = db.WorkFlow.Include("Works").Include("Works.WorkLists").FirstOrDefault(q => q.WorkFlowId == wfId);
                        if (workflow != null)
                        {
                            var Works = workflow.Works.ToList();
                            var WorkLists = Works.SelectMany(r => r.WorkLists).ToList();
                             
                            foreach (var item in Works)
                            {
                                db.Work.Remove(item);
                            }
                            foreach (var item in WorkLists)
                            {
                                db.WorkList.Remove(item);
                            }

                            db.WorkFlow.Remove(workflow);

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
            public WorkFlow findWorkFlow(long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var workFlow = db.WorkFlow.Include("Works").FirstOrDefault(r => r.WorkFlowId == Id);
                        if (workFlow != null)
                        {
                            return workFlow;
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
                catch (Exception)
                { 
                    return null;
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
