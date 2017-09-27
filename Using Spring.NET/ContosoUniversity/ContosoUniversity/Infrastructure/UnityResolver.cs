using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace ContosoUniversity.Infrastructure
{
    public class UnityResolver : IDependencyResolver
    {
        private readonly IUnityContainer _unityContainer;

        public UnityResolver(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }
        object IDependencyResolver.GetService(Type serviceType)
        {
            try
            {
               return _unityContainer.Resolve(serviceType);
            }
            catch (Exception)
            {

                return null;
            }
        }

        IEnumerable<object> IDependencyResolver.GetServices(Type serviceType)
        {
            try
            {
                return _unityContainer.ResolveAll(serviceType);
            }
            catch (Exception)
            {

                return new List<object>();
            }
        }

        public void Dispose()
        {
            _unityContainer.Dispose();
        }
    }
}