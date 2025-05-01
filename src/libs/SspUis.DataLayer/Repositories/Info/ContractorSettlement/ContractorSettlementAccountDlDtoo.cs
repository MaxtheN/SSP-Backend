using System.Text.Json.Serialization;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer
{
    public class ContractorSettlementAccountDlDtoo<TDto> : EntityDto<TDto, ContractorSettlementAccount>, IHaveIdProp<long>
        where TDto : ContractorSettlementAccountDlDtoo<TDto>
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
        [LocalizedRange(1, int.MaxValue)]
        public long OwnerId { get; set; }

        public override void UpdateEntity(ContractorSettlementAccount entity)
        {
            base.UpdateEntity(entity);
        }

        public override ContractorSettlementAccount CreateEntity()
        {
           var data =  base.CreateEntity();
           return data;
        }
    }
}
