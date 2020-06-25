using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WFS.db.Tables;
using WFS.web.Models;
using WFS.web.Utilities;

namespace WFS.web.Controllers
{
    [Session.SessionControl()]
    public class testController : Controller
    { 
        public ActionResult Index()
        {
            MainModel n = new MainModel(""); 
            return View(n);
        }

        [HttpPost]
        public JsonResult addImage()
        {
            try
            {
                var image = System.Web.HttpContext.Current.Request.Files[0];

                ImageProcess Ip = new ImageProcess();
                Ip.Resolution(image, new int[] { 128, 256, 512 }, "test", "UserPicture");
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        #region FirstCode
        //[HttpPost]
        //public async Task<ActionResult> insertRoot(FormCollection form)
        //{
        //    using (business.Management.Management.RootManagement rootmanagement = new business.Management.Management.RootManagement())
        //    {
        //        Root addRoot = new Root
        //        {
        //            Login_Date = default(DateTime),
        //            Name = form["Name"].ToString(),
        //            Password = form["Password"].ToString(),
        //            Register_Date = DateTime.Now,
        //            Status = Convert.ToBoolean(form["Status"].ToString().Replace("on", "true").Replace("off", "false")),
        //            Surname = form["Surname"].ToString(),
        //            Username = form["Username"].ToString()
        //        };
        //        rootmanagement.addRoot(addRoot);
        //    }
        //    return await Task.Run(() => View("Insert"));//return await Task.Run(()=>RedirectoAction("Oturumac"));//return await Task.Run(()=>RedirectoAction("Oturumac","Login"));
        //}
        //data: $("#formRootAdd").Serialize()
        //[HttpPost]
        //public JsonResult _insertRoot(FormCollection form)
        //{
        //    try
        //    {
        //        using (business.Management.Management.RootManagement rootmanagement = new business.Management.Management.RootManagement())
        //        {
        //            Root addRoot = new Root
        //            {
        //                Login_Date = default(DateTime),
        //                Name = form["Name"].ToString(),
        //                Password = form["Password"].ToString(),
        //                Register_Date = DateTime.Now,
        //                Status = Convert.ToBoolean(form["Status"].ToString().Replace("on", "true").Replace("off", "false")),
        //                Surname = form["Surname"].ToString(),
        //                Username = form["Username"].ToString()
        //            };
        //            rootmanagement.addRoot(addRoot);
        //        }
        //        return Json(new { state = true, message = "Kayıt eklendi" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { state = false, message = "Kayıt eklenirken hata oluştu" }, JsonRequestBehavior.AllowGet);

        //    } //return await Task.Run(()=>RedirectoAction("Oturumac"));//return await Task.Run(()=>RedirectoAction("Oturumac","Login"));
        //}
        //[HttpPost]
        //public JsonResult _insertRoot(string Name, string Password, string Status, string Surname, string Username)
        //{
        //    try
        //    {
        //        using (business.Management.Management.RootManagement rootmanagement = new business.Management.Management.RootManagement())
        //        {
        //            Root addRoot = new Root
        //            {
        //                Login_Date = default(DateTime),
        //                Name = Name,
        //                Password = Password,
        //                Register_Date = DateTime.Now,
        //                Status = Convert.ToBoolean(Status.Replace("on", "true").Replace("off", "false")),
        //                Surname = Surname,
        //                Username = Username
        //            };
        //            rootmanagement.addRoot(addRoot);
        //        }
        //        return Json(new { state = true, message = "Kayıt eklendi" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { state = false, message = "Kayıt eklenirken hata oluştu" }, JsonRequestBehavior.AllowGet);

        //    } //return await Task.Run(()=>RedirectoAction("Oturumac"));//return await Task.Run(()=>RedirectoAction("Oturumac","Login"));
        //}
        //[HttpGet]
        //public async Task<ActionResult> RootAddN()
        //{
        //    return await Task.Run(() => View("Insert"));
        //}
        //[HttpPost]
        //public async Task<ActionResult> _insertRoot2(string Name, string Password, string Status, string Surname, string Username)
        //{
        //    try
        //    {
        //        using (business.Management.Management.RootManagement rootmanagement = new business.Management.Management.RootManagement())
        //        {
        //            Root addRoot = new Root
        //            {
        //                Login_Date = default(DateTime),
        //                Name = Name,
        //                Password = Password,
        //                Register_Date = DateTime.Now,
        //                Status = Convert.ToBoolean(Status.Replace("on", "true").Replace("off", "false")),
        //                Surname = Surname,
        //                Username = Username
        //            };
        //            rootmanagement.addRoot(addRoot);
        //        }
        //        return await Task.Run(() => View("Insert"));
        //    }
        //    catch (Exception)
        //    {
        //        return await Task.Run(() => View("Index"));

        //    } //return await Task.Run(()=>RedirectoAction("Oturumac"));//return await Task.Run(()=>RedirectoAction("Oturumac","Login"));
        //}
        //[HttpGet]
        //public async Task<ActionResult> Index()
        //{
        //    try
        //    {

        //        using (wfs.db.wfscontext.cfgcontext db = new wfs.db.wfscontext.cfgcontext())
        //        {

        //            var date = datetime.now;
        //            db.configuration.lazyloadingenabled = false;
        //            for (int i = 0; i < 1000; db.root.count())
        //            {
        //                db.root.add(new wfs.db.tables.root
        //                {
        //                    login_date = datetime.now,
        //                    name = "",
        //                    password = "",
        //                    register_date = datetime.now,
        //                    status = true,
        //                    surname = guid.newguid().tostring(),
        //                    username = ""
        //                }); break;
        //            }
        //            foreach (var item in db.root)
        //            {

        //            }
        //            db.savechanges();
        //            var edate = datetime.now;

        //        }
        //        return await task.run(() => view(new models.MainModel("root")));
        //    }
        //    catch (exception ex)
        //    {
        //        return null;
        //    }
        //} 
        #endregion
    }
}