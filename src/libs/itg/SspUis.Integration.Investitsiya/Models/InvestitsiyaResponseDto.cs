using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Investitsiya.Models
{
    public class InvestitsiyaResponseDto
    {
        public long ResponseId { get; set; }
        public int ResultCode { get; set; }
        public string ResultNote { get; set; }
        public List<InvestmentContract> ContractList { get; set; }
    }
    public class InvestmentContract
    {
        public string Idn { get; set; }
        public string BankId { get; set; }
        public string BankName { get; set; }
        public int CntrStatus { get; set; }
        public string CntrStatusName { get; set; }
        public string CntrType { get; set; }
        public string CntrTypeName { get; set; }
        public string ContractorUzName { get; set; }
        public int CntrSubject { get; set; }
        public string CurrCode1 { get; set; }
        public string CurrCode2 { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string ContractorForName { get; set; }
        public string ContractorForCountryCode { get; set; }
        public string ContractorCountry { get; set; }
        public string CurrencyCodeName1 { get; set; }
        public string CurrencyCodeName2 { get; set;}
        public string ContractSubject { get; set; }
    }
}