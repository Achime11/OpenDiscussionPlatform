using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.DataAccessLayer {
    // A Database Context should be used per-request with 'using' block
    // https://docs.microsoft.com/en-us/ef/ef6/fundamentals/working-with-dbcontext#lifetime

    // "In reality, MVC applications are designed to have a single context,
    // and you should actually add your models to the existing context created by the scaffold, 
    // rather than create a new, separate context"
    //https://stackoverflow.com/questions/23200365/multiple-database-files-context-asp-net-mvc

    // The connection string is found by the Entity Framework in Web.Config by using the name of this context (very cool)
    // https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application#specify-the-connection-string
    
    public class DatabaseContext:DbContext {
        public DatabaseContext() {
            // https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/migrations/#automatically-upgrading-on-application-startup-migratedatabasetolatestversion-initializer
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}