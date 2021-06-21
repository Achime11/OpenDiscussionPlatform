using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SecondWebApplication {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CommentCreate",
                url: "Comment/New/{discussion_id}",
                defaults: new { controller = "Comment", action = "New"}
            );

            routes.MapRoute(
                name: "CategoryViewer",
                url: "Category/Show/{id}",
                defaults: new { controller = "Category", action = "Show" }
            );

            routes.MapRoute(
                name: "DiscussionViewer",
                url: "Discussion/Show/{id}",
                defaults: new { controller = "Discussion", action = "Show" }
             );
            routes.MapRoute(
                name: "DiscussionDeleter",
                url: "Discussion/Delete/{id}",
                defaults: new { controller = "Discussion", action = "Delete" }
             );

            routes.MapRoute(
                name: "DefaultRoute",
                url: "{controller}/{action}/{Id}",
                defaults: new { controller = "Category", action = "Index", Id=UrlParameter.Optional }
            );
        }
    }
}
