using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WFS.db.Tables;
using WFS.db.WFScontext;
using WFS.web.Models; 

namespace WFS.web.Controllers
{
    [Session.SessionControl()]
    public class MessageController : Controller
    {
        // GET: Message
        [HttpGet]
        public async Task<ActionResult> Index(string type)
        {
            MainModel mainModel = new MainModel("Message");
            ViewBag.type = type; 
            return await Task.Run(() => View(mainModel));
        }

        [HttpGet]
        public async Task<ActionResult> ReadMessage(long message)
        {
            ViewBag.message = message;
            MainModel mainModel = new MainModel("Message", message);
            return await Task.Run(() => View(mainModel));
        }

        [HttpGet]
        public async Task<ActionResult> SendMessage()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getPersonals", user.Firm_Id);
            return await Task.Run(() => View(mainModel));
        }

        [HttpPost]
        public async Task<JsonResult> AddMessage(string Mname, string Mtxt, List<long> personels)
        {
            try
            { 

                using (business.Management.MessageManagement.MessageFunctions mm = new business.Management.MessageManagement.MessageFunctions())
                {
                    cfgContext db = new cfgContext();
                    var user = web.Session.SessionUser.User.User;
                    var nom = new business.Management.NotificationManager.NotificationFunctions();
                    var name = "";

                    if(user.Role == "ClientManager")
                    { 
                        var cm = db.ClientManager.FirstOrDefault(r => r.managerUserId == user.UserId);
                        name = cm.Name +" "+ cm.Surname; 
                    }
                    else
                    { 
                        var p = db.Personal.FirstOrDefault(r => r.personalUserId == user.UserId);
                        name = p.Name + " " + p.Surname; 
                    }

                    Message newMessage = new Message
                    {
                        MessageName = Mname,
                        MessageTxt = Mtxt,
                        MessageDate = DateTime.Now,
                        url = "",
                        MessageTag = false,
                        MessageTrash = false,
                        MessageRead = false,
                        SenderUserName = name,
                        SenderUserId = user.UserId,
                        State = true
                    };

                    bool result = false;

                    foreach (var item in personels)
                    {
                        var Personal = db.Personal.Include("personalUser").Include("personalUser.Notifications").Include("personalUser.Messages").FirstOrDefault(r => r.PersonalId == item);

                        result = mm.addMessage(newMessage, Personal.personalUser.UserId);
                        nom.addMessageNotification(newMessage, Personal.personalUser.UserId);
                    }

                    if (result)
                    {
                        return await Task.Run(() => Json(new { result = true, message = "Mesaj gönderildi." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        return await Task.Run(() => Json(new { result = false, message = "Mesaj gönderilemedi." }, JsonRequestBehavior.AllowGet));
                    } 
                    
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            } 
        }


        [HttpPost]
        public async Task<JsonResult> SetMessageRead(long mId, long uId, string role)
        { 
            try
            {
                using (business.Management.MessageManagement.MessageFunctions mm = new business.Management.MessageManagement.MessageFunctions())
                {
                    if (mm.setRead(uId, mId, role))
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

                throw;
            } 
        }

        [HttpPost]
        public async Task<JsonResult> SetStarEnable(long mId, long uId, string role)
        {
            try
            {
                using (business.Management.MessageManagement.MessageFunctions mm = new business.Management.MessageManagement.MessageFunctions())
                {
                    MainModel mainModel = new MainModel("Message"); 

                    if (mm.setStar(uId, mId, role))
                    { 
                        return await Task.Run(() => Json(new { result = true, updatedModel = mainModel }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        return await Task.Run(() => Json(new { result = false }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteMessage(long mId, long uId, string role)
        {
            try
            {
                using (business.Management.MessageManagement.MessageFunctions mm = new business.Management.MessageManagement.MessageFunctions())
                {
                    if (mm.moveTrash(uId, mId, role))
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

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteMessageAll(long uId, string role)
        {
            try
            {
                using (business.Management.MessageManagement.MessageFunctions mm = new business.Management.MessageManagement.MessageFunctions())
                {
                    if (mm.moveTrashAll(uId, role))
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

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateModel()
        {
            MainModel mainModel = new MainModel("Message");
            return await Task.Run(() => Json(mainModel, JsonRequestBehavior.AllowGet));
        }

    }
}