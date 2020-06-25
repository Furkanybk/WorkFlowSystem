using Microsoft.AspNet.SignalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WFS.db.WFScontext;
using WFS.web.Models;

namespace WFS.web.Hubs
{
    public class NotificationListener
    {
        private NotificationStatus notificationStatus;
        private Object threadSafeCode = new Object();
        NpgsqlConnection conn;
        NpgsqlCommand command;
        NpgsqlCommand sqlCmd; 
        public bool notificationflg = false;
        string userid = null;
        public NotificationListener()
        { 
            notificationStatus = new NotificationStatus();
        }
         
        public string GetNotifList()
        {
            this.notificationStatus.NotifDetailsList = new List<NotificationDetail>();
             

            if (conn == null)
            {
                conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=1;Database=wfs_db;MaxPoolSize=1000;Pooling=true;Timeout=500;SyncNotification=true;preload reader=true;");
            }

            try
            {
                sqlCmd = new NpgsqlCommand();

                sqlCmd.Connection = conn;
                if (sqlCmd.Connection.State == ConnectionState.Closed)
                {
                    sqlCmd.Connection.Open();
                }
                 
                if (command == null)
                {
                    command = new NpgsqlCommand("listen notifytickets;", conn);
                    command.ExecuteNonQuery();
                }

                if (userid == null)
                {
                    userid = web.Session.SessionUser.User.User.UserId.ToString();
                }
                 
                //sqlCmd.CommandType = CommandType.Text;
                //sqlCmd.CommandText = "SELECT * FROM dbo.notifyview WHERE userid = " + userid + " ORDER BY createdate DESC"; 
                 
                if (notificationflg == false)
                {
                    conn.Notification += new NotificationEventHandler(OnNotification);
                    notificationflg = true;
                }

                #region oldcode
                //using (cfgContext db = new cfgContext())
                //{
                //    var currentUserId = WFS.web.Session.SessionUser.User.User.UserId;
                //    var notifList = db.User.Include("Notifications").FirstOrDefault(r => r.UserId == currentUserId).Notifications.ToList();

                //    foreach (var item in notifList)
                //    {
                //        NotificationDetail notif = new NotificationDetail()
                //        {
                //            NotificationId = item.notificationid,
                //            Title = item.title,
                //            Message = item.message,
                //            CreateDate = item.createdate.ToString(),
                //            State = item.state,
                //            IsSeen = item.isseen,
                //            Status = item.status,
                //            NotifStatus = item.notifstatus
                //        };
                //        notificationStatus.NotifDetailsList.Add(notif);
                //    }
                //}
                #endregion

                #region reader
                //reader = sqlCmd.ExecuteReader();

                //while (reader.Read())
                //{
                //    NotificationDetail notif = new NotificationDetail();

                //    notif.NotificationId = reader.GetInt64(0);
                //    notif.Title = reader.GetString(1);
                //    notif.Message = reader.GetString(2);
                //    notif.CreateDate = reader.GetDateTime(3).ToString();
                //    notif.State = reader.GetString(4);
                //    notif.IsSeen = reader.GetBoolean(5);
                //    notif.Status = reader.GetBoolean(6);
                //    notif.NotifStatus = reader.GetBoolean(7);

                //    if (!notif.IsSeen)
                //    {
                //        notificationStatus.NotifDetailsList.Add(notif);
                //    }
                //} 
                #endregion

                using (cfgContext db = new cfgContext())
                {
                    var id = Convert.ToInt64(userid);
                    var Notifys = db.User.Include("Notifications").FirstOrDefault(r => r.UserId == id).Notifications.ToList().OrderByDescending(r => r.createdate); 
                    foreach (var item in Notifys)
                    { 
                        if (!item.isseen)
                        {
                            NotificationDetail notif = new NotificationDetail()
                            {
                                NotificationId = item.notificationid,
                                Title = item.title,
                                Message = item.message,
                                DetailUrl = item.detailUrl,
                                CreateDate = item.createdate.ToString(),
                                State = item.state,
                                IsSeen = item.isseen,
                                Status = item.status,
                                NotifStatus = item.notifstatus
                            };
                            notificationStatus.NotifDetailsList.Add(notif);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw;
            } 

            lock (threadSafeCode)
            {
                HttpRuntime.Cache["Notification" + userid] = SerializeObjectToJson(this.notificationStatus);
            }
             
            return (HttpRuntime.Cache["Notification" + userid] as string);
        }


        private void OnNotification(object sender, NpgsqlNotificationEventArgs e)
        { 
            var context = GlobalHost.ConnectionManager.GetHubContext<NotifHub>(); 
            #region actionType
            //string actionName = e.AdditionalInformation.ToString();
            //string actionType = "";
            //if (actionName.Contains("DELETE"))
            //{
            //    actionType = "Delete";
            //}
            //if (actionName.Contains("UPDATE"))
            //{
            //    actionType = "Update";
            //}
            //if (actionName.Contains("INSERT"))
            //{
            //    actionType = "Insert";
            //}  
            #endregion
            context.Clients.All.addMessage(this.GetNotifList()); 
        }
         
        public String SerializeObjectToJson(Object notifStatus)
        {
            try
            {
                return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(notifStatus);
            }
            catch (Exception) { return null; }
        }
    }
}