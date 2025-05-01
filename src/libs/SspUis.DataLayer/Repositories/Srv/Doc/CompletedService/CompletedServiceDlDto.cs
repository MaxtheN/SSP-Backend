using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CompletedServiceDlDto<TDto> : EntityDto<TDto, CompletedService>
        where TDto : CompletedServiceDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(30)]
        public string DocNumber { get; set; }

        [LocalizedRequired]
        public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [LocalizedStringLength(600)]
        public string Details { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ContractorId { get; set; }

        [LocalizedRange(1, long.MaxValue)]
        public long? ServiceContractId { get; set; }

        [LocalizedRange(1, long.MaxValue)]
        public long? ServiceApplicationId { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long? EmployeeManageId { get; set; }

        public override CompletedService CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            return entity;
        }
        public override void UpdateEntity(CompletedService entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
        }
    }
}
