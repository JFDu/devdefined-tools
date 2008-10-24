using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace AdoDsDemo.Services
{
    public class NorthwindApi : DataService<AdventureWorksModel.AdventureWorksEntities>
    {
        public static void InitializeService(IDataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.UseVerboseErrors = true;            
        }
    }
}
