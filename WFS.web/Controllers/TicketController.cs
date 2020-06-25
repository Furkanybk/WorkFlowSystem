using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WFS.db.Tables;
using WFS.web.Models;

namespace WFS.web.Controllers
{
    [Session.SessionControl()]
    public class TicketController : Controller
    {
        // GET: Ticket
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            MainModel mainModel = new MainModel("");
            return await Task.Run(() => View(mainModel));
        } 
        [HttpPost]
        public JsonResult AddTicket(string ticketTitle, string ticketMsg)
        {
            var user = web.Session.SessionUser.User.User;

            try
            {
                using (business.Management.TicketManagement.TicketFunctions tm = new business.Management.TicketManagement.TicketFunctions())
                {
                    Ticket newTicket = new Ticket
                    { 
                        Title = ticketTitle,
                        Message = ticketMsg, 
                        SenderName = user.UserName,
                        postTime = DateTime.Now
                    }; 
                    if(tm.addTicket(newTicket))
                    { 
                        return Json(new { result = true, message = "Destek talebiniz alınmıştır. Gelen kutunuzu kontrol etmeyi unutmayınız." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Destek talebi alınamadı..." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            { 
                return Json(new { result = false, message = "Destek talebi alınamadı..." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}