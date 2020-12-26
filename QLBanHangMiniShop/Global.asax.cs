using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QLBanHangMiniShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HttpContext.Current.Application["VisitorOnline"] = 0;
            HttpContext.Current.Application.Add("VisitorOnline", 0);           
        }
        //protected void Session_End(object sender,EventArgs e)
        //{
            
        //    Class.SessionsAccess session = (Class.SessionsAccess)Session["Access"];
        //    if(session.modeAccess==Class.AccessMode.Visitor||session.modeAccess==Class.AccessMode.Member)
        //    {
        //        Class.Applications.UpdateVisitorOnline(false);
        //    }
            
        //}
        protected void Session_Start()
        {
            string s= "";
        }

    }
}
