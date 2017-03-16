using Microsoft.Practices.Unity;
using PoolHockeyMVC.Controllers;
using Resolver;
using Unity.Mvc5;

namespace PoolHockeyMVC
{
    public class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            System.Web.Mvc.DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // register dependency resolver for WebAPI RC
            //GlobalConfiguration.Configuration.DependencyResolver = new Unity.Mvc5.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();        
            // container.RegisterType<IProductServices, ProductServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());

            //register the parameterless constructor with Unity though and stop it taking the longer parameter by doing this
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());

            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            //Component initialization via MEF
            ComponentLoader.LoadContainer(container, ".\\bin", "PoolHockeyMVC.dll");
            ComponentLoader.LoadContainer(container, ".\\bin", "PoolHockeyBLL.dll");

        }
    }
}