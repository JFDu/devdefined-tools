using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using AdoDsDemo.Services.Models;
using System.Linq.Expressions;

namespace AdoDsDemo.Services
{
    public class SystemApi : DataService<SystemEntities>
    {
        public static void InitializeService(IDataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
        }

        /*[QueryInterceptor("Types")]
        public Expression<Func<TypeInfo, bool>> Exclude()
        {
            return (type) => type.BaseType == "
        }*/

    }
}
