using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFS.db.Tables;

namespace WFS.db.WFScontext
{
    public class cfgContext : DbContext
    {
        public cfgContext() : base(nameOrConnectionString: "connectStr")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        #region Tables
        public DbSet<User> User { get; set; }
        public DbSet<Root> Root { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<ChatRoom> ChatRoom { get; set; }
        public DbSet<ClientManager> ClientManager { get; set; }
        public DbSet<CustomerFirmManager> CustomerFirmManager { get; set; }
        public DbSet<DayEnd> DayEnd { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Firm> Firm { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Notification> Notification { get; set; } 
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Work> Work { get; set; }
        public DbSet<WorkList> WorkList { get; set; }
        public DbSet<WorkFlow> WorkFlow { get; set; } 
        public DbSet<Files> UploadFiles { get; set; } 
        public DbSet<Email> Email { get; set; } 
        public DbSet<Message> Message { get; set; } 
        public DbSet<Ticket> Ticket { get; set; } 
        public DbSet<UserLog> UserLog { get; set; }  
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region RelationShip
            //One to one relationship
            #region One to One

            #region Firm-ClientManager
            modelBuilder.Entity<Firm>().ToTable("Firm").HasKey(x => x.FirmId);
            modelBuilder.Entity<ClientManager>().ToTable("ClientManager").HasKey(x => x.ClientManagerId);
            modelBuilder.Entity<ClientManager>().HasRequired(x => x.ManagerFirm).WithMany().HasForeignKey(x => x.ManagerFirmId);
            #endregion  

            #region Firm-Personal
            modelBuilder.Entity<Firm>().ToTable("Firm").HasKey(x => x.FirmId);
            modelBuilder.Entity<Personal>().ToTable("Personal").HasKey(x => x.PersonalId);
            modelBuilder.Entity<Personal>().HasRequired(x => x.OwnFirm).WithMany().HasForeignKey(x => x.OwnFirmId);
            #endregion

            #region Department-DayEnd
            modelBuilder.Entity<Department>().ToTable("Department").HasKey(x => x.DepartmentId);
            modelBuilder.Entity<DayEnd>().ToTable("DayEnd").HasKey(x => x.DayEndId);
            modelBuilder.Entity<DayEnd>().HasRequired(x => x.DepDayEnd).WithMany().HasForeignKey(x => x.DepDayEndId);
            #endregion 

            #region User-ClientManager
            modelBuilder.Entity<User>().ToTable("User").HasKey(x => x.UserId);
            modelBuilder.Entity<ClientManager>().ToTable("ClientManager").HasKey(x => x.ClientManagerId);
            modelBuilder.Entity<ClientManager>().HasRequired(x => x.managerUser).WithMany().HasForeignKey(x => x.managerUserId);
            #endregion

            #region ClientManager-CustomerFirmManager
            modelBuilder.Entity<ClientManager>().ToTable("ClientManager").HasKey(x => x.ClientManagerId);
            modelBuilder.Entity<CustomerFirmManager>().ToTable("CustomerFirmManager").HasKey(x => x.CustomerFirmManagerId);
            modelBuilder.Entity<CustomerFirmManager>().HasRequired(x => x.Client).WithMany().HasForeignKey(x => x.ClientId);
            #endregion

            #region User-Personal
            modelBuilder.Entity<User>().ToTable("User").HasKey(x => x.UserId);
            modelBuilder.Entity<Personal>().ToTable("Personal").HasKey(x => x.PersonalId);
            modelBuilder.Entity<Personal>().HasRequired(x => x.personalUser).WithMany().HasForeignKey(x => x.personalUserId);
            #endregion  

            #region Personal-WorkList
            modelBuilder.Entity<Personal>().ToTable("Personal").HasKey(x => x.PersonalId);
            modelBuilder.Entity<WorkList>().ToTable("WorkList").HasKey(x => x.WorkListId);
            modelBuilder.Entity<WorkList>().HasRequired(x => x.WLpersonal).WithMany().HasForeignKey(x => x.WLpersonalId);
            #endregion 

            #region User-Message
            modelBuilder.Entity<User>().ToTable("User").HasKey(x => x.UserId);
            modelBuilder.Entity<Message>().ToTable("Message").HasKey(x => x.MessageId);
            modelBuilder.Entity<Message>().HasRequired(x => x.SenderUser).WithMany().HasForeignKey(x => x.SenderUserId);
            #endregion

            #region User-UserLog
            modelBuilder.Entity<User>().ToTable("User").HasKey(x => x.UserId);
            modelBuilder.Entity<UserLog>().ToTable("UserLog").HasKey(x => x.UserLogId);
            modelBuilder.Entity<UserLog>().HasRequired(x => x._User).WithMany().HasForeignKey(x => x._UserId);
            #endregion 

            #endregion
            //One to many relationship
            #region Collection RelationShip

            #region Firm RS
            modelBuilder.Entity<Firm>().HasMany(i => i.CustomerFirmManagers).WithMany();

            modelBuilder.Entity<Firm>().HasMany(i => i.Departments).WithMany();

            modelBuilder.Entity<Department>().HasMany(i => i.WorkFlows).WithMany();

            modelBuilder.Entity<WorkFlow>().HasMany(i => i.Works).WithMany(); 

            modelBuilder.Entity<Work>().HasMany(i => i.WorkLists).WithMany();  

            modelBuilder.Entity<Personal>().HasMany(i => i.Works).WithMany();  
            #endregion

            #region Personal RS 

            modelBuilder.Entity<Personal>().HasMany(i => i.Chats).WithMany();

            modelBuilder.Entity<Personal>().HasMany(i => i.Notes).WithMany();

            modelBuilder.Entity<ClientManager>().HasMany(i => i.Notes).WithMany();

            #endregion

            #region User RS
             
            modelBuilder.Entity<User>().HasMany(i => i.Notifications).WithMany(); 
            modelBuilder.Entity<User>().HasMany(i => i.Messages).WithMany();

            #endregion

            #region Chat RS
            modelBuilder.Entity<ChatRoom>().HasMany(i => i.Chats).WithMany();
            #endregion

            #endregion
            #endregion
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
