using System;
using Castle.MonoRail.Framework;
using DevDefined.OAuth.Core;
using DevDefined.OAuth.Provider;

namespace DevDefined.OAuth
{
    public class OAuthFilter : Filter
    {
        [ThreadStatic] private static AccessOutcome _threadOutcome;

        protected override bool OnBeforeAction(IEngineContext context, IController controller,
                                               IControllerContext controllerContext)
        {
            var authService = context.Services.GetService<IOAuthService>();

            AccessOutcome outcome = authService.AccessProtectedResource(context.Request);

            _threadOutcome = outcome;

            if (!outcome.Granted)
            {
                controllerContext.PropertyBag["outcome"] = outcome;
                throw Error.AccessDeniedToProtectedResource(outcome);
            }

            return true;
        }

        public static AccessOutcome CurrentOutcome
        {
            get { return _threadOutcome; }
        }

        protected override void OnAfterRendering(IEngineContext context, IController controller,
                                                 IControllerContext controllerContext)
        {
            _threadOutcome = null;
        }
    }
}