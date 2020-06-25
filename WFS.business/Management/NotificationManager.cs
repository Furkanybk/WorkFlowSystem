using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFS.db.Tables;
using WFS.db.WFScontext;

namespace WFS.business.Management
{
    public class NotificationManager : IDisposable
    {
        public class NotificationFunctions : IDisposable
        {
            #region Create
            public void addNotification(string type, Work lastwork, long pId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        switch (type)
                        {
                            case "addWork":

                                Notification notif = new Notification
                                {
                                    title = "Yeni İş Atandı.",
                                    message = lastwork.Name + " adlı iş atandı.",
                                    detailUrl = "/Active/Work_Detail/?id=" + lastwork.WorkId.ToString(),
                                    createdate = DateTime.Now,
                                    isseen = false,
                                    notifstatus = false,
                                    state = "Bildirilmedi",
                                    status = true
                                };

                                var personal = db.Personal.Include("Works").Include("personalUser").Include("personalUser.Notifications").FirstOrDefault(s => s.personalUserId == pId);

                                if (personal.personalUser.Notifications == null) personal.personalUser.Notifications = new List<Notification>();
                                personal.personalUser.Notifications.Add(notif);
                                db.SaveChanges();
                                break;
                        }
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
            }

            public void addMessageNotification(Message msg, long pId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        Notification notif = new Notification
                        {
                            title = "Yeni Mesaj Alındı.",
                            message = msg.SenderUserName + " adlı kişi mesaj gönderdi.",
                            detailUrl = "#",
                            createdate = DateTime.Now,
                            isseen = false,
                            notifstatus = false,
                            state = "Bildirilmedi",
                            status = true
                        };

                        var personal = db.Personal.Include("Works").Include("personalUser").Include("personalUser.Notifications").FirstOrDefault(s => s.personalUserId == pId);

                        if (personal.personalUser.Notifications == null) personal.personalUser.Notifications = new List<Notification>();
                        personal.personalUser.Notifications.Add(notif);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public void createNotif(string title, string message, string state)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        Notification notif = new Notification
                        {
                            title = title,
                            message = message,
                            createdate = DateTime.Now,
                            state = state,
                            isseen = false,
                            status = false,
                            notifstatus = false
                        };
                        db.Notification.Add(notif);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
            }

            public Notification Notif(string title, string message, string state)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        Notification notif = new Notification
                        {
                            title = title,
                            message = message,
                            createdate = DateTime.Now,
                            state = state,
                            isseen = false,
                            status = false,
                            notifstatus = false
                        };
                        return notif;
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            #endregion
            #region Update
            public bool updateNotification(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var notif = db.Notification.FirstOrDefault(r => r.notificationid == id);

                        if (notif.notifstatus == false)
                        {
                            notif.notifstatus = true;
                            notif.state = "Bildirildi";
                            db.SaveChanges();
                        }
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            public bool updateNotificationSeen(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var notif = db.User.Include("Notifications").FirstOrDefault(r => r.UserId == id).Notifications.Where(r => r.isseen == false).ToList(); 

                        foreach (var item in notif)
                        {
                            if (item.isseen == false)
                            {
                                item.isseen = true;
                            } 
                        }

                        db.SaveChanges(); 
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            public List<Notification> getNotifications(long Id)
            {
                using (cfgContext db = new cfgContext())
                { 
                    var user = db.User.Include("Notifications").FirstOrDefault(r => r.UserId == Id);

                     
                    return user.Notifications.ToList();  
                }
            }
            #endregion

            public Notification findNotif(long Id)
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.Notification.Find(Id);
                }
            }

            #region Delete
            public bool deleteNotifications(long Id)
            {
                using (cfgContext db = new cfgContext())
                {
                    try
                    {
                        var userNotifs = db.User.Include("Notifications").FirstOrDefault(r => r.UserId == Id);
                        foreach (var item in userNotifs.Notifications.ToList())
                        {
                            db.Notification.Remove(item);
                        }
                        db.SaveChanges();

                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            #endregion

            public string getTime(DateTime date)
            {
                var currentDT = DateTime.Now;

                var day = Math.Abs(currentDT.Day - date.Day);

                var hour = Math.Abs(currentDT.Hour - date.Hour);

                var minute = currentDT.Minute + (60 - date.Minute);


                if (minute < 60)
                {
                    return minute + " dk önce";
                }
                else if (hour < 24 && hour != 0)
                {
                    return hour + " saat önce";
                }
                else if (day >= 1)
                {
                    return day + " gün önce";
                }
                else
                {
                    return "az önce";
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
