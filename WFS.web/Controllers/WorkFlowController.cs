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
    public class WorkFlowController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> WorkFlowList()
        {
            MainModel mainModel = new MainModel("getWorkFlows");
            return await Task.Run(() => View("WorkFlowList", mainModel));
        }

        [HttpPost]
        public JsonResult workflowListPost(long departmentId)
        {
            try
            {
                using (business.Management.DepartmentManagement.DepartmentFunctions pm = new business.Management.DepartmentManagement.DepartmentFunctions())
                {
                    List<WorkFlow> workflowList = pm.findDepartment(departmentId).WorkFlows.ToList();

                    if (workflowList != null && !workflowList.Count.Equals(0))
                    {    
                       return Json(new { result = true, list = workflowList }, JsonRequestBehavior.AllowGet); 
                    }
                    else
                    {
                        return Json(new { result = false, message = "İş akışı oluşturulmamış." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult workflowListPostAll(long customerFirmId)
        {
            try
            {
                using (business.Management.PartnerManagement.PartnerFunctions pm = new business.Management.PartnerManagement.PartnerFunctions())
                {
                    List<WorkFlow> workflowList = pm.findPartner(customerFirmId).Client.ManagerFirm.Departments.SelectMany(q => q.WorkFlows).ToList();

                    if (workflowList != null && !workflowList.Count.Equals(0))
                    {
                        return Json(new { result = true, list = workflowList }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "İş akışı oluşturulmamış." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }

        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin","Manager" })]
        [HttpPost]
        public JsonResult depSelect(long customerFirmId)
        {
            try
            {
                using (business.Management.PartnerManagement.PartnerFunctions pm = new business.Management.PartnerManagement.PartnerFunctions())
                {
                    List<Department> depList = pm.findPartner(customerFirmId).Client.ManagerFirm.Departments.ToList();

                    if (depList != null && !depList.Count.Equals(0))
                    {
                        return Json(new { result = true, list = depList }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> CreateWorkFlow()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getPartners", user.Firm_Id);
            return await Task.Run(() => View(mainModel));
        }

        [HttpPost]
        public async Task<JsonResult> CreateWorkFlow(long depList, string Name, string Title, string Defination)
        {
            try
            {
                using (business.Management.WorkFlowManagement.WorkFlowFunctions wflosM = new business.Management.WorkFlowManagement.WorkFlowFunctions())
                {
                    WorkFlow newWorkFlow = new WorkFlow
                    {
                        Name = Name,
                        Title = Title,
                        Definition = Defination,
                        Register_Date = DateTime.Now, 
                        State = "New",
                        Status = true
                    };
                    if (wflosM.addWorkFlow(newWorkFlow, depList))
                    {
                        return await Task.Run(() => Json(new { result = true, message = "İş akışı eklendi."}, JsonRequestBehavior.AllowGet));
                    }
                    else
                    { 
                        return await Task.Run(() => Json(new { result = false, message = "İş akışı eklenemedi." }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            { 
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            }  
        }

        [HttpGet]
        public async Task<ActionResult> EditWorkFlow(long id)
        {
            MainModel mainModel = new MainModel("FindWorkFlow",id);
            return await Task.Run(() => View(mainModel));
        } 

        [HttpPost]
        public async Task<JsonResult> EditWorkFlow(long wfId, string Name, string Title, string Defination)
        {
            try
            {
                using (business.Management.WorkFlowManagement.WorkFlowFunctions wflosM = new business.Management.WorkFlowManagement.WorkFlowFunctions())
                { 
                    WorkFlow newWorkFlow = new WorkFlow
                    {
                        Name = Name,
                        Title = Title,
                        Definition = Defination,  
                        State = "Waiting" 
                    };
                    if (wflosM.updateWorkFlow(newWorkFlow, wfId))
                    {
                        return await Task.Run(() => Json(new { result = true, message = "İş akışı düzenlendi." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        ViewBag.message = "İş akışı güncellenemedi.";
                        return await Task.Run(() => Json(new { result = false, message = "İş akışı düzenlenemedi." }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteWorkFlow(long id)
        {
            try
            {
                using (business.Management.WorkFlowManagement.WorkFlowFunctions workflowM = new business.Management.WorkFlowManagement.WorkFlowFunctions())
                {
                    if (workflowM.deleteWorkFlow(id))
                    {
                        return await Task.Run(() => Json(new { result = true, message = "İş akışı silindi." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        return await Task.Run(() => Json(new { result = false, message = "İş akışı silinemedi." }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            }
        }
    }
}