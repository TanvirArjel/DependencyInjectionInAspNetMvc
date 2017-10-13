using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Infrastructure;
using ContosoUniversity.Models;
using Microsoft.Practices.Unity;

namespace ContosoUniversity
{
    public static class IocConfigurator
    {
        public static void ConfigureDependencyInjection()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType(typeof(IUnitOfWork), typeof(UnitOfWork));
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            DependencyResolver.SetResolver(new UnityResolver(container));
        }
    }
}