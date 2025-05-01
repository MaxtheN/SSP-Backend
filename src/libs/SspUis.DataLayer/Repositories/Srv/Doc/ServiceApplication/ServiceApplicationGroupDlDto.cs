using AutoMapper;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceApplicationGroupDlDto : EntityDto<ServiceApplicationGroupDlDto, ServiceApplicationGroup>,
        IHaveIdProp<long>, ILinkToEntity<ServiceApplicationGroup>
    {
        public long Id { get; set; }
        public int? GroupId { get; set; }
        public List<ServiceApplicationTableDlDto> Tables { get; set; } = new();

        protected override Action<IMappingExpression<ServiceApplicationGroupDlDto, ServiceApplicationGroup>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Tables, opt => opt.Ignore());

        public override ServiceApplicationGroup CreateEntity()
        {
            var entity = base.CreateEntity();
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(ServiceApplicationGroup entity)
        {
            base.UpdateEntity(entity);
            Tables.ApplyChangesTo<long, ServiceApplicationTableDlDto, ServiceApplicationTable>(entity.Tables);
        }
    }
}
