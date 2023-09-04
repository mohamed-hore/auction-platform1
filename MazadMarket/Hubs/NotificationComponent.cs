using MazadMarket.Models;
using MazadMarket.ViewModel;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MazadMarket.Hubs
{
    public class NotificationComponent
    {

        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT [Id],[productName],[Price] from [dbo].[Products] where [dateOfApprove] > @AddedOn";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }


        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotificationAuction(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;

            string sqlCommand = @"SELECT[Id],[ProductsId],[riseAmount],[dateOfRise] FROM [dbo].[ProductsAuctions] where [dateOfRise] > @AddedOn";
            //string sqlCommand = @"SELECT [Id],[productName],[Price] from [dbo].[Products] where [dateOfApprove] > @AddedOn";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDepAuc_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;
                //from here we will send notification message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                //re-register notification
                RegisterNotification(DateTime.Now);
            }
        }



        void sqlDepAuc_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDepAuc = sender as SqlDependency;
                sqlDepAuc.OnChange -= sqlDepAuc_OnChange;
                //from here we will send notification message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                //re-register notification
                RegisterNotificationAuction(DateTime.Now);
            }
        }




        public List<Notifications> GetNotifications(DateTime afterDate)
        {
            using (Model1 dc = new Model1())
            {
                var newAuction = dc.ProductsAuctions.Where(a => a.dateOfRise > afterDate).OrderByDescending(a => a.dateOfRise).ToList();
                var newproducts = dc.Products.Where(a => a.dateOfApprove > afterDate).OrderByDescending(a => a.dateOfApprove).ToList();
                var result = new List<Notifications>();
                Notifications m ;
                foreach (var item in newproducts)
                {
                    m = new Notifications();
                    m.notificationType = "New Product";
                    m.notification = item.productName+ " Has been Added By :"+item.userId ;
                    m.notificationDate = item.dateOfApprove;
                    result.Add(m);
                }

                foreach (var item in newAuction)
                {
                    m = new Notifications();
                    m.notificationType = "New Raise";
                    m.notification = item.userId + " Has been Raised Price of Product" + item.Products.productName +" to:"+item.riseAmount;
                    m.notificationDate = item.dateOfRise;
                    result.Add(m);
                }
                 
               // result.Sort() later 

                return result;

            }
        }









        public List<MidProduct> GetProduct(DateTime afterDate)
        {
            using (Model1 dc = new Model1())
            {

                var x = dc.Products.Where(a => a.dateOfApprove > afterDate).OrderByDescending(a => a.dateOfApprove).ToList();
                var y = new List<MidProduct>(); ;
                MidProduct m = new MidProduct();
                foreach (var item in x)
                {
                    m.productName = item.productName.ToString();
                    m.Price = item.Price.ToString();
                    y.Add(m);
                }
                return y;
            }
        }

        public List<MidAuction> GetProductAuc(DateTime afterDate)
        {
            using (Model1 dc = new Model1())
            {
                var x = dc.ProductsAuctions.Where(a => a.dateOfRise > afterDate).OrderByDescending(a => a.dateOfRise).ToList();
                var y = new List<MidAuction>(); ;
                MidAuction m = new MidAuction();
                foreach (var item in x)
                {
                    m.Id = item.Id;
                    m.ProductsId = item.ProductsId;
                    m.productsName = item.Products.productName;
                    m.riseAmount = item.riseAmount;
                    m.userId = item.userId;
                    m.dateOfRise = item.dateOfRise;
                    y.Add(m);
                }
                return y;
            }
        }
    }
}