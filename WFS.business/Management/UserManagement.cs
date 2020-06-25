using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFS.db.Tables;
using WFS.db.WFScontext;

namespace WFS.business.Management
{
    public class UserManagement : IDisposable
    {
        public class UserFunctions : IDisposable
        {
            #region CREATE 
            public bool addClientManager(ClientManager param)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.ClientManager.Add(param);
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }

            }
            public bool addPersonal(Personal param)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.Personal.Add(param); db.SaveChanges(); return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            public long addUser(User param)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.User.Add(param); addEmail(param.UserName); db.SaveChanges(); return param.UserId;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            #endregion
            #region READ 
            public List<ClientManager> ClientManagerList()
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.ClientManager.Include("managerUser").Include("ManagerFirm").ToList();
                }
            }
            public async Task<User> GetUser(string UserName,string Password) {
                using (cfgContext db =new cfgContext ())
                {
                    var EncPassword = SessionSettings.Crypting.En_De_crypt._Encrypt(Password);
                    return await db.User.FirstOrDefaultAsync(r => r.UserName.ToLower().TrimEnd().Contains(UserName.ToLower().TrimEnd()) && r.EncryptedPassword.Equals(EncPassword));
                }
            }
            public List<Personal> PersonalList(long id)
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.Personal.Include("personalUser").Where(r => r.OwnFirmId == id).ToList();
                }
            }

            public List<Personal> getPersonals(long id)
            {
                using (cfgContext db = new cfgContext())
                {
                    try
                    {
                        List<Personal> personal = new List<Personal>();
                        foreach (var item in db.Personal.Include("Works").Include("personalUser").ToList())
                        {
                            if (item.Works.FirstOrDefault(r => r.WorkId == id) != null)
                            {
                                 personal.Add(item);
                            }

                        }
                        return personal;
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                    
                }
            }
            #endregion
            #region UPDATE 
            public bool updateClientManager(ClientManager param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var clientManager = db.ClientManager.Find(Id);

                        if (clientManager != null)
                        {
                            clientManager.Name = param.Name;
                            clientManager.Surname = param.Surname;
                            clientManager.Email = param.Email;
                            clientManager.Password = param.Password;
                            clientManager.Contact = param.Contact;
                            db.SaveChanges();
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public bool updateUser(User param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var user = db.User.Find(Id);
                            
                        if (user != null)
                        {   
                            user.UserName = param.UserName;
                            user.EncryptedPassword = param.EncryptedPassword;
                            user.Image = param.Image;
                            user.EmailVeryfied = param.EmailVeryfied;
                            db.SaveChanges();
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public bool updatePersonal(Personal param, long Id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var personal = db.Personal.Find(Id);

                        if (personal != null)
                        {
                            personal.Name = param.Name;
                            personal.Surname = param.Surname;
                            personal.PRole = param.PRole;
                            personal.BirthDay = param.BirthDay;
                            personal.Mail = param.Mail;
                            personal.Password = param.Password;
                            personal.Contact = param.Contact;
                            personal.City = param.City;
                            personal.State = param.State;
                            personal.Address = param.Address; 

                            db.SaveChanges();
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            #endregion
            #region DELETE 
            public bool deleteClientManager(long cmId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var clientManager = db.ClientManager.Find(cmId);
                        var user = db.User.FirstOrDefault(r => r.UserId == clientManager.managerUserId);
                        if (clientManager != null)
                        {
                            dellEmail(clientManager.Email); 
                            db.User.Remove(user);
                            db.ClientManager.Remove(clientManager);
                            db.SaveChanges(); return true;
                        }
                        else { return false; }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool deletePersonal(long pId)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var personal = db.Personal.Find(pId);
                        var user = db.User.FirstOrDefault(r => r.UserId == personal.personalUserId);
                        if (personal != null)
                        {
                            dellEmail(personal.Mail);
                            db.User.Remove(user);
                            db.Personal.Remove(personal);
                            db.SaveChanges(); return true;
                        }
                        else { return false; }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            #endregion

            #region FIND
            public ClientManager findClientManager(string email, string password)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var manager = db.ClientManager.Include("ManagerFirm").FirstOrDefaultAsync(x => x.Email == email && x.Password == password).Result;
                        
                        if (manager != null)
                        {
                            return manager;
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
                catch (Exception)
                {

                    return null;
                }
            }
            public long findUser(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var user = db.Personal.Include("personalUser").FirstOrDefaultAsync(x => x.PersonalId == id).Result;

                        if (user != null)
                        {
                            return user.personalUserId;
                        }
                        else
                        {
                            return 0;
                        }

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public Personal findNotes(long id)
            {
                using (cfgContext db = new cfgContext())
                { 
                    var notes = db.Personal.Include("Notes").FirstOrDefault(r => r.personalUserId == id);
                    return notes; 
                }
            }

            public ClientManager findNotesManager(long id)
            {
                using (cfgContext db = new cfgContext())
                {
                    var notes = db.ClientManager.Include("Notes").FirstOrDefault(r => r.managerUserId == id);
                    return notes;
                }
            }
             
            public bool isManagerExist(string email)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        if (db.Email.FirstOrDefault(r => r.MailAdres.ToLower().TrimEnd().Contains(email.ToLower().TrimEnd())) == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public bool isPersonalExist(string email)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        if (db.Email.FirstOrDefault(r => r.MailAdres.ToLower().TrimEnd().Contains(email.ToLower().TrimEnd())) == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public ClientManager findClientManager(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var manager = db.ClientManager.Include("managerUser").Include("ManagerFirm").Include("ManagerFirm.CustomerFirmManagers").Include("managerUser.Messages").FirstOrDefault(r => r.ClientManagerId == id);
                        if (manager != null)
                        { 
                            return manager;
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            public Personal findPersonal(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var personal = db.Personal.Include("personalUser").Include("personalUser.Messages").Include("Works").Include("OwnFirm").Include("OwnFirm.CustomerFirmManagers").FirstOrDefault(r => r.PersonalId == id);
                        if (personal != null)
                        {
                            return personal;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public long findPersonal(string email)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var personal = db.Personal.FirstOrDefault(r => r.Mail == email);
                        if (personal != null)
                        {
                            return personal.PersonalId;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            public Personal findPersonal(string email, string password)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var personal = db.Personal.Include("OwnFirm").FirstOrDefault(r => r.Mail == email && r.Password == password);
                        if (personal != null)
                        {
                            return personal;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public async Task<ClientManager> GetClientManager(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    { 
                        return await db.ClientManager.Include("ManagerFirm").Include("managerUser").FirstOrDefaultAsync(r => r.managerUserId == id);
                    }
                }
                catch
                {
                    return null;
                }
            }

            public async Task<Personal> GetPersonal(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        return await db.Personal.Include("OwnFirm").Include("personalUser").FirstOrDefaultAsync(r => r.personalUserId == id);
                    }
                }
                catch
                {
                    return null;
                }
            }
            #endregion

            #region OTHER
            public void updateLoginDate(long id, DateTime DT)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.ClientManager.Find(id).Login_Date = DT;
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public void updateLoginDateP(long id, DateTime DT)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.Personal.Find(id).Login_Date = DT;
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public void verifieUser(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        db.User.Find(id).EmailVeryfied = true;
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public User getUser(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {

                        var a = db.User.FirstOrDefault(r => r.UserId == id);
                        return a;

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public User getUserWtoken(string token)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        return db.User.FirstOrDefault(r => r.Token == token);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public void addEmail(string mail)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        Email param = new Email
                        {
                            MailAdres = mail
                        };
                        db.Email.Add(param);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public void dellEmail(string mail)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var em = db.Email.FirstOrDefault(r => r.MailAdres == mail);
                        db.Email.Remove(em);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            } 
            #endregion

            #region Dispose
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected bool Disposed { get; private set; }
            protected virtual void Dispose(bool disposing)
            {
                Disposed = true;
            }
            #endregion

        } 
         
        #region Disposee
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected bool Disposed { get; private set; }
        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        }
        #endregion
    }
}
