using AutoMapper;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceContractGroupDlDto : EntityDto<ServiceContractGroupDlDto, ServiceContractGroup>,
        IHaveIdProp<long>, ILinkToEntity<ServiceContractGroup>
    {
        public long Id { get; set; }
        public int? GroupId { get; set; }
        public List<ServiceContractTableDlDto> Tables { get; set; } = new();

        protected override Action<IMappingExpression<ServiceContractGroupDlDto, ServiceContractGroup>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Tables, opt => opt.Ignore());

        public override ServiceContractGroup CreateEntity()
        {
            var entity = base.CreateEntity();
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(ServiceContractGroup entity)
        {
            base.UpdateEntity(entity);
            Tables.ApplyChangesTo<long, ServiceContractTableDlDto, ServiceContractTable>(entity.Tables);
        }
    }
}
