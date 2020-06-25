using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.db.Tables;

namespace WFS.web.Models
{
    public class LoginedUser
    { 
        public User User { get; set; } 
        public Root Root { get; set; }
        public long ClientManager_Id{ get; set; }
        public long Firm_Id{ get; set; }  
        public long Personal_Id{ get; set; }  
    }
}