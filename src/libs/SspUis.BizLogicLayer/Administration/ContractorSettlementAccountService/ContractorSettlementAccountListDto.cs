using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Administration.ContractorSettlementAccountService
{
    public class ContractorSettlementAccountListDto : ILinkToEntity<ContractorSettlementAccount>
    {
        public long Id { get; set; } 
        public string Bank { get; set; }
        public string BankCode { get; set; }
        public string State { get; set; }
        public long OwnerId { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public bool IsMain { get; set; }
    }
}
