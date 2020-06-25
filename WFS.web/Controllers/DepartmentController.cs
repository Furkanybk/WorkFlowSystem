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
    [Session.SessionControl(Role = new string[] { "ClientManager", "Admin"})]
    public class DepartmentController : Controller
    { 
        [HttpGet]
        public async Task<ActionResult> DepartmentList()
        {
            MainModel mainModel = new MainModel("getDepartments");
            return await Task.Run(() => View("DepartmentList", mainModel));
        }

        [HttpPost]
        public JsonResult departmentListPost(long customerFirmId)
        {
            try
            {
                using (business.Management.PartnerManagement.PartnerFunctions pm = new business.Management.PartnerManagement.PartnerFunctions())
                {
                    List<Department> depList = pm.findPartner(customerFirmId).Client.ManagerFirm.Departments.ToList();

                    if(depList != null && !depList.Count.Equals(0))
                    {
                        return Json(new { result = true, list = depList }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Departman oluşturulmamış." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet);
            } 
        }

        [HttpGet]
        public async Task<ActionResult> CreateDepartment()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getPartners", user.Firm_Id);
            return await Task.Run(() => View("CreateDepartment", mainModel));
        }
        [HttpPost]
        public async Task<JsonResult> CreateDepartment(long partnerId, string Name, string Title, string Defination)
        {
            try
            {
                using (business.Management.DepartmentManagement.DepartmentFunctions depM = new business.Management.DepartmentManagement.DepartmentFunctions())
                {
                    Department newDep = new Department
                    {
                        Name = Name,
                        Title = Title,
                        Definition = Defination,
                        Register_Date = DateTime.Now,
                        Status = true
                    };
                    depM.addDepartment(newDep, partnerId);

                    return await Task.Run(() => Json(new { result = true, message = "Departman oluşturuldu."}, JsonRequestBehavior.AllowGet));
                }
            }
            catch (Exception)
            { 
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            }

        }
        [HttpGet]
        public async Task<ActionResult> EditDepartment(long id)
        {
            MainModel mainModel = new MainModel("FindDepartment", id);
            return await Task.Run(() => View("EditDepartment", mainModel));
        }
        [HttpPost]
        public async Task<JsonResult> EditDepartment(long depId, string Name, string Title, string Defination)
        {
            try
            {
                using (business.Management.DepartmentManagement.DepartmentFunctions departmentM = new business.Management.DepartmentManagement.DepartmentFunctions())
                {
                    Department newDepartment = new Department
                    {
                        Name = Name,
                        Title = Title,
                        Definition = Defination
                    };
                    if(departmentM.updateDepartment(newDepartment, depId))
                    { 
                        return await Task.Run(() => Json(new { result = true, message = "Departman güncellendi" }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    { 
                        return await Task.Run(() => Json(new { result = false, message = "Departman güncellenemedi." }, JsonRequestBehavior.AllowGet));
                    } 
                }
            }
            catch (Exception)
            { 
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            }
        }
        [HttpPost]
        public async Task<JsonResult> DeleteDepartment(long id)
        {
            try
            {
                using (business.Management.DepartmentManagement.DepartmentFunctions departmentM = new business.Management.DepartmentManagement.DepartmentFunctions())
                {
                    if (departmentM.deleteDepartment(id))
                    { 
                        return await Task.Run(() => Json(new { result = true, message = "Departman silindi." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    { 
                        return await Task.Run(() => Json(new { result = false, message = "Departman silinemedi." }, JsonRequestBehavior.AllowGet));
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