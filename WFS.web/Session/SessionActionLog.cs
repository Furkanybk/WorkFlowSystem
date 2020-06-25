using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.db.Tables;
using WFS.db.WFScontext;

namespace WFS.web.Session
{
    public static class SessionActionLog
    {
        public async static void RecordPageAccess(string controller, string action)
        {
            try
            { 
                var _user = SessionUser.User.User;    

                using (cfgContext db = new cfgContext())
                { 
                    UserLog _UL = new UserLog
                    {
                        _UserId = _user.UserId, 
                        Action = action,
                        Controller = controller, 
                        LogDate = DateTime.Now 
                    }; 

                    db.UserLog.Add(_UL);
                    await db.SaveChangesAsync();
                } 
            }
            catch
            {

            }
        }
    }
}