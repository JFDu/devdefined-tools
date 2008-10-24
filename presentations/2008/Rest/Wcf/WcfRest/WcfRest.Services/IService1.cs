using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace WcfRest.Services
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in Web.config.
    [ServiceContract]
    public interface IApi
    {
        [OperationContract]
        [WebGet(UriTemplate = "customers")]
        List<CustomerDTO> GetCustomers();
        
        [OperationContract]
        [WebGet(UriTemplate = "customers/{customerName}")]
        CustomerDTO GetCustomer(string customerName);

        [OperationContract]
        [WebInvoke(UriTemplate = "customers", Method = "POST")]
        CustomerDTO PostCustomer(Customer customer);

        [OperationContract]
        [WebInvoke(UriTemplate = "customers", Method = "DELETE")]


        [OperationContract]
        [WebGet(UriTemplate = "customers/{customerName}/invoices")]
        List<Invoice> GetCustomerInvoices(string customerName);

        [OperationContract]
        [WebGet(UriTemplate = "customers/{customerName}/invoices/{invoiceId}")]
        Invoice GetInvoice(string customerName, string invoiceId);

        [OperationContract]
        [WebGet(UriTemplate = "customers.json", ResponseFormat = WebMessageFormat.Json)]
        List<CustomerDTO> GetCustomersAsJson();
    }

    [DataContract(Name="customer")]
    public class CustomerDTO
    {
        [DataMember]
        public Uri Self { get; set; }

        [DataMember]
        public string Organisation { get; set; }

        [DataMember]
        public List<Uri> Invoices { get; set; }
    }

    [DataContract]
    public class Invoice
    {
        [DataMember]
        public Decimal Cost { get; set; }
        
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Customer
    {        
        [DataMember]
        public string Organisation { get; set; }

        [DataMember]
        public List<Invoice> Invoices { get; set; }
    }
}
