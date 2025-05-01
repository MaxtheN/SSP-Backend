using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.BankCredit.Models
{
    public class BankCreditApplicationsResponseDto
    {
        public object Error { get; set; }
        public object Message { get; set; }
        public string Timestamp { get; set; }
        public int Status { get; set; }
        public object Path { get; set; }
        public BankCreditApplicationsResponse Data { get; set; }
        public object Response { get; set; }
    }
    public class BankCreditApplicationsResponse
    {
        public int ContractorTin { get; set; }
        public string ContractorName { get; set; }
        public string BankName { get; set; }
        public string BankMfo { get; set; }
        public List<BankCreditApplicationDto> Applications { get; set; }
    }
    public class BankCreditApplicationDto
    {
        public int DocNum { get; set; }
        public string DocDate { get; set; }
        public decimal? CreditSum { get; set; }
        public string DocStatus { get; set; }
        public decimal? IssuanceSum { get; set; }
    }




}
