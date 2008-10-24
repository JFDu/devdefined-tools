using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Helpers;
using MonoRailDemo.Web.Models;


namespace MonoRailDemo.Web.Controllers
{    
    [Layout("default")]
    public class CustomerController : SmartDispatcherController
    {
        private static List<Customer> _customers = new List<Customer> { 
             new Customer { Id = 1, FirstName = "Alex", LastName = "Henderson" },
             new Customer { Id = 1, FirstName = "Joseph", LastName = "Stalin" },
             new Customer { Id = 1, FirstName = "George", LastName = "Bush" },
             new Customer { Id = 1, FirstName = "Jason", LastName = "Gunn" },
             new Customer { Id = 1, FirstName = "Kid", LastName = "Koala" }
        };

        public void Index(string format)
        {
            RestRequest.For(format)
                .HasResponses(response =>
                {
                    response.Json(json => json.Serialize(_customers));
                    response.Xml(xml => xml.Serialize(_customers));
                    response.Html(html =>
                    {
                        PropertyBag["customers"] = _customers;                        
                    });
                });
        }

        public void Get(int id, string format)
        {
            Customer customer = _customers.First(c => c.Id == id);

            RestRequest.For(format)
                .HasResponses(response =>
                {
                    response.Json(json => json.Serialize(customer));
                    response.Xml(xml => xml.Serialize(customer));
                    response.Html(html =>
                    {
                        PropertyBag["customer"] = customer;                        
                    });
                });
        }

        public void Insert(Customer customer)
        {

        }
    }

    public static class RestRequest
    {
        public static RestRequestResponse For(string format)
        {
            return new RestRequestResponse(format);
        }
    }

    public class RestRequestResponse
    {
        private Dictionary<string, Action<IControllerContext, IEngineContext>> _responseActionsByFormat = new Dictionary<string,Action<IControllerContext, IEngineContext>>();
        private IControllerContext _context;
        private IEngineContext _engineContext;
        private string _selectedFormat;

        public RestRequestResponse(string selectedFormat)
            : this(selectedFormat, MonoRailHttpHandlerFactory.CurrentControllerContext, MonoRailHttpHandlerFactory.CurrentEngineContext)
        {
        }

        public RestRequestResponse(string selectedFormat, IControllerContext context, IEngineContext engineContext)
        {
            _selectedFormat = selectedFormat;
            _engineContext = engineContext;
            _context = context;
        }

        public string SelectedFormat
        {
            get { return _selectedFormat; }
        }

        public IControllerContext Context
        {
            get { return _context; }
        }

        public IEngineContext EngineContext
        {
            get { return _engineContext; }
        }

        public void HasResponses(Action<RestRequestResponse> handler)
        {
            handler.Invoke(this);
            _responseActionsByFormat[_selectedFormat].Invoke(_context, _engineContext);
        }

        public void AddResponseActionForFormat(string format, Action<IControllerContext, IEngineContext> action)
        {
            _responseActionsByFormat[format] = action;
        }
    }

    public static class ResponseExtensiosn
    {
        public static void Xml(this RestRequestResponse response, Action<XmlFormatter> formattingAction)
        {
            response.AddResponseActionForFormat("xml", (context, engineContext) => RenderXmlResponse(formattingAction, context, engineContext));
        }

        public static void Json(this RestRequestResponse response, Action<JsonFormatter> formattingAction)
        {
            response.AddResponseActionForFormat("json", (context, engineContext) => RenderJsonResponse(formattingAction, context, engineContext));
        }

        public static void Html(this RestRequestResponse response, Action<HtmlFormatter> formattingAction)
        {
            response.AddResponseActionForFormat("html", (context, engineContext) => RenderHtmlResponse(formattingAction));
        }

        private static void RenderXmlResponse(Action<XmlFormatter> action, IControllerContext context, IEngineContext engineContext)
        {
            XmlFormatter formatter = new XmlFormatter(context, engineContext);
            action.Invoke(formatter);            
        }

        private static void RenderJsonResponse(Action<JsonFormatter> action, IControllerContext context, IEngineContext engineContext)
        {
            JsonFormatter formatter = new JsonFormatter(context, engineContext);
            action.Invoke(formatter);
        }

        private static void RenderHtmlResponse(Action<HtmlFormatter> action)
        {
            HtmlFormatter formatter = new HtmlFormatter();
            action.Invoke(formatter);
        }
    }

    public class HtmlFormatter
    {
        public void RenderDefaultView() { }
    }

    public class XmlFormatter
    {
        private readonly RenderingSupport _renderingSupport;
        private readonly IEngineContext _engineContext;

        public XmlFormatter(IControllerContext context, IEngineContext engineContext)
        {
            _engineContext = engineContext;
            _renderingSupport = new RenderingSupport(context, engineContext);                
        }        

        public void Serialize(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                _renderingSupport.RenderText(writer.ToString());
            }
            _engineContext.Response.ContentType = "text/xml";
        }
    }

    public class JsonFormatter
    {
        private readonly RenderingSupport _renderingSupport;
        private readonly IEngineContext _engineContext;

        public JsonFormatter(IControllerContext context, IEngineContext engineContext)
        {
            _engineContext = engineContext;
            _renderingSupport = new RenderingSupport(context, engineContext);                
        }

        public void Serialize(object obj)
        {
            _renderingSupport.RenderText(_engineContext.Services.JSONSerializer.Serialize(obj));
            _engineContext.Response.ContentType = "text/json";
        }
    }
}