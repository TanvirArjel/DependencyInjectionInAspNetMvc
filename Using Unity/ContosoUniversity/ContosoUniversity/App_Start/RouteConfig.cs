using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ContosoUniversity.DAL;
using ContosoUniversity.Infrastructure;
using Microsoft.Practices.Unity;

namespace ContosoUniversity
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            //Dependency resolver configuration and services

            //UnityContainer container = new UnityContainer();
            //container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            //DependencyResolver.SetResolver(new UnityResolver(container));
        }
    }
}
