using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceApplicationDlDto<TDto> : BaseApplicationDlDto<TDto, ServiceApplication>
    where TDto : ServiceApplicationDlDto<TDto>
    {
        [LocalizedRequired]
        public bool IsFree { get; set; } = false;

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int RegionId { get; set; }

        public int? DistrictId { get; set; }
        public bool ToRegionalOffice { get; set; }
        public string? Message { get; set; }
        public List<ServiceApplicationGroupDlDto> Groups { get; set; } = new();

        protected override Action<IMappingExpression<TDto, ServiceApplication>> AlterMapping =>
            cfg =>
            {
                base.AlterMapping(cfg);
                cfg.ForMember(x => x.Groups, x => x.Ignore());
            };

        public override ServiceApplication CreateEntity()
        {
            ServiceApplication entity = base.CreateEntity();
            entity.Application.StatusId = StatusIdConst.CREATED;
            entity.Application.ApplicationTypeId = ApplicationTypeIdConst.SERVICE;
            Groups.AddTo(entity.Groups);
            return entity;
        }

        public override void UpdateEntity(ServiceApplication entity)
        {
            base.UpdateEntity(entity);
            entity.Application.StatusId = StatusIdConst.MODIFIED;
            Groups.ApplyChangesTo<long, ServiceApplicationGroupDlDto, ServiceApplicationGroup>(entity.Groups);
        }
    }
}
