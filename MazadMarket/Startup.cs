using MazadMarket.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(MazadMarket.Startup))]
namespace MazadMarket
{
    public partial class Startup
    {


        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
           
            ConfigureAuth(app);
            CreateRoles();
            CreateUsers();

            //var sqlConnectionString = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            //GlobalHost.DependencyResolver.UseSqlServer(sqlConnectionString);
            //this.ConfigureAuth(app);
            app.MapSignalR();
        }
        


        public void CreateUsers()
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = new ApplicationUser();
            user.Email = "mhd@ASPU.com";
            user.UserName = "MHD";
            user.PhoneNumber ="00919512334653";
            user.fullName = "mohamed";
            user.bankCardNumber ="811491529174";
            user.address = "syria";

            var check = userManager.Create(user, "MHD@lITEngneer123");
            if (check.Succeeded)
            {
                userManager.AddToRole(user.Id, "Admin");
            }

        }

        public void CreateRoles()
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            IdentityRole role;
            if (!roleManager.RoleExists("Admin"))
            {
                role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("User"))
            {
                role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}
