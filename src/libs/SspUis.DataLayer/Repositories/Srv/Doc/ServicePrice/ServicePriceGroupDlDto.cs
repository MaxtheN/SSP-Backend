using AutoMapper;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ServicePriceGroupDlDto 
        : EntityDto<ServicePriceGroupDlDto, ServicePriceGroup>,
        IHaveIdProp<long>, ILinkToEntity<ServicePriceGroup>
    {
        public long Id { get; set; }
        public int? GroupId { get; set; }
        public List<ServicePriceTableDlDto> Tables { get; set; } = new();

        protected override Action<IMappingExpression<ServicePriceGroupDlDto, ServicePriceGroup>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Tables, opt => opt.Ignore());

        public override ServicePriceGroup CreateEntity()
        {
            var entity = base.CreateEntity();
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(ServicePriceGroup entity)
        {
            base.UpdateEntity(entity);
            Tables.ApplyChangesByStateIdTo<long, ServicePriceTableDlDto, ServicePriceTable>(entity.Tables);
        }
    }
}
