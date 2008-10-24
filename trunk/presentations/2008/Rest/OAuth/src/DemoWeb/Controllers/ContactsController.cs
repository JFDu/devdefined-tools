using Castle.MonoRail.Framework;
using Castle.MonoRail.Rest;
using Castle.MonoRail.Rest.Binding;
using DevDefined.OAuth.Core;
using DevDefined.OAuth;
using MonoRailsOAuth.Web.Models;

namespace MonoRailsOAuth.Web.Controllers
{
    [Filter(ExecuteWhen.BeforeAction, typeof (OAuthFilter))]
    [Rescue("accessdenied", typeof (AccessDeniedException))]
    [ControllerDetails(Area="API")]
    public class ContactsController : RestfulController
    {
        public void Index()
        {
            var contacts = Contact.FindAll();
            PropertyBag["contacts"] = contacts;

            RespondTo(format =>
            {
                format.Xml(xml => xml.Serialize(contacts));
                format.Html(html => html.DefaultResponse());
            });
        }

        public void Show(int id)
        {
            Contact c = Contact.FindById(id);

            RespondTo(format =>
            {
                format.Xml(response => response.Serialize(c));
                format.Html(response => response.DefaultResponse());
            });
        }

        public void Create([XmlBind] Contact createMe)
        {
            Contact.AddNew(createMe);

            RespondTo(format =>
                    format.Xml(response => response.Empty(201, headers => headers["Location"] = UrlFor(createMe.Id.ToString()))));

        }

        public void New()
        {
            RespondTo(format => format.Xml(response => response.Serialize(new Contact())));
        }

        public void Update(int id, [XmlBind] Contact contact)
        {
            contact.Id = id;

            Contact.UpdateContact(contact);

            RespondTo(format =>
                format.Xml(response => response.Empty(200)));
        }

        public void Destroy(int id)
        {
            Contact.Delete(id);

            RespondTo(format =>
                format.Xml(response => response.Empty(200)));
        }

        public void Reset()
        {
            Contact.Reset();

            RespondTo(format =>
                format.Xml(response => response.Empty(200)));
        }
    }
}