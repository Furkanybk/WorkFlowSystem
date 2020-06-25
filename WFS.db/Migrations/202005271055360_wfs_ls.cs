namespace WFS.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wfs_ls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chat",
                c => new
                    {
                        ChatId = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                        State = c.String(),
                        State_Date = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ChatId);
            
            CreateTable(
                "dbo.ChatRoom",
                c => new
                    {
                        ChatRoomId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ChatRoomId);
            
            CreateTable(
                "dbo.ClientManager",
                c => new
                    {
                        ClientManagerId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Contact = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        Login_Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        IsWFSuser = c.Boolean(nullable: false),
                        ManagerFirmId = c.Long(nullable: false),
                        managerUserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ClientManagerId)
                .ForeignKey("dbo.Firm", t => t.ManagerFirmId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.managerUserId, cascadeDelete: true)
                .Index(t => t.ManagerFirmId)
                .Index(t => t.managerUserId);
            
            CreateTable(
                "dbo.Firm",
                c => new
                    {
                        FirmId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Mail = c.String(),
                        Contact = c.String(),
                        Fax = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Address = c.String(),
                        Url = c.String(),
                        Logo = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        showMain = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FirmId);
            
            CreateTable(
                "dbo.CustomerFirmManager",
                c => new
                    {
                        CustomerFirmManagerId = c.Long(nullable: false, identity: true),
                        Token = c.String(),
                        ClientId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerFirmManagerId)
                .ForeignKey("dbo.ClientManager", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                        Definition = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.WorkFlow",
                c => new
                    {
                        WorkFlowId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                        Definition = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        State = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.WorkFlowId);
            
            CreateTable(
                "dbo.Work",
                c => new
                    {
                        WorkId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                        Definition = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        Start_Date = c.DateTime(nullable: false),
                        Expected_Date = c.DateTime(nullable: false),
                        Finish_Date = c.DateTime(nullable: false),
                        State = c.String(),
                        Priority = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ProgressBar = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.WorkId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        fileId = c.Long(nullable: false, identity: true),
                        fileName = c.String(),
                        fileUrl = c.String(),
                        Work_WorkId = c.Long(),
                    })
                .PrimaryKey(t => t.fileId)
                .ForeignKey("dbo.Work", t => t.Work_WorkId)
                .Index(t => t.Work_WorkId);
            
            CreateTable(
                "dbo.WorkList",
                c => new
                    {
                        WorkListId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                        Definition = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        State = c.String(),
                        Status = c.Boolean(nullable: false),
                        WLpersonalId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.WorkListId)
                .ForeignKey("dbo.Personal", t => t.WLpersonalId, cascadeDelete: true)
                .Index(t => t.WLpersonalId);
            
            CreateTable(
                "dbo.Personal",
                c => new
                    {
                        PersonalId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        PRole = c.String(),
                        BirthDay = c.String(),
                        Contact = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Address = c.String(),
                        Mail = c.String(),
                        Password = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        Login_Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        OwnFirmId = c.Long(nullable: false),
                        personalUserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PersonalId)
                .ForeignKey("dbo.Firm", t => t.OwnFirmId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.personalUserId, cascadeDelete: true)
                .Index(t => t.OwnFirmId)
                .Index(t => t.personalUserId);
            
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        NoteId = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        NoteTxt = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        Update_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NoteId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        Token = c.String(),
                        UserName = c.String(),
                        EncryptedPassword = c.String(),
                        Role = c.String(),
                        Image = c.String(),
                        EmailVeryfied = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageId = c.Long(nullable: false, identity: true),
                        MessageName = c.String(),
                        MessageTxt = c.String(),
                        MessageDate = c.DateTime(nullable: false),
                        url = c.String(),
                        MessageRead = c.Boolean(nullable: false),
                        MessageTag = c.Boolean(nullable: false),
                        MessageTrash = c.Boolean(nullable: false),
                        State = c.Boolean(nullable: false),
                        SenderUserName = c.String(),
                        SenderUserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.User", t => t.SenderUserId, cascadeDelete: true)
                .Index(t => t.SenderUserId);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        notificationid = c.Long(nullable: false, identity: true),
                        title = c.String(),
                        message = c.String(),
                        detailUrl = c.String(),
                        createdate = c.DateTime(nullable: false),
                        state = c.String(),
                        isseen = c.Boolean(nullable: false),
                        status = c.Boolean(nullable: false),
                        notifstatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.notificationid);
            
            CreateTable(
                "dbo.DayEnd",
                c => new
                    {
                        DayEndId = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Date = c.DateTime(nullable: false),
                        Expected_Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        DepDayEndId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DayEndId)
                .ForeignKey("dbo.Firm", t => t.DepDayEndId, cascadeDelete: true)
                .Index(t => t.DepDayEndId);
            
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        EmailId = c.Long(nullable: false, identity: true),
                        MailAdres = c.String(),
                    })
                .PrimaryKey(t => t.EmailId);
            
            CreateTable(
                "dbo.ErrorLog",
                c => new
                    {
                        ErrorLogId = c.Long(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                        MoveDefination = c.String(),
                        LogDate = c.DateTime(nullable: false),
                        _User_UserId = c.Long(),
                    })
                .PrimaryKey(t => t.ErrorLogId)
                .ForeignKey("dbo.User", t => t._User_UserId)
                .Index(t => t._User_UserId);
            
            CreateTable(
                "dbo.Notifys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Details = c.String(),
                        Title = c.String(),
                        DetailsURL = c.String(),
                        SentTo = c.String(),
                        Date = c.DateTime(nullable: false),
                        IsRead = c.String(),
                        IsDeleted = c.String(),
                        IsReminder = c.String(),
                        Code = c.String(),
                        NotificationType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Root",
                c => new
                    {
                        RootId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        Register_Date = c.DateTime(nullable: false),
                        Login_Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RootId);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        TicketId = c.Long(nullable: false, identity: true),
                        SenderName = c.String(),
                        Title = c.String(),
                        Message = c.String(),
                        postTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TicketId);
            
            CreateTable(
                "dbo.UserLog",
                c => new
                    {
                        UserLogId = c.Long(nullable: false, identity: true),
                        _UserId = c.Long(nullable: false),
                        Controller = c.String(),
                        Action = c.String(),
                        MoveDefination = c.String(),
                        LogDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserLogId)
                .ForeignKey("dbo.User", t => t._UserId, cascadeDelete: true)
                .Index(t => t._UserId);
            
            CreateTable(
                "dbo.ChatRoomChat",
                c => new
                    {
                        ChatRoom_ChatRoomId = c.Long(nullable: false),
                        Chat_ChatId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChatRoom_ChatRoomId, t.Chat_ChatId })
                .ForeignKey("dbo.ChatRoom", t => t.ChatRoom_ChatRoomId, cascadeDelete: true)
                .ForeignKey("dbo.Chat", t => t.Chat_ChatId, cascadeDelete: true)
                .Index(t => t.ChatRoom_ChatRoomId)
                .Index(t => t.Chat_ChatId);
            
            CreateTable(
                "dbo.FirmCustomerFirmManager",
                c => new
                    {
                        Firm_FirmId = c.Long(nullable: false),
                        CustomerFirmManager_CustomerFirmManagerId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Firm_FirmId, t.CustomerFirmManager_CustomerFirmManagerId })
                .ForeignKey("dbo.Firm", t => t.Firm_FirmId, cascadeDelete: true)
                .ForeignKey("dbo.CustomerFirmManager", t => t.CustomerFirmManager_CustomerFirmManagerId, cascadeDelete: true)
                .Index(t => t.Firm_FirmId)
                .Index(t => t.CustomerFirmManager_CustomerFirmManagerId);
            
            CreateTable(
                "dbo.PersonalChat",
                c => new
                    {
                        Personal_PersonalId = c.Long(nullable: false),
                        Chat_ChatId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Personal_PersonalId, t.Chat_ChatId })
                .ForeignKey("dbo.Personal", t => t.Personal_PersonalId, cascadeDelete: true)
                .ForeignKey("dbo.Chat", t => t.Chat_ChatId, cascadeDelete: true)
                .Index(t => t.Personal_PersonalId)
                .Index(t => t.Chat_ChatId);
            
            CreateTable(
                "dbo.PersonalNote",
                c => new
                    {
                        Personal_PersonalId = c.Long(nullable: false),
                        Note_NoteId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Personal_PersonalId, t.Note_NoteId })
                .ForeignKey("dbo.Personal", t => t.Personal_PersonalId, cascadeDelete: true)
                .ForeignKey("dbo.Note", t => t.Note_NoteId, cascadeDelete: true)
                .Index(t => t.Personal_PersonalId)
                .Index(t => t.Note_NoteId);
            
            CreateTable(
                "dbo.UserMessage",
                c => new
                    {
                        User_UserId = c.Long(nullable: false),
                        Message_MessageId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Message_MessageId })
                .ForeignKey("dbo.User", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Message", t => t.Message_MessageId, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Message_MessageId);
            
            CreateTable(
                "dbo.UserNotification",
                c => new
                    {
                        User_UserId = c.Long(nullable: false),
                        Notification_notificationid = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Notification_notificationid })
                .ForeignKey("dbo.User", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Notification", t => t.Notification_notificationid, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Notification_notificationid);
            
            CreateTable(
                "dbo.PersonalWork",
                c => new
                    {
                        Personal_PersonalId = c.Long(nullable: false),
                        Work_WorkId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Personal_PersonalId, t.Work_WorkId })
                .ForeignKey("dbo.Personal", t => t.Personal_PersonalId, cascadeDelete: true)
                .ForeignKey("dbo.Work", t => t.Work_WorkId, cascadeDelete: true)
                .Index(t => t.Personal_PersonalId)
                .Index(t => t.Work_WorkId);
            
            CreateTable(
                "dbo.WorkWorkList",
                c => new
                    {
                        Work_WorkId = c.Long(nullable: false),
                        WorkList_WorkListId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Work_WorkId, t.WorkList_WorkListId })
                .ForeignKey("dbo.Work", t => t.Work_WorkId, cascadeDelete: true)
                .ForeignKey("dbo.WorkList", t => t.WorkList_WorkListId, cascadeDelete: true)
                .Index(t => t.Work_WorkId)
                .Index(t => t.WorkList_WorkListId);
            
            CreateTable(
                "dbo.WorkFlowWork",
                c => new
                    {
                        WorkFlow_WorkFlowId = c.Long(nullable: false),
                        Work_WorkId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkFlow_WorkFlowId, t.Work_WorkId })
                .ForeignKey("dbo.WorkFlow", t => t.WorkFlow_WorkFlowId, cascadeDelete: true)
                .ForeignKey("dbo.Work", t => t.Work_WorkId, cascadeDelete: true)
                .Index(t => t.WorkFlow_WorkFlowId)
                .Index(t => t.Work_WorkId);
            
            CreateTable(
                "dbo.DepartmentWorkFlow",
                c => new
                    {
                        Department_DepartmentId = c.Long(nullable: false),
                        WorkFlow_WorkFlowId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_DepartmentId, t.WorkFlow_WorkFlowId })
                .ForeignKey("dbo.Department", t => t.Department_DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.WorkFlow", t => t.WorkFlow_WorkFlowId, cascadeDelete: true)
                .Index(t => t.Department_DepartmentId)
                .Index(t => t.WorkFlow_WorkFlowId);
            
            CreateTable(
                "dbo.FirmDepartment",
                c => new
                    {
                        Firm_FirmId = c.Long(nullable: false),
                        Department_DepartmentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Firm_FirmId, t.Department_DepartmentId })
                .ForeignKey("dbo.Firm", t => t.Firm_FirmId, cascadeDelete: true)
                .ForeignKey("dbo.Department", t => t.Department_DepartmentId, cascadeDelete: true)
                .Index(t => t.Firm_FirmId)
                .Index(t => t.Department_DepartmentId);
            
            CreateTable(
                "dbo.ClientManagerNote",
                c => new
                    {
                        ClientManager_ClientManagerId = c.Long(nullable: false),
                        Note_NoteId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClientManager_ClientManagerId, t.Note_NoteId })
                .ForeignKey("dbo.ClientManager", t => t.ClientManager_ClientManagerId, cascadeDelete: true)
                .ForeignKey("dbo.Note", t => t.Note_NoteId, cascadeDelete: true)
                .Index(t => t.ClientManager_ClientManagerId)
                .Index(t => t.Note_NoteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLog", "_UserId", "dbo.User");
            DropForeignKey("dbo.ErrorLog", "_User_UserId", "dbo.User");
            DropForeignKey("dbo.DayEnd", "DepDayEndId", "dbo.Firm");
            DropForeignKey("dbo.ClientManagerNote", "Note_NoteId", "dbo.Note");
            DropForeignKey("dbo.ClientManagerNote", "ClientManager_ClientManagerId", "dbo.ClientManager");
            DropForeignKey("dbo.ClientManager", "managerUserId", "dbo.User");
            DropForeignKey("dbo.ClientManager", "ManagerFirmId", "dbo.Firm");
            DropForeignKey("dbo.FirmDepartment", "Department_DepartmentId", "dbo.Department");
            DropForeignKey("dbo.FirmDepartment", "Firm_FirmId", "dbo.Firm");
            DropForeignKey("dbo.DepartmentWorkFlow", "WorkFlow_WorkFlowId", "dbo.WorkFlow");
            DropForeignKey("dbo.DepartmentWorkFlow", "Department_DepartmentId", "dbo.Department");
            DropForeignKey("dbo.WorkFlowWork", "Work_WorkId", "dbo.Work");
            DropForeignKey("dbo.WorkFlowWork", "WorkFlow_WorkFlowId", "dbo.WorkFlow");
            DropForeignKey("dbo.WorkWorkList", "WorkList_WorkListId", "dbo.WorkList");
            DropForeignKey("dbo.WorkWorkList", "Work_WorkId", "dbo.Work");
            DropForeignKey("dbo.WorkList", "WLpersonalId", "dbo.Personal");
            DropForeignKey("dbo.PersonalWork", "Work_WorkId", "dbo.Work");
            DropForeignKey("dbo.PersonalWork", "Personal_PersonalId", "dbo.Personal");
            DropForeignKey("dbo.Personal", "personalUserId", "dbo.User");
            DropForeignKey("dbo.UserNotification", "Notification_notificationid", "dbo.Notification");
            DropForeignKey("dbo.UserNotification", "User_UserId", "dbo.User");
            DropForeignKey("dbo.UserMessage", "Message_MessageId", "dbo.Message");
            DropForeignKey("dbo.UserMessage", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Message", "SenderUserId", "dbo.User");
            DropForeignKey("dbo.Personal", "OwnFirmId", "dbo.Firm");
            DropForeignKey("dbo.PersonalNote", "Note_NoteId", "dbo.Note");
            DropForeignKey("dbo.PersonalNote", "Personal_PersonalId", "dbo.Personal");
            DropForeignKey("dbo.PersonalChat", "Chat_ChatId", "dbo.Chat");
            DropForeignKey("dbo.PersonalChat", "Personal_PersonalId", "dbo.Personal");
            DropForeignKey("dbo.Files", "Work_WorkId", "dbo.Work");
            DropForeignKey("dbo.FirmCustomerFirmManager", "CustomerFirmManager_CustomerFirmManagerId", "dbo.CustomerFirmManager");
            DropForeignKey("dbo.FirmCustomerFirmManager", "Firm_FirmId", "dbo.Firm");
            DropForeignKey("dbo.CustomerFirmManager", "ClientId", "dbo.ClientManager");
            DropForeignKey("dbo.ChatRoomChat", "Chat_ChatId", "dbo.Chat");
            DropForeignKey("dbo.ChatRoomChat", "ChatRoom_ChatRoomId", "dbo.ChatRoom");
            DropIndex("dbo.ClientManagerNote", new[] { "Note_NoteId" });
            DropIndex("dbo.ClientManagerNote", new[] { "ClientManager_ClientManagerId" });
            DropIndex("dbo.FirmDepartment", new[] { "Department_DepartmentId" });
            DropIndex("dbo.FirmDepartment", new[] { "Firm_FirmId" });
            DropIndex("dbo.DepartmentWorkFlow", new[] { "WorkFlow_WorkFlowId" });
            DropIndex("dbo.DepartmentWorkFlow", new[] { "Department_DepartmentId" });
            DropIndex("dbo.WorkFlowWork", new[] { "Work_WorkId" });
            DropIndex("dbo.WorkFlowWork", new[] { "WorkFlow_WorkFlowId" });
            DropIndex("dbo.WorkWorkList", new[] { "WorkList_WorkListId" });
            DropIndex("dbo.WorkWorkList", new[] { "Work_WorkId" });
            DropIndex("dbo.PersonalWork", new[] { "Work_WorkId" });
            DropIndex("dbo.PersonalWork", new[] { "Personal_PersonalId" });
            DropIndex("dbo.UserNotification", new[] { "Notification_notificationid" });
            DropIndex("dbo.UserNotification", new[] { "User_UserId" });
            DropIndex("dbo.UserMessage", new[] { "Message_MessageId" });
            DropIndex("dbo.UserMessage", new[] { "User_UserId" });
            DropIndex("dbo.PersonalNote", new[] { "Note_NoteId" });
            DropIndex("dbo.PersonalNote", new[] { "Personal_PersonalId" });
            DropIndex("dbo.PersonalChat", new[] { "Chat_ChatId" });
            DropIndex("dbo.PersonalChat", new[] { "Personal_PersonalId" });
            DropIndex("dbo.FirmCustomerFirmManager", new[] { "CustomerFirmManager_CustomerFirmManagerId" });
            DropIndex("dbo.FirmCustomerFirmManager", new[] { "Firm_FirmId" });
            DropIndex("dbo.ChatRoomChat", new[] { "Chat_ChatId" });
            DropIndex("dbo.ChatRoomChat", new[] { "ChatRoom_ChatRoomId" });
            DropIndex("dbo.UserLog", new[] { "_UserId" });
            DropIndex("dbo.ErrorLog", new[] { "_User_UserId" });
            DropIndex("dbo.DayEnd", new[] { "DepDayEndId" });
            DropIndex("dbo.Message", new[] { "SenderUserId" });
            DropIndex("dbo.Personal", new[] { "personalUserId" });
            DropIndex("dbo.Personal", new[] { "OwnFirmId" });
            DropIndex("dbo.WorkList", new[] { "WLpersonalId" });
            DropIndex("dbo.Files", new[] { "Work_WorkId" });
            DropIndex("dbo.CustomerFirmManager", new[] { "ClientId" });
            DropIndex("dbo.ClientManager", new[] { "managerUserId" });
            DropIndex("dbo.ClientManager", new[] { "ManagerFirmId" });
            DropTable("dbo.ClientManagerNote");
            DropTable("dbo.FirmDepartment");
            DropTable("dbo.DepartmentWorkFlow");
            DropTable("dbo.WorkFlowWork");
            DropTable("dbo.WorkWorkList");
            DropTable("dbo.PersonalWork");
            DropTable("dbo.UserNotification");
            DropTable("dbo.UserMessage");
            DropTable("dbo.PersonalNote");
            DropTable("dbo.PersonalChat");
            DropTable("dbo.FirmCustomerFirmManager");
            DropTable("dbo.ChatRoomChat");
            DropTable("dbo.UserLog");
            DropTable("dbo.Ticket");
            DropTable("dbo.Root");
            DropTable("dbo.Notifys");
            DropTable("dbo.ErrorLog");
            DropTable("dbo.Email");
            DropTable("dbo.DayEnd");
            DropTable("dbo.Notification");
            DropTable("dbo.Message");
            DropTable("dbo.User");
            DropTable("dbo.Note");
            DropTable("dbo.Personal");
            DropTable("dbo.WorkList");
            DropTable("dbo.Files");
            DropTable("dbo.Work");
            DropTable("dbo.WorkFlow");
            DropTable("dbo.Department");
            DropTable("dbo.CustomerFirmManager");
            DropTable("dbo.Firm");
            DropTable("dbo.ClientManager");
            DropTable("dbo.ChatRoom");
            DropTable("dbo.Chat");
        }
    }
}
