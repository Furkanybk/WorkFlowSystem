using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFS.db.Tables;
using WFS.db.WFScontext;

namespace WFS.business.Management
{
    public class MessageManagement : IDisposable
    {
        public class MessageFunctions : IDisposable
        {
            #region CREATE
            public bool addMessage(Message param, long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var personal = db.Personal.Include("Works").Include("personalUser").Include("personalUser.Messages").FirstOrDefault(s => s.personalUserId == id); 
                        if (personal.personalUser.Messages == null) personal.personalUser.Messages = new List<Message>();
                        personal.personalUser.Messages.Add(param);
                        db.SaveChanges();

                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            #endregion
            #region READ
            public List<Message> ListMessages(long id, string role)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        if(role == "ClientManager")
                        { 
                            return db.ClientManager.Include("managerUser").Include("managerUser.Messages").Include("managerUser.Messages.SenderUser").FirstOrDefault(q => q.managerUserId == id).managerUser.Messages.ToList();
                        }
                        else
                        { 
                            return db.Personal.Include("personalUser").Include("personalUser.Messages").Include("personalUser.Messages.SenderUser").FirstOrDefault(q => q.personalUserId == id).personalUser.Messages.ToList();
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public List<Message> ListSended(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    { 
                        return db.Message.Where(r => r.SenderUserId == id).Distinct().ToList();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public Message getMessage(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                       return db.Message.Include("SenderUser").FirstOrDefault(r => r.MessageId == id);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            #endregion
            #region UPDATE
            public bool setRead(long uId, long mId, string role)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        if(role == "ClientManager")
                        {
                            var msj = db.ClientManager.Include("managerUser").Include("managerUser.Messages").FirstOrDefault(r => r.ClientManagerId == uId).managerUser.Messages.FirstOrDefault(q => q.MessageId == mId);
                            msj.MessageRead = true;
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            var msj = db.Personal.Include("personalUser").Include("personalUser.Messages").FirstOrDefault(r => r.PersonalId == uId).personalUser.Messages.FirstOrDefault(q => q.MessageId == mId);
                            msj.MessageRead = true;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            public bool setStar(long uId, long mId, string role)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        if (role == "ClientManager")
                        {
                            var msj = db.ClientManager.Include("managerUser").Include("managerUser.Messages").FirstOrDefault(r => r.ClientManagerId == uId).managerUser.Messages.FirstOrDefault(q => q.MessageId == mId);
                            if(msj.MessageTag)
                            {
                                msj.MessageTag = false;
                            }
                            else
                            {
                                msj.MessageTag = true;
                            } 
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            var msj = db.Personal.Include("personalUser").Include("personalUser.Messages").FirstOrDefault(r => r.PersonalId == uId).personalUser.Messages.FirstOrDefault(q => q.MessageId == mId);
                            if (msj.MessageTag)
                            {
                                msj.MessageTag = false;
                            }
                            else
                            {
                                msj.MessageTag = true;
                            }
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            public bool moveTrash(long uId, long mId, string role)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        if (role == "ClientManager")
                        {
                            var msj = db.ClientManager.Include("managerUser").Include("managerUser.Messages").FirstOrDefault(r => r.ClientManagerId == uId).managerUser.Messages.FirstOrDefault(q => q.MessageId == mId);
                            msj.MessageTrash = true;
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            var msj = db.Personal.Include("personalUser").Include("personalUser.Messages").FirstOrDefault(r => r.PersonalId == uId).personalUser.Messages.FirstOrDefault(q => q.MessageId == mId);
                            msj.MessageTrash = true;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            public bool moveTrashAll(long uId, string role)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        if (role == "ClientManager")
                        {
                            var msj = db.ClientManager.Include("managerUser").Include("managerUser.Messages").FirstOrDefault(r => r.ClientManagerId == uId).managerUser.Messages;
                            foreach (var item in msj)
                            {
                                if(!item.MessageTrash)
                                { 
                                    item.MessageTrash = true;
                                }
                            } 
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            var msj = db.Personal.Include("personalUser").Include("personalUser.Messages").FirstOrDefault(r => r.PersonalId == uId).personalUser.Messages;
                            foreach (var item in msj)
                            {
                                if (!item.MessageTrash)
                                {
                                    item.MessageTrash = true;
                                }
                            }
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            #endregion
            #region DELETE
            #endregion
            #region FIND
            #endregion

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
