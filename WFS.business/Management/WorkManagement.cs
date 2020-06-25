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
    public class WorkManagement : IDisposable
    {
        public class WorkFunctions : IDisposable
        {
            #region CREATE
            public bool addWork(Work param, long Id, List<long> pList)
            { 
                try
                {
                    var nom = new NotificationManager.NotificationFunctions();

                    using (cfgContext db = new cfgContext())
                    {
                        foreach (var item in pList)
                        {
                            var Personal = db.Personal.Include("Works").FirstOrDefault(r => r.PersonalId == item); 

                            if (Personal.Works == null)
                            {
                                Personal.Works = new List<Work>();
                            }

                            Personal.Works.Add(param);
                        }
                        var WorkFlow = db.WorkFlow.Include("Works").FirstOrDefault(r => r.WorkFlowId == Id);
                        
                        if (WorkFlow.Works == null)
                        {
                            WorkFlow.Works = new List<Work>();
                        } 

                        WorkFlow.Works.Add(param); 
                        db.SaveChanges();

                        foreach (var item in pList)
                        {
                            var Personal = db.Personal.Include("personalUser").Include("personalUser.Notifications").FirstOrDefault(r => r.PersonalId == item); 
                             
                            nom.addNotification("addWork",param,Personal.personalUser.UserId);
                        } 
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            public bool addWorkPiece(WorkList param, long wId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var Work = db.Work.Include("WorkLists").FirstOrDefault(r => r.WorkId == wId);

                        if (Work.WorkLists == null)
                        {
                            Work.WorkLists = new List<WorkList>();
                        }
                        if (Work.WorkLists.FirstOrDefault(r => r.Name.ToLower().TrimEnd().Contains(param.Name.ToLower().TrimEnd())) == null)
                        { 
                            Work.WorkLists.Add(param);
                        }
                        else
                        {
                            return false;
                        }
                        db.SaveChanges();
                        if (Work.State.Contains("Yeni") || Work.State.Contains("Tamamlandı"))
                        {
                            updateWorkState(wId); 
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            #endregion
            #region READ
            public List<Work> WorkList()
            {
                using (cfgContext db = new cfgContext())
                {
                    return  db.Work.ToList();
                }
            }

            public long getWorkList(string str, long id)
            {
                var count = 0;
                using (cfgContext db = new cfgContext())
                {
                    switch (str)
                    {
                        case "finished": 
                            count = db.Firm.Include("CustomerFirmManagers").Include("CustomerFirmManagers.Client").Include("CustomerFirmManagers.Client.ManagerFirm.Departments").Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows").Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows.Works")
                                .FirstOrDefault(r => r.FirmId == id).CustomerFirmManagers.Select(w => w.Client).SelectMany(e => e.ManagerFirm.Departments).SelectMany(w => w.WorkFlows).SelectMany(q => q.Works)
                                .Where(t => t.State == "Tamamlandı").ToList().Count;
                            break;
                        case "ongoing":
                            count = db.Firm.Include("CustomerFirmManagers").Include("CustomerFirmManagers.Client").Include("CustomerFirmManagers.Client.ManagerFirm.Departments").Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows").Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows.Works")
                                .FirstOrDefault(r => r.FirmId == id).CustomerFirmManagers.Select(w => w.Client).SelectMany(e => e.ManagerFirm.Departments).SelectMany(w => w.WorkFlows).SelectMany(q => q.Works)
                                .Where(t => t.State == "Çalışılıyor").ToList().Count;
                            break; 
                    }
                    return count;
                    
                }
            }

            public List<Work> LastWorkList(long id)
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.Firm.Include("CustomerFirmManagers").Include("CustomerFirmManagers.Client").Include("CustomerFirmManagers.Client.ManagerFirm.Departments").Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows").Include("CustomerFirmManagers.Client.ManagerFirm.Departments.WorkFlows.Works")
                        .FirstOrDefault(r => r.FirmId == id).CustomerFirmManagers.Select(w => w.Client).SelectMany(e => e.ManagerFirm.Departments).SelectMany(w => w.WorkFlows).SelectMany(q => q.Works)
                        .OrderByDescending(x => x.Register_Date).Take(7).ToList(); 
                }
            }

            #endregion
            #region UPDATE
            public bool updateWork(Work param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var work = db.Work.Find(Id);

                        if (work != null)
                        {
                            work.Name = param.Name;
                            work.Title = param.Title;
                            work.Definition = param.Definition;
                            work.Status = param.Status; 
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

            public bool updateWorkState(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var work = db.Work.Find(id);

                        if(work.State == "Tamamlandı")
                        {
                            work.State = "Çalışılıyor";
                            work.Finish_Date = default(DateTime);
                        }
                        else
                        {
                            work.State = "Çalışılıyor";
                            work.Start_Date = DateTime.Now;
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
            #region DELETE
            public bool deleteWork(long wId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var work = db.Work.Include("WorkLists").Include("UploadFiles").FirstOrDefault(r => r.WorkId == wId);
                        if (work != null)
                        {
                            var worklists = work.WorkLists.ToList();
                            var workFiles = work.UploadFiles.ToList();

                            foreach (var item in worklists)
                            {
                                db.WorkList.Remove(item);
                            }

                            foreach(var item in workFiles)
                            {
                                db.UploadFiles.Remove(item);
                            }
                            db.Work.Remove(work);
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

            public bool deleteWorkList(long wId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var workL = db.WorkList.Find(wId);
                        if (workL != null)
                        {
                            db.WorkList.Remove(workL);
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
            public Work findWork(long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var work = db.Work.Include("WorkLists").Include("UploadFiles").FirstOrDefault(r => r.WorkId == Id);
                        if (work != null)
                        {
                            return work;
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

            public bool completeWorkList(long id, long wId)
            {
                using (cfgContext db = new cfgContext())
                {
                    var wList = db.WorkList.Find(id);
                    if (wList == null)
                    {
                        return false;
                    }
                    else
                    {
                        wList.State = "Tamamlandı";
                        db.SaveChanges(); 
                        return true;
                    } 
                }
            }

            public bool calculateBar(long wId)
            {
                using (cfgContext db = new cfgContext())
                {
                    var work = db.Work.Find(wId);
                    var Wlists = db.Work.Include("WorkLists").FirstOrDefault(r => r.WorkId == wId).WorkLists;
                    float WlistsCount = Wlists.Count();
                    float CompletedWlistsCount = Wlists.Where(x => x.State.Contains("Tamamlandı")).Count();
                    var bar = work.ProgressBar;
                    if(!WlistsCount.Equals(0))
                        bar = (CompletedWlistsCount / WlistsCount) * 100;

                    if (bar == 100 && work.State != "Tamamlandı")
                    {
                        work.State = "Tamamlandı";
                        work.Finish_Date = DateTime.Now;
                    }
                    work.ProgressBar = bar;
                    db.SaveChanges();
                    return true;
                }
            }

            public bool checkWorkName(long id, string name)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    { 
                        if (db.WorkFlow.Include("Works").FirstOrDefault(r => r.WorkFlowId == id).Works.FirstOrDefault(q => q.Name == name) != null)
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
