using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorSettlementAccountDlDto : EntityDto<ContractorSettlementAccountDlDto, ContractorSettlementAccount>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string AccountName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(20)]
        [LocalizedMinLength(20)]
        public string AccountCode { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int BankId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StateId { get; set; }
        [LocalizedRequired]
        public bool IsMain { get; set; }
    }
}
