using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.db.Tables;

namespace WFS.web.Models
{
    public class MainModel : IDisposable
    {
        public MainModel(string req)
        { 
            getCurrentUser();
            getMessages();

            switch (req)
            {
                case "getFirms":
                    getFirms();
                    break;
                case "getDepartments":
                    getDepartments();
                    break;
                case "getWorkFlows":
                    getWorkFlows();
                    break;
                case "getWorks":
                    getWorks();
                    break; 
                case "WFS":
                    getWFS();
                    break;
                case "Message":
                    getMessages();
                    break;
                case "mainpage":
                    getMainPage();
                    break;
            }

        }
          
        public MainModel(string incomeRequest, long id)
        { 
            getCurrentUser();
            getMessages();
            switch (incomeRequest)
            { 
                case "getPartners":
                    getCustomerFirms(id);
                    break;
                case "getPersonals":
                    getPersonals(id);
                    break;
                case "getWork":
                    getWork(id);
                    break;

                case "FindFirm":
                    findFirm(id);
                    break;
                case "FindPartner":
                    findPartner(id);
                    break;
                case "FindDepartment":
                    editDepartment(id);
                    break;
                case "FindWorkFlow":
                    editWorkFlow(id);
                    break;
                case "FindWork":
                    editWork(id);
                    break;
                case "FindPersonal":
                    editPersonal(id);
                    break;
                case "getNotes":
                    getNotes(id);
                    break;
                case "WFS":
                    getWFS(id);
                    break;
                case "workDetail":
                    workDetail(id);
                    break;
                case "getOwnWorks":
                    getOwnWorks(id);
                    break;
                case "getNotifications":
                    getNotifications(id);
                    break;
                case "Message":
                    getMessage(id);
                    break;
                default:
                    break;
            }
        }

        public MainModel(string incomeRequest, long id, long did)
        {
            getCurrentUser();
            getMessages();
            switch (incomeRequest)
            { 
                case "WFS":
                    getWFS(id,did);
                    break;
                default:
                    break;
            }
        }

        public MainModel(string incomeRequest, long id, long did, long wfid)
        {
            getCurrentUser();
            getMessages();
            switch (incomeRequest)
            {
                case "WFS":
                    getWFS(id, did, wfid);
                    break;
                default:
                    break;
            }
        }


        public void getCurrentUser()
        {
            using (business.Management.UserManagement.UserFunctions um = new business.Management.UserManagement.UserFunctions())
            {
                var user = web.Session.SessionUser.User;

                var DencPassword = business.SessionSettings.Crypting.En_De_crypt._Decrypt(user.User.EncryptedPassword);
                var a =  um.GetUser(user.User.UserName, DencPassword).Result; 
                user.User = a;

                if (user.User.Role.Contains("ClientManager"))
                {
                    clientManager = um.findClientManager(user.ClientManager_Id);
                    CurrentUserViewModel current = new CurrentUserViewModel
                    {
                        Id = clientManager.ClientManagerId,
                        ownFirm = clientManager.ManagerFirm,
                        userId = clientManager.managerUserId,
                        Role = clientManager.managerUser.Role,
                        PRole = "Admin",
                        Name = clientManager.Name,
                        Surname = clientManager.Surname,
                        Contact = clientManager.Contact,
                        Mail = clientManager.Email,
                        Password = clientManager.Password,
                        Image = user.User.Image
                    };
                    currentUser = current;
                }
                else
                {
                    personal = um.findPersonal(user.Personal_Id); 
                    CurrentUserViewModel current = new CurrentUserViewModel
                    {
                        Id = personal.PersonalId,
                        ownFirm = personal.OwnFirm,
                        userId = personal.personalUserId,
                        Role = personal.personalUser.Role,
                        PRole = personal.PRole,
                        Name = personal.Name,
                        Surname = personal.Surname,
                        Contact = personal.Contact,
                        Birthday = personal.BirthDay,
                        City = personal.City,
                        State = personal.State,
                        Address = personal.Address,
                        Mail = personal.Mail,
                        Password = personal.Password,
                        Image = user.User.Image
                    };
                    currentUser = current;
                }

            }
        }

