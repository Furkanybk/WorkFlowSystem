using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WFS.db.Tables;
using WFS.db.WFScontext;
using WFS.web.App_Start;
using WFS.web.Services;
using WFS.web.Session;

namespace WFS.web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {

                using (cfgContext db = new cfgContext())
                {
                    if (db.Root.ToList().Count == 0)
                    {
                        var management = new business.Management.Management.Login();

                        Root newRoot = new Root
                        {
                            Name = "Furkan",
                            Username = "Yoobibikunn",
                            Password = business.SessionSettings.Crypting.En_De_crypt._Encrypt("*airbusA380boeing-"),
                            Register_Date = DateTime.Now,
                            Login_Date = default(DateTime),
                            Status = true
                        };
                        management.createRoot(newRoot);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BackgroundWorker bWorker = new BackgroundWorker();
            bWorker.RunJob();
            GlobalFilters.Filters.Add(new LogSessionActivityAttribute()); 

        }
        public void Application_Error(Object sender, EventArgs e)
        { 
            try
            {
                Exception exception = Server.GetLastError();
                Server.ClearError();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "Panel");
                routeData.Values.Add("action", "Error");
                routeData.Values.Add("exception", exception);

                if (exception.GetType() == typeof(HttpException))
                {
                    routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
                }
                else
                {
                    routeData.Values.Add("statusCode", 500);
                }

                Response.TrySkipIisCustomErrors = true;
                IController controller = new web.Controllers.PanelController();
                controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                Response.End();
            }
            catch (Exception)
            {

            }
        }
         
    }
}
