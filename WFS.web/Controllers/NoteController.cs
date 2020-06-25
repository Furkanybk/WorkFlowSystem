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
    public class NoteController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Note()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getNotes",user.User.UserId);
            return await Task.Run(() => View(mainModel));
        } 

        [HttpPost]
        public JsonResult DetailNote()
        {
            MainModel mainModel = new MainModel("");
            return Json(new { result = true, message = "Başarı ile giriş yapıldı." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddNote(long Id, string Title, string txt)
        {
            var user = web.Session.SessionUser.User;

            try
            {
                using (business.Management.NoteManagement.NoteFunctions nm = new business.Management.NoteManagement.NoteFunctions())
                {
                    Note newNote = new Note
                    {
                        Title = Title, 
                        NoteTxt = txt,
                        Register_Date = DateTime.Now
                    };
                    
                    
                    if(user.User.Role != "ClientManager")
                    { 
                        nm.addNote(newNote, Id, "Personal");
                    }
                    else
                    {
                        nm.addNote(newNote, Id, "ClientManager");
                    }

                    return Json(new { result = true, message = "Not eklendi." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Not eklenemedi." }, JsonRequestBehavior.AllowGet);
            }
        }
         
        [HttpGet]
        public JsonResult EditNote(long Id)
        {
            try
            {
                using (business.Management.NoteManagement.NoteFunctions nm = new business.Management.NoteManagement.NoteFunctions())
                {
                    var note = nm.findNote(Id);

                    if(note != null)
                    {
                        return Json(new { result = true, noteM = note }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Note bulunamadı." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            { 
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            } 
        }

        [HttpPost]
        public JsonResult EditNoteP(long NId, string Title, string Priority, string txt)
        {
            try
            {
                using (business.Management.NoteManagement.NoteFunctions nm = new business.Management.NoteManagement.NoteFunctions())
                {
                    Note newNote = new Note
                    {
                        Title = Title, 
                        NoteTxt = txt,
                        Update_Date = DateTime.Now
                    };
                    nm.updateNote(newNote, NId);

                    return Json(new { result = true, message = "Not düzenlendi." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Not düzenlenemedi." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteNote(long id)
        {
            try
            {
                using (business.Management.NoteManagement.NoteFunctions nm = new business.Management.NoteManagement.NoteFunctions())
                {
                    if (nm.deleteNote(id))
                    {  
                        return Json(new { result = true, message = "Note silindi." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {  
                        return Json(new { result = false, message = "Note silinemedi." }, JsonRequestBehavior.AllowGet);
                    }
                     
                }
            }
            catch (Exception)
            { 
                return Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}