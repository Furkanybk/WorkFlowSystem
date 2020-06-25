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
    public class FirmController : Controller
    { 
        [HttpGet]
        public async Task<ActionResult> FirmInfo()
        { 
            MainModel mainModel = new MainModel("WFS");
            return await Task.Run(() => View(mainModel));
        }

        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
        [HttpGet] 
        public ActionResult EditFirm()
        {
            var id = web.Session.SessionUser.User.Firm_Id;
            MainModel mainModel = new MainModel("FindFirm", id);
            return View(mainModel);
        }

        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
        [HttpPost]
        public JsonResult UpdateFirm(string firmAd, string city, string state, string tel, string fax, string email, string url, string adress)
        {
            var user = web.Session.SessionUser.User;
            try
            {
                ImageProcess Ip = new ImageProcess();
                var image = System.Web.HttpContext.Current.Request.Files[0];
                using (business.Management.FirmManagement.FirmFunctions firmM = new business.Management.FirmManagement.FirmFunctions())
                {
                    string filename = null;
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/jpg" || image.ContentType == "image/png"))
                    {
                        filename = Ip.Resolution(image, new int[] { 128, 256}, firmAd, "FirmLogo");
                    }
                    else
                    {
                        filename = firmM.findFirm(user.Firm_Id).Logo;
                    }

                    Firm updateFirm = new Firm
                    {
                        Name = firmAd,
                        City = city,
                        State = state,
                        Contact = tel,
                        Fax = fax,
                        Mail = email,
                        Url = url,
                        Address = adress,
                        Logo = filename
                    };
                    if(firmM.updateFirm(updateFirm, user.Firm_Id))
                    { 
                        return Json(new { result = true, message = "Firma bilgileri başarılı bir şekilde güncellendi." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { 
                        return Json(new { result = false, message = "Firma bilgileri güncellenemedi." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Firma bilgileri güncellenemedi." }, JsonRequestBehavior.AllowGet);
                throw;
            } 
        }


        [Session.SessionControl(Role = new string[] { "Root" })]
        [HttpGet]
        public async Task<ActionResult> Firms()
        {
            RootModel rootModel = new RootModel("getFirms");
            return await Task.Run(() => View(rootModel));
        }
    }
}