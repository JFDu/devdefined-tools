using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Common;

namespace AdoDsDemo.FormsClient.Models
{
    [DataServiceKey("CurrencyCode")]
    public class Currency
    {
        public Currency() { ModifiedDate = DateTime.Now; }
        public string CurrencyCode { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
