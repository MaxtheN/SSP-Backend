using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.BankCredit.Models
{
    public class BankCreditContractStatusDto
    {
        public object Error { get; set; }
        public object Message { get; set; }
        public string Timestamp { get; set; }
        public int Status { get; set; }
        public object Path { get; set; }
        public List<BankCreditContractStatusResponse> Data { get; set; }
        public object Response { get; set; }
    }
    public class BankCreditContractStatusResponse
    {
        public int ContractorTin { get; set; }
        public string ContractorName { get; set; }
        public int CreditState { get; set; }
    }
}