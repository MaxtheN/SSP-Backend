using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models
{
    public class InvoiceModel
    {
        public Guid court_id { get; set; }
        public InvoiceEntity entity { get; set; }

        public InvoiceEntityDetails entity_details { get; set; }
        public List<Invoice> invoices { get; set; }

    }

    public class InvoiceEntity
    {
        public long tin { get; set; }
    }
     public class InvoiceEntityDetails
     {
        public string org_type { get; set; }
        public string name { get; set; }
        public string address { get; set; }
     }

    public class Invoice
    {
        public string amount_type { get; set; }
        public decimal? amount { get; set; }
    }
}
