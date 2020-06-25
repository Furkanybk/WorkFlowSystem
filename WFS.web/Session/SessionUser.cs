using Newtonsoft.Json; 
using System.Web; 
using WFS.web.Models;

namespace WFS.web.Session
{
    public static class SessionUser
    { 
       public static LoginedUser User
        {
            get
            {
                 return string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? null : JsonConvert.DeserializeObject<LoginedUser>(HttpContext.Current.User.Identity.Name);
            }
        }
       
    }
}