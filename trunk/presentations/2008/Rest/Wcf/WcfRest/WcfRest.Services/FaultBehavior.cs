using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WcfRest.Services
{
    public class FaultingWebHttpBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(FaultingWebHttpBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new FaultingWebHttpBehavior();
        }
    }

    public class FaultingWebHttpBehavior : WebHttpBehavior
    {
        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new PrimativeErrorHandler());
        }

        public class PrimativeErrorHandler : IErrorHandler
        {
            public bool HandleError(Exception error)
            {
                return true;
            }

            public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
            {
                FaultCode faultCode = FaultCode.CreateSenderFaultCode(error.GetType().Name, "http://tempuri.org/net/exceptions");
                fault = Message.CreateMessage(version, faultCode, error.Message, null);
            }
        }
    }
}
