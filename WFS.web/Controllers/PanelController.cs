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
    public class PanelController : Controller
    {  
        [HttpGet]
        [Session.SessionControl()]
        public async Task<ActionResult> Index()
        { 
            MainModel mainModel = new MainModel("mainpage");
            return await Task.Run(() => View(mainModel)); 
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult PermissionError()
        {
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}