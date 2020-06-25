
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFS.db.Tables;
using WFS.business.SessionSettings;
using System.Threading.Tasks;
using System.Web.Security;
using WFS.web.Models;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.Hosting;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web.UI.WebControls;
using WFS.web.Utilities;

namespace WFS.web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ClientRegister()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Confirm(string activationcode)
        {
            try
            {
                using (business.Management.UserManagement.UserFunctions um = new business.Management.UserManagement.UserFunctions())
                {
                    var user = um.getUserWtoken(activationcode);
                    ViewBag.UID = user.UserId;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return View();
        }
        [HttpGet] 
        public ActionResult Verify()
        {
            ViewBag.id = web.Session.SessionUser.User.User.UserId;
            return View();
        }
        [HttpPost]
        public JsonResult RegisterConfirm(int userId)
        {
            using (business.Management.UserManagement.UserFunctions userM = new business.Management.UserManagement.UserFunctions())
            {
                userM.verifieUser(userId);
                var msg = "Emailiniz onaylandı!";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult CustomerRegister(int wfs)
        { 
            Session["wfs"] = wfs;
            return View();
        }
        [HttpGet]
        public ActionResult PersonalRegister(long Id)
        {
            Session["personalFirmId"] = Id;
            return View();
        } 
        [HttpPost]
        public JsonResult userLogin(FormCollection form)
        {
            try
            {
                using (business.Management.UserManagement.UserFunctions userManagement = new business.Management.UserManagement.UserFunctions())
                {
                    var user = userManagement.GetUser(form["Email"].ToString(), form["Password"].ToString()).Result;
                    var clientManager = new ClientManager();
                    var personal = new Personal();
                     
                    if (user != null)
                    {
                        LoginedUser _user = null;
                        if (user.Role == "ClientManager") clientManager = userManagement.findClientManager(user.UserName, user.EncryptedPassword);
                        if (user.Role == "Personal" || user.Role == "Manager" || user.Role == "Admin") personal = userManagement.findPersonal(user.UserName, user.EncryptedPassword);


                        if (user.Role.Contains("ClientManager"))
                        {
                            _user = new LoginedUser { User = user, ClientManager_Id=clientManager.ClientManagerId, Firm_Id = clientManager.ManagerFirmId };
                            userManagement.updateLoginDate(clientManager.ClientManagerId, DateTime.Now);
                            FormsAuthentication.SetAuthCookie(Newtonsoft.Json.JsonConvert.SerializeObject(_user), false);
                        }
                        else
                        {
                            _user = new LoginedUser { User = user, Firm_Id = personal.OwnFirmId, Personal_Id = personal.PersonalId };
                            userManagement.updateLoginDateP(personal.PersonalId, DateTime.Now);
                            FormsAuthentication.SetAuthCookie(Newtonsoft.Json.JsonConvert.SerializeObject(_user), false);
                        }

                        //Services.ClearCache.clear();

                        return Json(new { result = true, message = "Başarı ile giriş yapıldı." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Kayıtlı kullanıcı bulunamadı." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = "Hata Oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> RL(string Email, string Password)
        {
            var EncPassword = business.SessionSettings.Crypting.En_De_crypt._Encrypt(Password);
            try
            {
                using (business.Management.Management.Login r = new business.Management.Management.Login())
                {
                    if (r.checkLogin(Email, EncPassword))
                    {
                        LoginedUser _root = null;
                        var root = r.GetRoot(Email, EncPassword);
                        _root = new LoginedUser { Root = await root, User = new User() };
                        r.updateLoginDate(root.Result.RootId, DateTime.Now);
                        FormsAuthentication.SetAuthCookie(Newtonsoft.Json.JsonConvert.SerializeObject(_root), false);

                        return await Task.Run(() => Json(new { result = true, message = "Giriş başarılı." }, JsonRequestBehavior.AllowGet));
                    }
                    else
                    {
                        return await Task.Run(() => Json(new { result = false, message = "Kayıtlı kullanıcı bulunamadı." }, JsonRequestBehavior.AllowGet));
                    }
                }
            }
            catch (Exception e)
            {
                return await Task.Run(() => Json(new { result = false, message = "Hata oluştu." }, JsonRequestBehavior.AllowGet));
            }
        }

        public async Task<ActionResult> Logout()
        {
            Services.ClearCache.clear();
            FormsAuthentication.SignOut();
            return await Task.Run(() => RedirectToAction("Index", "Home"));
        }

        [HttpPost]
        public JsonResult passwprd(string pasword)
        {
            PasswordRules pw = new PasswordRules();
            int dif = 0;
            string result = "";
            if (pw.GeneratePasswordScore(pasword) > 80)
            {
                result = "Şifreniz güçlü";
            }
            else if(pw.GeneratePasswordScore(pasword) >= 60)
            {
                result = "Şifreniz orta seviye";
            }
            else if(pw.GeneratePasswordScore(pasword) < 60)
            {
                result = "Şifreniz zayıf";
            }
            dif = pw.GeneratePasswordScore(pasword);

            return Json(new { data =dif,message =result   }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult firmRegister(string firmAd, string city, string state, string tel, string fax, string email, string url, string adress)
        {
            try
            {
                var logo = System.Web.HttpContext.Current.Request.Files[0];
                ImageProcess Ip = new ImageProcess();
                using (business.Management.FirmManagement.FirmFunctions firmManagement = new business.Management.FirmManagement.FirmFunctions())
                {
                    if (!firmManagement.isFirmExist(email))
                    {
                        string filename = null;
                        if (logo != null && (logo.ContentType == "image/jpeg" || logo.ContentType == "image/jpg" || logo.ContentType == "image/png"))
                        {
                            filename = Ip.Resolution(logo, new int[] { 128, 256, 512 }, firmAd, "FirmLogo");
                        }
                        else
                        {
                            filename = $"firm_default.jpeg";
                        } 

                        Firm newFirm = new Firm
                        {
                            Name = firmAd,
                            Mail = email,
                            Contact = tel,
                            Fax = fax,
                            City = city,
                            State = state,
                            Address = adress,
                            Url = url,
                            Register_Date = DateTime.Now,
                            Logo = filename,
                            Status = true
                        };
                        Session["latest"] = firmManagement.addFirm(newFirm); 


                        return Json(new { result = true, message = "Firma bilgileri başarılı bir şekilde kaydedildi." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Giriş yaptığınız firma bilgileri sistemde kayıtlı lütfen bilgilerinizi kontrol ediniz." }, JsonRequestBehavior.AllowGet);
                    }
                    
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Beklenmeyen bir hata oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult clientRegister(string Name, string Surname, string Emailc, string Password, string Contact)
        {
            try
            {
                var image = System.Web.HttpContext.Current.Request.Files[0];
                ImageProcess Ip = new ImageProcess(); 
                using (business.Management.UserManagement.UserFunctions userManagement = new business.Management.UserManagement.UserFunctions())
                {
                    PasswordRules pw = new PasswordRules();
                    int wfsNumber = Convert.ToInt32(Session["wfs"]);
                    string role;
                    bool IsUser;
                    
                    if (wfsNumber == 1)
                    {
                        role = "CustomerManager";
                        IsUser = false;
                    }
                    else
                    {
                        role = "ClientManager"; 
                        IsUser = true;
                    }

                    if (!userManagement.isManagerExist(Emailc))
                    {
                        if (pw.GeneratePasswordScore(Password) >= 60)
                        { 
                            string hashedPW = Crypting.En_De_crypt._Encrypt(Password);
                            string filename = null;
                            if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/jpg" || image.ContentType == "image/png"))
                            {
                                filename = Ip.Resolution(image, new int[] { 128, 256, 512 }, Emailc.Split('@')[0], "UserPicture");
                            }
                            else
                            {
                                filename = $"user_default.png";
                            }

                            ClientManager newManager = new ClientManager
                            {
                                managerUserId = userManagement.addUser(new db.Tables.User
                                {
                                    EncryptedPassword = hashedPW,
                                    Token = Guid.NewGuid().ToString(),
                                    Role = role,
                                    UserName = Emailc,
                                    Image = filename,
                                    EmailVeryfied = false
                                }),
                                Name = Name,
                                Surname = Surname,
                                Email = Emailc,
                                Password = hashedPW,
                                Contact = Contact,
                                Register_Date = DateTime.Now,
                                Login_Date = default(DateTime),
                                Status = true,
                                ManagerFirmId = (long)Session["latest"],
                                IsWFSuser = IsUser
                            }; 
                            userManagement.addClientManager(newManager); 

                            BuildEmailTemplate(newManager.managerUserId);

                            return Json(new { result = true, redirect = "Login", message = "Kaydınız başarılı bir şekilde oluşturuldu." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = false, message = "Şifreniz zayıf daha güçlü bir şifre giriniz." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        using (business.Management.FirmManagement.FirmFunctions fm = new business.Management.FirmManagement.FirmFunctions())
                        {
                            fm.deleteFirm((long)Session["latest"]);
                        }
                        return Json(new { message = "Giriş yaptığınız bilgiler sistemde kayıtlı lütfen bilgilerinizi kontrol ediniz." }, JsonRequestBehavior.AllowGet);
                    }
                  
                    
                }
            }
            catch (Exception e )
            {
                return Json(new { message = "Hata Oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult personalRegister(string Name, string Surname,string BirthDay, string Contact, string City, string State, string Address, string Email, string Password)
        {
            try
            {
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
                                OwnFirmId = Convert.ToInt64(Session["personalFirmId"]),
                                personalUserId = userManagement.addUser(new db.Tables.User
                                {
                                    EncryptedPassword = hashedPW,
                                    Token = Guid.NewGuid().ToString(),
                                    Role = "Personal",
                                    UserName = Email.ToString(),
                                    Image = filename,
                                    EmailVeryfied = false
                                }),
                                Name = Name.ToString(),
                                Surname = Surname.ToString(),
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

                            return Json(new { result = true, redirect = "Login", message = "Kaydınız başarılı bir şekilde oluşturuldu." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = false, message = "Şifreniz zayıf daha güçlü bir şifre giriniz." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { redirect = "Password", result = false, message = "Giriş yaptığınız mail sisteme kayıtlı." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { message = "Hata Oluştu." }, JsonRequestBehavior.AllowGet);
            }
        }

        public void BuildEmailTemplate(long id)
        {
            var userM = new business.Management.UserManagement.UserFunctions();
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = userM.getUser(id);
            var url = "http://localhost:50255/" + "User/Confirm?activationcode=" + regInfo.Token;

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
            
            if(!string.IsNullOrEmpty(bcc))
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