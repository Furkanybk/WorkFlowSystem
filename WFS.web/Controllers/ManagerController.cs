using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WFS.business.SessionSettings;
using WFS.db.Tables;
using WFS.web.Models;
using WFS.web.Utilities;

namespace WFS.web.Controllers
{
    [Session.SessionControl()]
    public class ManagerController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Profile()
        { 
            MainModel mainModel = new MainModel("");
            return await Task.Run(() => View(mainModel));
        }

        [HttpPost]
        public async Task<JsonResult> ProfileUpdate(string Name, string Surname, string Contact, string Email, string Password)
        { 
            var user = web.Session.SessionUser.User;
            ImageProcess Ip = new ImageProcess();

            try
            {
                var image = System.Web.HttpContext.Current.Request.Files[0];

                string filename = null;
                if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/jpg" || image.ContentType == "image/png"))
                { 
                    filename = Ip.Resolution(image, new int[] { 128,256,512 }, user.User.UserName.Split('@')[0],"UserPicture");
                }
                else
                { 
                    filename = user.User.Image;
                }

                using (business.Management.UserManagement.UserFunctions userM = new business.Management.UserManagement.UserFunctions())
                {
                    PasswordRules pw = new PasswordRules();
                    string hashedPW = Password;

                    if (pw.GeneratePasswordScore(Password) >= 60)
                    { 
                        if (Password != user.User.EncryptedPassword)
                        { 
                            hashedPW = business.SessionSettings.Crypting.En_De_crypt._Encrypt(Password);
                        }
                        bool verify = true;

                        if (Email != user.User.UserName)
                        {
                            if (user.User.Role == "ClientManager")
                            {
                               if (userM.isManagerExist(Email))
                               {
                                    return Json(new { result = false, message = "Girdiğiniz bilgiler sistemde kayıtlı..." }, JsonRequestBehavior.AllowGet);
                               }
                            }
                            else
                            {
                                if(userM.isPersonalExist(Email))
                                {
                                    return Json(new { result = false, message = "Girdiğiniz bilgiler sistemde kayıtlı..." }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            BuildEmailTemplate(user.User.UserId);
                            verify = false;
                        }

                        User updateuser = new User
                        {
                            UserName = Email,
                            EncryptedPassword = hashedPW,
                            Image = filename,
                            EmailVeryfied = verify
                        }; 

                        if (userM.updateUser(updateuser, user.User.UserId))
                        {
                            if (user.User.Role.Contains("ClientManager"))
                            {
                                ClientManager updateManager = new ClientManager
                                {
                                    Name = Name,
                                    Surname = Surname,
                                    Email = Email,
                                    Password = hashedPW,
                                    Contact = Contact,
                                };
                                if (userM.updateClientManager(updateManager, user.ClientManager_Id))
                                { 
                                    return Json(new { result = true, message = "Bilgileriniz güncellendi." }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { result = false, message = "Bilgileriniz güncellenemedi." }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                Personal updatePersonal = new Personal
                                {
                                    Name = Name,
                                    Surname = Surname,
                                    Contact = Contact,
                                    Mail = Email,
                                    Password = hashedPW
                                };
                                if (userM.updatePersonal(updatePersonal, user.Personal_Id))
                                { 
                                    return Json(new { result = true, message = "Bilgileriniz güncellendi." }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { result = false, message = "Bilgileriniz güncellenemedi." }, JsonRequestBehavior.AllowGet);
                                }

                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = "Bilgileriniz güncellenemedi." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = "Şifreniz zayıf daha güçlü bir şifre giriniz." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Hata oluştu" }, JsonRequestBehavior.AllowGet);
            } 
        }

        [HttpPost]
        public async Task<JsonResult> JustUpdate()
        {
            MainModel mainModel = new MainModel("");
            return Json(new { result = true, message = "Bilgileriniz güncellendi." }, JsonRequestBehavior.AllowGet);
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