using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Castle.MonoRail.Framework.Routing;

namespace MonoRailDemo.Web
{
    public class VerbPatternRoute : IRoutingRule
    {
        private readonly PatternRoute _innerRoute;
        private readonly string _httpMethod;

        public VerbPatternRoute(string httpMethod, PatternRoute innerRoute)
        {
            _httpMethod = httpMethod;
            _innerRoute = innerRoute;
        }

        public string CreateUrl(string hostname, string virtualPath, System.Collections.IDictionary parameters, out int points)
        {
            return _innerRoute.CreateUrl(hostname, virtualPath, parameters, out points);
        }

        public int Matches(string url, Castle.MonoRail.Framework.IRouteContext context, RouteMatch match)
        {
            if (context.Request.HttpMethod == _httpMethod)
            {
                return _innerRoute.Matches(url, context, match);
            }

            return 0;
        }

        public string RouteName
        {
            get { return _innerRoute.RouteName; }
        }

    }
}
