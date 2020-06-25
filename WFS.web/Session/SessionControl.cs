using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WFS.web.Session
{
     
   public class SessionControl : ActionFilterAttribute
    {
        public string[] Role { get; set; }  
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (!HttpContext.Current.Response.IsRequestBeingRedirected) 
                    filterContext.HttpContext.Response.Redirect("/Panel/Unauthorized"); 
            }
            else
            { 
                if (SessionUser.User.User.EmailVeryfied != true)
                {
                    if(Role != null)
                    {

                    }
                    else
                    { 
                        filterContext.HttpContext.Response.Redirect("/User/Verify");
                    }
                } 
                if (Role != null)
                {
                    if (!Role.Contains("Root"))
                    {
                        if (!Role.Contains(SessionUser.User.User.Role))
                        {
                            filterContext.HttpContext.Response.Redirect("/Panel/PermissionError");
                        }  
                    } 
                    else if (Role.Contains("Root"))
                    {
                        if (!SessionUser.User.Root.Status)
                        {
                            filterContext.HttpContext.Response.Redirect("/Root/RootLogin");
                        }
                    }
                } 
            }
        }
    }
}