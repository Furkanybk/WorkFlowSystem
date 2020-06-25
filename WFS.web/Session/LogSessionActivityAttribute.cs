using System;
using System.Web;
using System.Web.Mvc;
using WFS.db.Tables;

namespace WFS.web.Session
{
    public class LogSessionActivityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                try
                {
                    SessionActionLog.RecordPageAccess(
                        filterContext.Controller.ToString().Split('.')[3],
                        filterContext.ActionDescriptor.ActionName
                        );
                }
                catch
                {

                }
            } 
            base.OnActionExecuting(filterContext);
        }
    }
}