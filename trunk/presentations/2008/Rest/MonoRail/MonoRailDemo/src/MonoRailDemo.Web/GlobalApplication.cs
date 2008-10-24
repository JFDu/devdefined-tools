using System;
using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Configuration;
using Castle.MonoRail.Framework.Internal;
using Castle.MonoRail.Framework.Routing;
using Castle.MonoRail.Framework.Views.NVelocity;
using Castle.MonoRail.WindsorExtension;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using MonoRailDemo.Web.Controllers;

namespace MonoRailDemo.Web
{
    public class GlobalApplication : HttpApplication, IContainerAccessor, IMonoRailConfigurationEvents

    {
        private static IWindsorContainer _container;

        protected void Application_Start(object sender, EventArgs e)
        {
            CreateContainer();
            RegisterWebComponents();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            RoutingModuleEx.Engine.Add(new VerbPatternRoute("GET", new PatternRoute("get_pattern", "/<controller>/<id>.<format>")
                .Restrict("id").ValidInteger
                .DefaultFor("format").Is("html")
                .DefaultForAction().Is("get")));
            
            RoutingModuleEx.Engine.Add(new VerbPatternRoute("GET", new PatternRoute("get_all_pattern", "/<controller>.<format>").DefaultFor("format").Is("html").DefaultForAction().Is("index")));            
        }
        
        protected void Application_End(object sender, EventArgs e)
        {
            _container.Dispose();
        }

        public IWindsorContainer Container
        {
            get { return _container; }
        }

        public void Configure(IMonoRailConfiguration configuration)
        {
            configuration.ViewEngineConfig.ViewPathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
            configuration.ViewEngineConfig.ViewEngines.Add(new ViewEngineInfo(typeof(NVelocityViewEngine), false));            
        }

        private static void CreateContainer()
        {
            _container = new WindsorContainer();
            _container.AddFacility("mr", new MonoRailFacility());
        }

        private static void RegisterWebComponents()
        {
            _container.Register(AllTypes.Of<SmartDispatcherController>().
                FromAssembly(typeof(HomeController).Assembly));

            _container.Register(AllTypes.Of<ViewComponent>()
                .FromAssembly(typeof(GlobalApplication).Assembly)
                .Configure(reg => reg.Named(reg.ServiceType.Name)));
        }
    }
}