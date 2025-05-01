using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models
{
    public class InvoiceResponseModel
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public List<InvoiceDetails> invoices { get; set; }
    }
    public class InvoiceDetails
    {
        public InvoiceReceipt receipt { get; set; }

    }

    public class InvoiceReceipt
    {
        public decimal amount { get; set; }
        public long number { get; set; }
        public DateTime issued { get; set; }
    }
}
