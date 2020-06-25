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
    public class SettingsController : Controller
    {  
        [HttpGet]
        public async Task<ActionResult> Index()
        { 
            MainModel mainModel = new MainModel("");
            return await Task.Run(() => View(mainModel)); 
        } 
    }
}