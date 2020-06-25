using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WFS.web.Models;
using System.Web.Security;
using WFS.db.Tables;

namespace WFS.web.Controllers
{
    [Session.SessionControl(Role = new string[] { "Root" })]
    public class RecordController : Controller
    { 
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            RootModel rootModel = new RootModel("");
            return await Task.Run(() => View(rootModel));
        }
    }
}