using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateContractorDlDto : ContractorDlDto<UpdateContractorDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StateId { get; set; }
    }

    public class UpdateContractorSettlementAccountDlDto : EntityDto<UpdateContractorSettlementAccountDlDto, ContractorSettlementAccount>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }

        [LocalizedRequired]
        public string AccountName { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(20)]
        public string AccountCode { get; set; }

        [LocalizedRequired]
        public bool IsMain { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int BankId { get; set; }

        public override void UpdateEntity(ContractorSettlementAccount entity)
        {
            base.UpdateEntity(entity);
            entity.AccountName = AccountName;
            entity.AccountCode = AccountCode;
            entity.IsMain = IsMain;
            entity.BankId = BankId;
        }
    }
}
