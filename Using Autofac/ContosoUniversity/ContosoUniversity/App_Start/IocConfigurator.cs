using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace ContosoUniversity
{ 
    public static class IocConfigurator
    {
        public static void ConfigureDependencyInjection()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterType<SchoolContext>().InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}