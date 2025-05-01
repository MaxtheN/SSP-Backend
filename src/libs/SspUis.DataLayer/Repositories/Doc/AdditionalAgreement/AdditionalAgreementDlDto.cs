using System;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AdditionalAgreementDlDto<TDto> : EntityDto<TDto, AdditionalAgreement>
        where TDto : AdditionalAgreementDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        public long ContractorId { get; set; }
        [LocalizedRequired]
        public int ApplicationTypeId { get; set; }
        [LocalizedRequired]
        public long MemshipContractId { get; set; }
        public decimal? BaseFixedMinimumValue { get; set; }
        [LocalizedRequired]
        public bool CanPayDivided { get; set; }
        public string Details { get; set; }
        public decimal Amount { get; set; } = 0;
        public override AdditionalAgreement CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            return entity;
        }
        public override void UpdateEntity(AdditionalAgreement entity)
        {
            entity.StatusId = StatusIdConst.MODIFIED;
            base.UpdateEntity(entity);
        }
    }
}
