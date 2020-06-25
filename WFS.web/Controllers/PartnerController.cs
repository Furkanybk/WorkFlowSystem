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
    public class PartnerController : Controller
    {
        [HttpGet]
        [Session.SessionControl(Role = new string[] {"ClientManager","Admin"})]
        public async Task<ActionResult> PartnerList()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getPartners", user.Firm_Id);
            return await Task.Run(() => View(mainModel));
        }
         
        [HttpGet]
        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
        public async Task<ActionResult> CreatePartner()
        {
            MainModel mainModel = new MainModel("getFirms");
            return await Task.Run(() => View(mainModel));
        } 
        [HttpGet]
        public async Task<ActionResult> PartnerProfile(long id)
        {
            MainModel mainModel = new MainModel("FindPartner",id);
            return await Task.Run(() => View("PartnerProfile", mainModel));
        }

        [HttpPost]
        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
        public async Task<JsonResult> DeletePartner(long pId)
        { 
            try
            {
                using (business.Management.PartnerManagement.PartnerFunctions partnerM = new business.Management.PartnerManagement.PartnerFunctions())
                {
                    if(partnerM.deletePartner(pId))
                    {
                        return await Task.Run(() => Json(new { result = true, message = "Partner silindi."  }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        return await Task.Run(() => Json(new { result = false, message = "Partner silinemedi." }, JsonRequestBehavior.AllowGet));
                    } 
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet)); 
            }
        }

        [HttpPost]
        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
        public async Task<JsonResult> CreatePartner(long firmList)
        {
            var user = WFS.web.Session.SessionUser.User;
            var fm = new business.Management.FirmManagement.FirmFunctions();
            try
            {
                using (business.Management.PartnerManagement.PartnerFunctions partnerM = new business.Management.PartnerManagement.PartnerFunctions())
                { 
                    var selectedManager = partnerM.selectManager(firmList);
                    if(partnerM.isPartnerExist(selectedManager.ManagerFirmId, user.Firm_Id))
                    {
                        CustomerFirmManager newcfManager = new CustomerFirmManager
                        {
                            ClientId = selectedManager.ClientManagerId,
                            Token = Guid.NewGuid().ToString()
                        };
                        fm.addCustomerFirm(user.Firm_Id, newcfManager);
                        return await Task.Run(() => Json(new { result = true, message = "Partner eklendi." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    { 
                        return await Task.Run(() => Json(new { result = false, message = "Partner zaten kayıtlı." }, JsonRequestBehavior.AllowGet));
                    } 
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            } 
        }
         
        [HttpPost]
        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
        public JsonResult selectFirm(long firmId)
        {
            using (business.Management.FirmManagement.FirmFunctions ff = new business.Management.FirmManagement.FirmFunctions())
            {
                var selectedFirm = ff.selectFirm(firmId);
                var pm = new business.Management.PartnerManagement.PartnerFunctions();
                var selectedManager = pm.selectManager(selectedFirm.FirmId);

                if (selectedFirm != null)
                {
                    return Json(new { result = true, firm = selectedFirm, manager = selectedManager }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
            }
        } 
    }
}