        public void getWFS()
        {
            var user = web.Session.SessionUser.User;
            findFirm(user.Firm_Id);
            var wfm = new business.Management.WorkFlowManagement.WorkFlowFunctions();
            
            if(firm.CustomerFirmManagers.Count != 0)
            {
                foreach (var item in firm.CustomerFirmManagers)
                {
                    if (wfsModel == null)
                    {
                        wfsModel = new List<WFS_Model>();
                    }
                    WFS_Model wfsM = new WFS_Model
                    {
                        wfsId = item.CustomerFirmManagerId,
                        Partner = item,
                        Departments = item.Client.ManagerFirm.Departments,
                        WorkFlows = item.Client.ManagerFirm.Departments.SelectMany(p => p.WorkFlows).ToList(),
                        Works = item.Client.ManagerFirm.Departments.SelectMany(p => p.WorkFlows).SelectMany(a => a.Works).ToList()
                    };
                    wfsModel.Add(wfsM);
                } 
            }
            else
            {
                wfsModel = new List<WFS_Model>();
                wfsModel = null;
            }
            
        }
        public void getWFS(long id)
        {
            getWFS();

            wfsModelM = wfsModel.FirstOrDefault(r => r.wfsId == id);
        }
        public void getWFS(long id,long did)
        {
            getWFS();

            wfsModelM = wfsModel.FirstOrDefault(r => r.wfsId == id);
            wfsModelM.WorkFlows = wfsModelM.Departments.FirstOrDefault(r => r.DepartmentId == did).WorkFlows; 
        }

        public void getWFS(long id, long did, long wfid)
        {
            getWFS();

            wfsModelM = wfsModel.FirstOrDefault(r => r.wfsId == id);
            wfsModelM.WorkFlows = wfsModelM.Departments.FirstOrDefault(r => r.DepartmentId == did).WorkFlows;
            wfsModelM.Works = wfsModelM.WorkFlows.FirstOrDefault(r => r.WorkFlowId == wfid).Works;
        }
        public void getFirms()
        {
            using (business.Management.FirmManagement.FirmFunctions fm = new business.Management.FirmManagement.FirmFunctions())
            {
                firms = fm.FirmList();
            }
        }
        public void getPersonals(long id)
        {
            using (business.Management.UserManagement.UserFunctions um = new business.Management.UserManagement.UserFunctions())
            {
                personals = um.PersonalList(id); 
            }
            getWFS();
        } 
        public void getCustomerFirms(long Id)
        {
            using (business.Management.FirmManagement.FirmFunctions fm = new business.Management.FirmManagement.FirmFunctions())
            {
                partners = fm.findFirm(Id).CustomerFirmManagers.ToList();
            }
        }
        public void getDepartments()
        {
            using (business.Management.DepartmentManagement.DepartmentFunctions dm = new business.Management.DepartmentManagement.DepartmentFunctions())
            {
                var user = web.Session.SessionUser.User;
                getCustomerFirms(user.Firm_Id);
                departments = dm.ListDepartment(user.Firm_Id);
            }
        }
        public void getWorkFlows()
        {
            using (business.Management.WorkFlowManagement.WorkFlowFunctions wfm = new business.Management.WorkFlowManagement.WorkFlowFunctions())
            {
                var user = web.Session.SessionUser.User;
                getDepartments();
                workFlows = wfm.ListWorkFlow(user.Firm_Id);
            }
        }
        public void getWorks()
        {
            using (business.Management.WorkManagement.WorkFunctions wm = new business.Management.WorkManagement.WorkFunctions())
            {
                getWorkFlows();
                works = wm.WorkList();
            }
        }
        public void getNotes(long user_id)
        {
            using (business.Management.UserManagement.UserFunctions wm = new business.Management.UserManagement.UserFunctions())
            {
                var user = web.Session.SessionUser.User;

                if(user.User.Role != "ClientManager")
                {
                    var u = wm.findNotes(user_id);
                    notes = u.Notes.ToList();
                }
                else
                {
                    var u = wm.findNotesManager(user_id);
                    notes = u.Notes.ToList();
                }
            }
        } 
        public void getWork(long id)
        {
            using (business.Management.WorkManagement.WorkFunctions wm = new business.Management.WorkManagement.WorkFunctions())
            {
                workM = wm.findWork(id);
            }
        }
        public void getNotifications(long id)
        {
            using (business.Management.NotificationManager.NotificationFunctions nom = new business.Management.NotificationManager.NotificationFunctions())
            {
                notifications = nom.getNotifications(id);
            }
        }
        public void getMessages()
        {
            try
            {
                using (business.Management.MessageManagement.MessageFunctions mm = new business.Management.MessageManagement.MessageFunctions())
                { 
                    messages = mm.ListMessages(currentUser.userId, currentUser.Role);
                    sendedMessages = mm.ListSended(currentUser.userId);
                }
            }
            catch (Exception e)
            { 
                throw;
            }
        }

