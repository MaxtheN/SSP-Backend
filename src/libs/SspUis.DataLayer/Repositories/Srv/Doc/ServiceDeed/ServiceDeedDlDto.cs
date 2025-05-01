using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceDeedDlDto<TDto> : EntityDto<TDto, ServiceDeed>
        where TDto : ServiceDeedDlDto<TDto>
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
        public long ApplicationId { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ContractorId { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long SrvContractorId { get; set; }
		[IgnoreWordProperty]
		public List<ServiceDeedGroupDlDto> Groups { get; set; } = new();

        protected override Action<IMappingExpression<TDto, ServiceDeed>> AlterMapping => cfg => cfg
               .ForMember(x => x.Groups, x => x.Ignore());

        public override ServiceDeed CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            entity.Id2 = Guid.NewGuid();
            Groups.AddTo(entity.Groups);
            return entity;
        }

        public override void UpdateEntity(ServiceDeed entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
            Groups.ApplyChangesTo<long, ServiceDeedGroupDlDto, ServiceDeedGroup>(entity.Groups);
        }
    }
}
