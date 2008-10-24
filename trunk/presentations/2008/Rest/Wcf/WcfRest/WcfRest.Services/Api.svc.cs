using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace WcfRest.Services
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    public class Api : IApi
    {
        private static List<Customer> _customers = new List<Customer> { new Customer { 
                    Organisation = "CustomerAlpha",
                    Invoices = new List<Invoice> {
                        new Invoice  { Id = 1 , Description = "Invoice #1", Cost = 20m },
                        new Invoice  { Id = 2 , Description = "Invoice #2", Cost = 35m }
                    }
                },
                new Customer { 
                    Organisation = "CustomerBeta",
                    Invoices = new List<Invoice> {
                        new Invoice  { Id = 1 , Description = "Invoice #1", Cost = 19.50m },
                        new Invoice  { Id = 2 , Description = "Invoice #2", Cost = 28m }
                    }
                }
        };

        public List<CustomerDTO> GetCustomers()
        {
            // TODO: process "skip" and "take" parameters

            return _customers.Select(customer => MapCustomer(customer)).ToList();
        }

        public Invoice GetInvoice(string customerName, string invoiceId)
        {
            return GetCustomerInvoices(customerName).First(invoice => invoice.Id.ToString() == invoiceId);
        }

        public CustomerDTO GetCustomer(string customerName)
        {
            return GetCustomers().First(customer => customer.Organisation == customerName);
        }

        public List<Invoice> GetCustomerInvoices(string customerName)
        {
            return _customers.Where(customer => customer.Organisation == customerName)
                .SelectMany(customer => customer.Invoices).ToList();
        }
        
        private CustomerDTO MapCustomer(Customer customer)
        {
            return new CustomerDTO
                {
                    Self = BuildCustomerUri(customer),
                    Organisation = customer.Organisation,
                    Invoices = BuildInvoiceUris(customer).ToList()
                };
        }

        private IEnumerable<Uri> BuildInvoiceUris(Customer customer)
        {
            if (customer.Invoices != null)
            {
                foreach (var invoice in customer.Invoices)
                {
                    yield return BuildInvoiceUri(customer, invoice);
                }
            }
        }

        private Uri BuildInvoiceUri(Customer customer, Invoice invoice)
        {
            return new Uri(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri,
                            "customers/" + customer.Organisation + "/invoices/" + invoice.Id);
        }

        private Uri BuildCustomerUri(Customer customer)
        {
            return new Uri(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri, "customers/" + customer.Organisation);
        }

        public CustomerDTO PostCustomer(Customer customer)
        {
            var outgoing = WebOperationContext.Current.OutgoingResponse;
            
            outgoing.SetStatusAsCreated(BuildCustomerUri(customer));

            return MapCustomer(customer);
        }
    }
}
