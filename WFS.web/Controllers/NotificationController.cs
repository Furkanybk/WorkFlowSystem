using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WFS.db.WFScontext;
using WFS.web.Models;

namespace WFS.web.Controllers
{
    [Session.SessionControl()]
    public class NotificationController : Controller
    { 
        public ActionResult Notification()
        {
            MainModel mainModel = new MainModel("getNotifications",web.Session.SessionUser.User.User.UserId);

            return View(mainModel);
        } 

        public async Task<JsonResult> addNotif()
        {
            try
            {
                using (business.Management.NotificationManager.NotificationFunctions nm = new business.Management.NotificationManager.NotificationFunctions())
                {
                    nm.createNotif("","","");

                   return await Task.Run(() => Json(new { result = true }, JsonRequestBehavior.AllowGet));
                }
            }
            catch (Exception e)
            {

                return await Task.Run(() => Json(new { result = false }, JsonRequestBehavior.AllowGet)); 
            }
        }

        [HttpPost]
        public async Task<JsonResult> Notify(long notifId)
        { 
            try
            {
                using (business.Management.NotificationManager.NotificationFunctions nom = new business.Management.NotificationManager.NotificationFunctions())
                { 
                    if(nom.updateNotification(notifId))
                    { 
                        return await Task.Run(() => Json(new { result = true }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        return await Task.Run(() => Json(new { result = false }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false }, JsonRequestBehavior.AllowGet));
            }
        }

        [HttpPost]
        public async Task<JsonResult> NotifySeen()
        { 

            try
            {
                using (business.Management.NotificationManager.NotificationFunctions nom = new business.Management.NotificationManager.NotificationFunctions())
                {
                    var userid = web.Session.SessionUser.User.User.UserId;
                    if (nom.updateNotificationSeen(userid))
                    { 
                        return await Task.Run(() => Json(new { result = true }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    { 
                        return await Task.Run(() => Json(new { result = false }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false }, JsonRequestBehavior.AllowGet));
            }
        }
         
        [HttpPost]
        public async Task<JsonResult> clear()
        {
            try
            {
                var user = web.Session.SessionUser.User.User;
                using (business.Management.NotificationManager.NotificationFunctions nm = new business.Management.NotificationManager.NotificationFunctions())
                {
                    if(nm.deleteNotifications(user.UserId))
                    {
                        return await Task.Run(() => Json(new { result = true, message = "Bildirimler temizlendi." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        return await Task.Run(() => Json(new { result = false, message = "Bildirimler temizlenemedi." }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message= "hata oluştu." }, JsonRequestBehavior.AllowGet));
            }
        }
    }
}