        public void workDetail(long id)
        {
            using (business.Management.WorkManagement.WorkFunctions wm = new business.Management.WorkManagement.WorkFunctions())
            {
                try
                {
                    var pm = new business.Management.UserManagement.UserFunctions();
                    workM = wm.findWork(id);
                    if (workM != null)
                    {
                        wm.calculateBar(workM.WorkId);
                        foreach (var item in workM.WorkLists)
                        {
                            item.WLpersonal = pm.findPersonal(item.WLpersonalId);
                        }
                        Wpersonals = pm.getPersonals(workM.WorkId);
                    }
                }
                catch (Exception)
                { 
                    return;
                } 
            }
        } 

        public void getMessage(long id)
        { 
            try
            {
                using (business.Management.MessageManagement.MessageFunctions mm = new business.Management.MessageManagement.MessageFunctions())
                {
                    messageM = mm.getMessage(id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
        public void getOwnWorks(long id)
        {
            try
            {
                using (business.Management.UserManagement.UserFunctions wm = new business.Management.UserManagement.UserFunctions())
                { 
                    works = wm.findPersonal(id).Works.ToList();
                }
            }
            catch (Exception)
            { 
                throw;
            }
        }

        public void findFirm(long id)
        {
            using (business.Management.FirmManagement.FirmFunctions fm = new business.Management.FirmManagement.FirmFunctions())
            {
                firm = fm.findFirm(id); 
            }
        }
        public void findPartner(long id)
        {
            using (business.Management.PartnerManagement.PartnerFunctions partner = new business.Management.PartnerManagement.PartnerFunctions())
            {
                customerFirm = partner.findPartner(id); 
                getWFS(customerFirm.CustomerFirmManagerId);

                if (ListPersonals == null)
                {
                    ListPersonals = new List<List<Personal>>();
                }

                foreach (var item in wfsModel.SelectMany(p => p.Works))
                {
                    var pm = new business.Management.UserManagement.UserFunctions();
                    
                    Wpersonals = pm.getPersonals(item.WorkId);
                    ListPersonals.Add(Wpersonals);
                }
            }
        }

        public void editPersonal(long id)
        {
            using (business.Management.UserManagement.UserFunctions um = new business.Management.UserManagement.UserFunctions())
            {
                var a  = um.findPersonal(id);
                if(a.Password != null)
                {
                    var decPass = business.SessionSettings.Crypting.En_De_crypt._Decrypt(a.Password);
                    a.Password = decPass;
                }
                personal = a;
            }
        }
        public void editDepartment(long id)
        {
            using (business.Management.DepartmentManagement.DepartmentFunctions department = new business.Management.DepartmentManagement.DepartmentFunctions())
            {
                departmentM = department.findDepartment(id);
            }
        }
        public void editWorkFlow(long id)
        {
            using (business.Management.WorkFlowManagement.WorkFlowFunctions wflow = new business.Management.WorkFlowManagement.WorkFlowFunctions())
            {
                workflowM = wflow.findWorkFlow(id);
            }
        }
        public void editWork(long id)
        {
            using (business.Management.WorkManagement.WorkFunctions w = new business.Management.WorkManagement.WorkFunctions())
            {
                workM = w.findWork(id);
            }
        }

        public void getMainPage()
        {
            var partnerCount = currentUser.ownFirm.CustomerFirmManagers.Count;
            var workb = new business.Management.WorkManagement.WorkFunctions(); 
            try
            {
                var id = currentUser.ownFirm.FirmId;
                MainPageViewModel mp = new MainPageViewModel
                { 
                    partnerCount = partnerCount,
                    completedWorkCount = workb.getWorkList("finished", id),
                    onGoingWorkCount = workb.getWorkList("ongoing", id),
                    lastWorks = workb.LastWorkList(id),
                    messages = messages 
                };
                mainpage = mp;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<Firm> firms { get; set; }
        public List<Personal> personals { get; set; }
        public List<Personal> Wpersonals { get; set; }
        public List<List<Personal>> ListPersonals { get; set; }
        public List<CustomerFirmManager> partners { get; set; }
        public List<Department> departments { get; set; }
        public List<WorkFlow> workFlows { get; set; }
        public List<Work> works { get; set; } 
        public List<Note> notes { get; set; } 
        public List<Notification> notifications { get; set; } 
        public List<Message> messages { get; set; }  
        public List<Message> sendedMessages { get; set; } 

        public List<WFS_Model> wfsModel { get; set; }
        public WFS_Model wfsModelM { get; set; }

        public CustomerFirmManager customerFirm { get; set; }
        public Department departmentM { get; set; }
        public WorkFlow workflowM { get; set; }
        public Work workM { get; set; }
        public Note noteM { get; set; }
        public Notification notfM { get; set; }
        public Message messageM { get; set; }

        public Firm firm { get; set; }
        public ClientManager clientManager { get; set; }
        public Personal personal { get; set; }
        public CurrentUserViewModel currentUser { get; set; }
        public MainPageViewModel mainpage { get; set; }


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
    }

    public class RootModel : IDisposable
    { 
        public RootModel(string req)
        {
            getCurrentUser();
            switch (req)
            {
                case "getFirms":
                    getFirms();
                    break;

                case "tickets":
                    getTickets();
                    break;
            } 
        }

        public void getFirms()
        {
            try
            {
                using (business.Management.UserManagement.UserFunctions um = new business.Management.UserManagement.UserFunctions())
                {
                    Managers =  um.ClientManagerList();   
                }
            }
            catch(Exception)
            {
            }
        }

        public void getTickets()
        {
            try
            {
                using (db.WFScontext.cfgContext db = new db.WFScontext.cfgContext())
                {
                    tickets = db.Ticket.ToList(); 
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void getCurrentUser()
        {
            using (business.Management.Management.Login um = new business.Management.Management.Login())
            {
                var user = web.Session.SessionUser.User.Root; 
                CurrentRootViewModel current = new CurrentRootViewModel
                {
                    Id = user.RootId,
                    Name = user.Name, 
                    Mail = user.Username,
                    Password = user.Password
                };
                currentRoot = current; 
            }
        }
        public List<FirmManagerVM> FirmManagers { get; set; }
        public List<Firm> firms { get; set; }
        public List<ClientManager> Managers { get; set; }
        public List<Personal> personals { get; set; }   
        public List<Department> departments { get; set; }
        public List<WorkFlow> workFlows { get; set; }
        public List<Work> works { get; set; }
        public List<Note> notes { get; set; } 
        public List<Message> messages { get; set; } 
        public List<Ticket> tickets { get; set; } 

        public List<WFS_Model> wfsModel { get; set; }
        public WFS_Model wfsModelM { get; set; }
         
        public Department departmentM { get; set; }
        public WorkFlow workflowM { get; set; }
        public Work workM { get; set; }
        public Note noteM { get; set; } 
        public Message messageM { get; set; } 
        public Firm firm { get; set; }
        public ClientManager clientManager { get; set; }
        public Personal personal { get; set; } 
        public Ticket ticket { get; set; }  

        public CurrentRootViewModel currentRoot { get; set; } 
         
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
}