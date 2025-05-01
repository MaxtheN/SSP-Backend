using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{ 
    public class BusinessmanUserContractorLogDlDto
        : EntityDto<BusinessmanUserContractorLogDlDto, BusinessmanUserContractorLog>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int BusinessmanUserId { get; set; }

        [LocalizedRequired]
        public DateTime DateOfExpire { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(100)]
        public string UniqueKey { get; set; }

        public long? ContractorId { get; set; }

        public override BusinessmanUserContractorLog CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.DateAt = DateTime.Now;
            return entity;
        }
    }
}
