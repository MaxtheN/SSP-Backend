using AutoMapper;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceDeedGroupDlDto : EntityDto<ServiceDeedGroupDlDto, ServiceDeedGroup>,
        IHaveIdProp<long>, ILinkToEntity<ServiceDeedGroup>
    {
        public long Id { get; set; }
        public int? GroupId { get; set; }
        public List<ServiceDeedTableDlDto> Tables { get; set; } = new();

        protected override Action<IMappingExpression<ServiceDeedGroupDlDto, ServiceDeedGroup>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Tables, opt => opt.Ignore());

        public override ServiceDeedGroup CreateEntity()
        {
            var entity = base.CreateEntity();
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(ServiceDeedGroup entity)
        {
            base.UpdateEntity(entity);
            Tables.ApplyChangesTo<long, ServiceDeedTableDlDto, ServiceDeedTable>(entity.Tables);
        }
    }
}
