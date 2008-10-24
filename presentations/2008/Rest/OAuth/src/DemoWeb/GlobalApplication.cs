using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MonoRail.Rest;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using DevDefined.OAuth;
using MonoRailsOAuth.Web.Services;
using Castle.MonoRail.Framework.Routing;

namespace MonoRailsOAuth.Web
{
    public class GlobalApplication : HttpApplication, IContainerAccessor
    {
        private static IWindsorContainer container;

        #region IContainerAccessor

        public IWindsorContainer Container
        {
            get
            {
                if (container == null) Application_OnStart();
                return container;
            }
        }

        #endregion

        public void Application_OnStart()
        {
            if (container != null) return;
            container = new WindsorContainer(new XmlInterpreter());
            
            // register our OAuth test service, to handle validating requests to protected resources etc.

            container.AddComponent("oauth.testService", typeof(IOAuthService), typeof(TestOAuthService));

            // register service required by Rest library

            container.AddComponent("rest.defaultUrlProvider", typeof(DefaultUrlProvider));
            
            // register all controllers

            container.Register(AllTypes.Of<Castle.MonoRail.Framework.Controller>()
                .FromAssembly(typeof(GlobalApplication).Assembly));            

            // register routes
            RoutingModuleEx.Engine.Add(new PatternRoute("api_id", "api/<controller>/<id>").DefaultForAction().Is("entry").DefaultForArea().Is("api")
                .Restrict("id").ValidInteger);
            RoutingModuleEx.Engine.Add(new PatternRoute("api_collection", "api/<controller>").DefaultForAction().Is("collection").DefaultForArea().Is("api"));
            
        }

        public void Application_OnEnd()
        {
            container.Dispose();
        }
    }
}