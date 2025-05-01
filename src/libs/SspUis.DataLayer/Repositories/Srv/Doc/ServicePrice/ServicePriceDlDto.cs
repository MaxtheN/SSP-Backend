using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ServicePriceDlDtoo<TDto> : EntityDto<TDto, ServicePrice>
        where TDto : ServicePriceDlDtoo<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(30)]
        public string DocNumber { get; set; }

        [LocalizedRequired]
        public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [LocalizedStringLength(600)]
        public string Details { get; set; }

        public List<ServicePriceGroupDlDto> Groups { get; set; } = new();

        protected override Action<IMappingExpression<TDto, ServicePrice>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Groups, opt => opt.Ignore());

        public override ServicePrice CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            Groups.AddTo(entity.Groups);
            return entity;
        }

        public override void UpdateEntity(ServicePrice entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
            Groups.ApplyChangesTo<long, ServicePriceGroupDlDto, ServicePriceGroup>(entity.Groups);
        }
    }
}
