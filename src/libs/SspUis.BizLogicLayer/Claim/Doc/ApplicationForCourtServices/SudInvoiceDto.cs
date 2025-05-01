using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Claim.Doc.ApplicationForCourtServices
{
    public class SudInvoiceDto
    {
        public Guid CourtId { get; set; }
        public long tin { get; set; }
        public decimal? Amount { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
    }
}
