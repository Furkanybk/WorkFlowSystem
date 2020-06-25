using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WFS.business.SessionSettings;
using WFS.db.Tables;
using WFS.web.Models;
using WFS.web.Utilities;

namespace WFS.web.Controllers
{
    [Session.SessionControl(Role = new string[] { "ClientManager", "Admin" })]
    public class PersonalController : Controller
    { 
        [HttpGet]
        public async Task<ActionResult> PersonalList()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getPersonals",user.Firm_Id);
            return await Task.Run(() => View(mainModel));
        }
        [HttpGet]
        public async Task<ActionResult> CreatePersonal()
        {
            MainModel mainModel = new MainModel("");
            return await Task.Run(() => View(mainModel));
        }
         
        [HttpPost]
        public JsonResult CreatePersonal(string Name, string Surname, string PRole, string BirthDay, string Contact, string City, string State, string Address, string Email, string Password, string options)
        {
            try
            {
                var user = web.Session.SessionUser.User;
                var image = System.Web.HttpContext.Current.Request.Files[0];
                using (business.Management.UserManagement.UserFunctions userManagement = new business.Management.UserManagement.UserFunctions())
                {
                    PasswordRules pw = new PasswordRules();
                    ImageProcess Ip = new ImageProcess();
                    if (!userManagement.isPersonalExist(Email))
                    {
                        if (pw.GeneratePasswordScore(Password) >= 60)
                        {
                            string hashedPW = Crypting.En_De_crypt._Encrypt(Password);
                            string filename = null;
                            if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/jpg" || image.ContentType == "image/png"))
                            {
                                filename = Ip.Resolution(image, new int[] { 128, 256, 512 }, Email.Split('@')[0], "UserPicture"); 
                            }
                            else
                            {
                                filename = $"user_default.png";
                            }

                            Personal newPersonal = new Personal
                            {
                                OwnFirmId = user.Firm_Id,
                                personalUserId = userManagement.addUser(new db.Tables.User
                                {
                                    EncryptedPassword = hashedPW,
                                    Token = Guid.NewGuid().ToString(),
                                    Role = options.ToString(),
                                    UserName = Email.ToString(),
                                    Image = filename,
                                    EmailVeryfied = false
                                }),
                                Name = Name.ToString(),
                                Surname = Surname.ToString(),
                                PRole = PRole.ToString(),
                                BirthDay = BirthDay.ToString(),
                                Contact = Contact.ToString(),
                                City = City.ToString(),
                                State = State.ToString(),
                                Address = Address.ToString(),
                                Mail = Email.ToString(),
                                Password = hashedPW,
                                Register_Date = DateTime.Now,
                                Login_Date = default(DateTime),
                                Status = true
                            };
                            userManagement.addPersonal(newPersonal);
                            BuildEmailTemplate(newPersonal.personalUserId);

                            return Json(new { result = true, redirect = "PersonalList", message = "Personal kaydı başarılı şekilde oluşturuldu." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = false, message = "Şifre zayıf, lütfen daha güçlü bir şifre yazın." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if(Email == "")
                        {
                            return Json(new { result = false, message = "Bilgileri eksiksiz giriniz." }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = "Personal emaili kullanımda." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { message = "Hata Oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<ActionResult> EditPersonal(long id)
        {
            MainModel mainModel = new MainModel("FindPersonal", id);
            return await Task.Run(() => View(mainModel));
        }
        [HttpPost]
        public async Task<JsonResult> EditPersonal(long personalId, string Name, string Surname, string PRole, string Birthday, string Mail, string Password, string Contact, string City, string State, string Address)
        { 
            try
            {
                var image = System.Web.HttpContext.Current.Request.Files[0];
                ImageProcess Ip = new ImageProcess();
                using (business.Management.UserManagement.UserFunctions um = new business.Management.UserManagement.UserFunctions())
                {
                    var thisPersonal = um.findPersonal(personalId);
                    var lel = true;

                    if (um.isPersonalExist(Mail) && thisPersonal.Mail != Mail)
                    { 
                        return await Task.Run(() => Json(new { result = false, message = "Email kullanımda lütfen başka bir email deneyiniz." }, JsonRequestBehavior.AllowGet));
                    }
                    if(thisPersonal.Mail != Mail)
                    {
                        lel = false;
                    }
                    string filename = null;
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/jpg" || image.ContentType == "image/png"))
                    {
                        filename = Ip.Resolution(image, new int[] { 128, 256, 512 }, Mail.Split('@')[0], "UserPicture");
                    } 
                    else if(thisPersonal.personalUser.Image != null)
                    {
                        filename = thisPersonal.personalUser.Image;
                    }
                    else
                    {
                        filename = $"user_default.png";
                    }
                    long personalUserId = um.findUser(personalId);
                    var pass = Crypting.En_De_crypt._Encrypt(Password); 

                    User updateuser = new User
                    {
                        UserName = Mail,
                        EncryptedPassword = pass,
                        Image = filename,
                        EmailVeryfied = lel
                    };

                    Personal personal = new Personal
                    {
                        Name = Name,
                        Surname = Surname,
                        PRole = PRole,
                        BirthDay = Birthday,
                        Mail = Mail,
                        Password = pass,
                        Contact = Contact,
                        City = City,
                        State = State,
                        Address = Address
                    };
                    
                    if(um.updateUser(updateuser, personalUserId))
                    {
                        if(um.updatePersonal(personal, personalId))
                        {
                            if (lel == false) BuildEmailTemplate(thisPersonal.personalUserId);
                            return await Task.Run(() => Json(new { result = true, message = "Personal bilgileri güncellendi." }, JsonRequestBehavior.AllowGet));
                        }
                        else
                        {
                            return await Task.Run(() => Json(new { result = false, message =  "Personal bilgileri güncellenemedi lütfen aynı bilgiler ile tekrar deneyiniz." }, JsonRequestBehavior.AllowGet));
                        }
                    }
                    else
                    { 
                        return await Task.Run(() => Json(new { result = false, message = "Personal bilgileri güncellenemedi." }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu" }, JsonRequestBehavior.AllowGet));
            } 
        }

        [HttpPost]
        public async Task<JsonResult> DeletePersonal(long pId)
        {
            try
            {
                using (business.Management.UserManagement.UserFunctions um = new business.Management.UserManagement.UserFunctions())
                {
                    if (um.deletePersonal(pId))
                    {
                        return await Task.Run(() => Json(new { result = true, message = "Personel silindi." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        return await Task.Run(() => Json(new { result = false, message = "Personel silinemedi." }, JsonRequestBehavior.AllowGet));
                    } 
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet)); 
            }
        }

        [Session.SessionControl(Role = new string[] { "ClientManager", "Admin","Manager","Personal" })]
        [HttpGet]
        public async Task<ActionResult> Works()
        {
            var user = web.Session.SessionUser.User;
            MainModel mainModel = new MainModel("getOwnWorks",user.Personal_Id);
            return await Task.Run(() => View(mainModel));

        }

        public void BuildEmailTemplate(long id)
        {
            var userM = new business.Management.UserManagement.UserFunctions();
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = userM.getUser(id);
            var url = "http://localhost:50525/" + "User/Confirm?activationcode=" + regInfo.Token;

            body = body.Replace("ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Hesabınız Başarı ile oluşturuldu.", body, regInfo.UserName);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "wfs@sevalinmutfagi.com";
            to = sendTo;
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));

            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        private static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "mail.sevalinmutfagi.com";
            client.Port = 587;
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("wfs@sevalinmutfagi.com", "20Airbus525");

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}