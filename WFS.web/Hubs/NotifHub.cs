using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.web.Hubs
{
    public class NotifHub:Hub
    {
        private Object threadSafeCode = new Object();

        public void Send(string message, string action)
        {
            Clients.All.showMessage(message, action);
        }

        public void Start()
        {
            var uId = web.Session.SessionUser.User.User.UserId;
            if ((String.IsNullOrEmpty(HttpRuntime.Cache["Notification" + uId] as string)))
            {
                lock (threadSafeCode)
                {
                    NotificationListener listener = new NotificationListener();
                    string jsonNotifs = listener.GetNotifList();
                    HttpRuntime.Cache["Notification" + uId] = jsonNotifs;
                    Clients.Caller.addMessage(jsonNotifs, uId);
                }
            }
            else
            {
                Clients.Caller.addMessage((HttpRuntime.Cache["Notification" + uId] as string));
            }
        }
    }
}