using MazadMarket.Hubs;
using MazadMarket.Models;
using MazadMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MazadMarket.Controllers
{
    public class HomeController : Controller
    {

        private Model1 db = new Model1();
        public ActionResult Index()
        {
            HomePageViewModel productlists = new HomePageViewModel();
            productlists.lastAddedProduct = db.Products.OrderByDescending(a => a.dateOfAdd).Where(a => (a.sold.Equals(false) && a.approved.Equals(true) && a.dateOfAuctionStart <= DateTime.Now && a.dateOfAuctionEnd >= DateTime.Now && !a.userId.Equals("Admin"))).Take(8).ToList();
            productlists.lastOfOurProduct = db.Products.OrderByDescending(a => a.dateOfAdd).Where(a => (a.sold.Equals(false) && a.approved.Equals(true) && a.dateOfAuctionStart <= DateTime.Now && a.dateOfAuctionEnd >= DateTime.Now && a.userId.Equals("Admin"))).Take(8).ToList();
            productlists.lastSoldProduct = db.Products.OrderByDescending(a => a.dateOfSell).Where(a => a.sold.Equals(true)).Take(8).ToList();
            return View(productlists);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult language(string language)
        {

            if (!String.IsNullOrEmpty(language))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }

            HttpCookie cookie = new HttpCookie("Languages");
            cookie.Value = language;
            Response.Cookies.Add(cookie);


            return RedirectToAction("Index");
        }



        public JsonResult GetNotification()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetNotifications(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }




        //public JsonResult GetNotificationProducts()
        //{
        //    var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
        //    NotificationComponent NC = new NotificationComponent();
        //    var list = NC.GetProduct(notificationRegisterTime);
        //    //update session here for get only new added contacts (notification)
        //    Session["LastUpdate"] = DateTime.Now;
        //    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}


        //public JsonResult GetNotificationProductsAuc()
        //{
        //    var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
        //    NotificationComponent NC = new NotificationComponent();
        //    var list = NC.GetProductAuc(notificationRegisterTime);
        //    //update session here for get only new added contacts (notification)
        //    Session["LastUpdate"] = DateTime.Now;
        //    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

    }
}