using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WFS.db.Tables;
using WFS.db.WFScontext;
using WFS.web.Models;
using WFS.web.Utilities;

namespace WFS.web.Controllers
{
    [Session.SessionControl()]
    public class ActiveController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> WFS()
        { 
            MainModel mainModel = new MainModel("WFS");
            return await Task.Run(() => View(mainModel));
        }

        [HttpGet]
        public async Task<ActionResult> WFS_Deps(long id)
        {
            MainModel mainModel = new MainModel("WFS", id);
            return await Task.Run(() => View(mainModel));
        }

        [HttpGet]
        public async Task<ActionResult> WFS_WFlows(long id, long did)
        {
            MainModel mainModel = new MainModel("WFS", id,did);
            return await Task.Run(() => View(mainModel));
        }

        [HttpGet]
        public async Task<ActionResult> WFS_Works(long id, long did, long wfid)
        {
            MainModel mainModel = new MainModel("WFS", id, did, wfid);
            return await Task.Run(() => View(mainModel));
        }
        [HttpGet]
        public async Task<ActionResult> Work_Detail(long id)
        {
            MainModel mainModel = new MainModel("workDetail", id);
            return await Task.Run(() => View(mainModel));
        }

        [Session.SessionControl(Role = new string[] { "ClientManager","Admin","Manager" })]
        [HttpGet]
        public async Task<ActionResult> AddWork()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getPersonals",user.Firm_Id);
            return await Task.Run(() => View(mainModel));
        }

        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin", "Manager" })]
        [HttpPost]
        public async Task<JsonResult> AddWork(long wfss, string Name, string Title, string Defination, DateTime expectedDate, List<long> personels, string options)
        {
            bool priority = false;
            var state = options;
            if (state == "Normal") state = "Yeni";
            else
            {
                priority = true;
            }
            var pm = new business.Management.UserManagement.UserFunctions(); 
            var pList = new List<Personal>();

            ImageProcess Ip = new ImageProcess();
              
            foreach (var item in personels)
            {
                pList.Add(pm.findPersonal(item));
            }
            try
            { 
                using (business.Management.WorkManagement.WorkFunctions wm = new business.Management.WorkManagement.WorkFunctions())
                {
                    if (wm.checkWorkName(wfss,Name))
                    {
                        return await Task.Run(() => Json(new { result = false, message = "Kayıtlı bir iş adı girdiniz, lütfen farklı bir iş adı giriniz." }, JsonRequestBehavior.AllowGet));
                    }
                    Work newWork = new Work
                    {
                        Name = Name,
                        Title = Title,
                        Definition = Defination,
                        Expected_Date = expectedDate,
                        Register_Date = DateTime.Now, 
                        State = state,
                        Priority = priority,
                        Status = true,
                        ProgressBar = 0.0f
                    };
                    newWork.UploadFiles = new List<Files>();

                    var postedfiles = System.Web.HttpContext.Current.Request.Files; 

                    string furl,fname;
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        var unique = Guid.NewGuid().ToString();
                        fname = postedfiles[i].FileName;

                        if (postedfiles[i] != null && (postedfiles[i].ContentType == "image/jpeg" || postedfiles[i].ContentType == "image/jpg" || postedfiles[i].ContentType == "image/png"))
                        { 
                            var mainFolder = Server.MapPath($"~/Images/WorkPics/" + Name);
                            if (!Directory.Exists(mainFolder))
                            {
                                Directory.CreateDirectory(mainFolder);
                            } 

                            furl = Ip.Resolution(postedfiles[i], new int[] { 256, 1024 }, fname, "WorkPics/"+ Name);

                            Files newImage = new Files
                            {
                                fileName = fname,
                                fileUrl = furl
                            };

                            newWork.UploadFiles.Add(newImage);
                        }   
                    }

                    if(wm.addWork(newWork,wfss,personels))
                    { 
                        return await Task.Run(() => Json(new { result = true, message = "İş oluşturuldu." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    { 
                        return await Task.Run(() => Json(new { result = false, message = "İş oluşturulamadı." }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            }  
        }

        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
        [HttpPost]
        public JsonResult Work_Delete(long id)
        {
            try
            {
                using (business.Management.WorkManagement.WorkFunctions wm = new business.Management.WorkManagement.WorkFunctions())
                {
                    if (wm.deleteWork(id))
                    {
                        return Json(new { result = true, message = "İş silindi." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "İş silinemedi." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }
         
        [HttpPost]
        public JsonResult CreateWorkPiece(long wId, long pId, string Name, string Title, string Definition)
        {
            try
            {
                using (business.Management.WorkManagement.WorkFunctions wm = new business.Management.WorkManagement.WorkFunctions())
                {
                    WorkList newWPiece = new WorkList
                    {
                        Name = Name,
                        Title = Title,
                        Definition = Definition,
                        Register_Date = DateTime.Now,
                        State = "Çalışılıyor",
                        Status = true,
                        WLpersonalId = pId
                    };
                    wm.addWorkPiece(newWPiece,wId); 
                    return Json(new { result = true}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            { 
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CompleteWorkPiece(long Id, long wId)
        {
            try
            {
                using (business.Management.WorkManagement.WorkFunctions wm = new business.Management.WorkManagement.WorkFunctions())
                {
                    wm.completeWorkList(Id, wId); 
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }
         
        [HttpPost]
        public JsonResult DeleteWorkPiece(long wId)
        {
            try
            {
                using (business.Management.WorkManagement.WorkFunctions wm = new business.Management.WorkManagement.WorkFunctions())
                {
                    wm.deleteWorkList(wId);
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}