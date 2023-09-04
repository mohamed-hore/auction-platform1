using MazadMarket.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MazadMarket
{

    public class MvcApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //here in Application Start we will start Sql Dependency
            SqlDependency.Start(con);
        }
        //protected void Session_Start(object sender ,EventArgs e )
        //{
        //    NotificationComponent NC = new NotificationComponent();
        //    var currentTime = DateTime.Now;
        //    HttpContext.Current.Session["LastUpdated"] = currentTime;
        //  //  NC.RegisterNotification(currentTime);

        //}


        //protected void Application_End()
        //{
        //    SqlDependency.Stop(con);
        //}
        protected void Session_Start(object sender, EventArgs e)
        {
            NotificationComponent NC = new NotificationComponent();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["LastUpdated"] = currentTime;
            NC.RegisterNotification(currentTime);
            NC.RegisterNotificationAuction(currentTime);
            
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {


            HttpCookie cookie = HttpContext.Current.Request.Cookies["Languages"];
            if (cookie != null && cookie.Value != null )
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value);
            }
        }


        protected void Application_End()
        {
            //here we will stop Sql Dependency
            SqlDependency.Stop(con);
        }
    }
}
