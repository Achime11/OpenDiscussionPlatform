using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SecondWebApplication.Models;

[assembly: OwinStartupAttribute(typeof(SecondWebApplication.Startup))]
namespace SecondWebApplication {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {

            ConfigureAuth(app);

            // Se apeleaza o metoda in care se adauga contul de administrator si rolurile aplicatiei
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles() {
            ApplicationDbContext context = new ApplicationDbContext(); 
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)); 
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Admin")) {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin"; 
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser(); 
                user.UserName = "admin@gmail.com"; 
                user.Email = "admin@gmail.com";

                var adminCreated = UserManager.Create(user, "!1Admin");
                if (adminCreated.Succeeded) { 
                    UserManager.AddToRole(user.Id, "Admin"); 
                }
            }
            if (!roleManager.RoleExists("Moderator")) {
                var role = new IdentityRole(); 
                role.Name = "Moderator"; 
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "moderator@gmail.com";
                user.Email = "moderator@gmail.com";

                var adminCreated = UserManager.Create(user, "!1Moderator");
                if (adminCreated.Succeeded) {
                    UserManager.AddToRole(user.Id, "Moderator");
                }
            }

            if (!roleManager.RoleExists("User")) { 
                var role = new IdentityRole(); 
                role.Name = "User"; 
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "user@gmail.com";
                user.Email = "user@gmail.com";

                var adminCreated = UserManager.Create(user, "!1User");
                if (adminCreated.Succeeded) {
                    UserManager.AddToRole(user.Id, "User");
                }
            }

            // and unregistered
        }
    }
}
