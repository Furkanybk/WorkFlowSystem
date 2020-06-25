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
    [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
    public class WorkController : Controller
    {
        [Session.SessionControl()]
        [HttpGet]
        public async Task<ActionResult> WorkList()
        {
            MainModel mainModel = new MainModel("getWorks");
            return await Task.Run(() => View(mainModel));
        } 
        [HttpPost]
        public JsonResult workListPost(long workflowId)
        {
            try
            {
                using (business.Management.WorkFlowManagement.WorkFlowFunctions wfm = new business.Management.WorkFlowManagement.WorkFlowFunctions())
                {
                    List<Work> workList = wfm.findWorkFlow(workflowId).Works.ToList();

                    if (workList != null && !workList.Count.Equals(0))
                    {
                        return Json(new { result = true, list3 = workList }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "İş oluşturulmamış." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet);
            }
        } 
        [HttpPost]
        public JsonResult wflowSelect(long depId)
        {
            try
            {
                using (business.Management.DepartmentManagement.DepartmentFunctions dm = new business.Management.DepartmentManagement.DepartmentFunctions())
                {
                    List<WorkFlow> wflowList = dm.findDepartment(depId).WorkFlows.ToList();

                    if (wflowList != null && !wflowList.Count.Equals(0))
                    {
                        return Json(new { result = true, list2 = wflowList }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        } 
        [HttpGet]
        public async Task<ActionResult> CreateWork()
        { 
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getPersonals",user.Firm_Id);
            return await Task.Run(() => View(mainModel));
        } 
        [HttpGet]
        public async Task<ActionResult> EditWork(long id)
        {
            MainModel mainModel = new MainModel("FindWork",id);
            return await Task.Run(() => View(mainModel));
        } 
        [HttpPost]
        public async Task<ActionResult> EditWork(long wId, string Name, string Title, string Defination)
        {
            try
            {
                using (business.Management.WorkManagement.WorkFunctions wM = new business.Management.WorkManagement.WorkFunctions())
                {
                    Work newWork = new Work
                    {
                        Name = Name,
                        Title = Title,
                        Definition = Defination,
                        State = "Çalışılıyor"
                    };
                    if (wM.updateWork(newWork, wId))
                    {
                        return await Task.Run(() => RedirectToAction("WorkList"));
                    }
                    else
                    {
                        ViewBag.message = "İş akışı güncellenemedi.";
                        return await Task.Run(() => RedirectToAction("WorkList"));
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        } 
        [HttpGet]
        public async Task<ActionResult> DeleteWork(long id)
        {
            try
            {
                using (business.Management.WorkManagement.WorkFunctions workM = new business.Management.WorkManagement.WorkFunctions())
                {
                    if (workM.deleteWork(id))
                    {
                        ViewBag.message = "İş başarı ile silindi.";
                    }
                    else { ViewBag.message = "İş silinemedi."; }

                    return await Task.Run(() => RedirectToAction("WorkList"));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}