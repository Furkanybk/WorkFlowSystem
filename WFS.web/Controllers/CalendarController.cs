using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WFS.web.Models;

namespace WFS.web.Controllers
{
    public class CalendarController : Controller
    {
        [Session.SessionControl()]
        public async Task<ActionResult> Index()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("");
            return await Task.Run(() => View(mainModel));
        }
    }
}