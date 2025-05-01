using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class DebtDlDto<TDto> : EntityDto<TDto, Debt>
        where TDto : DebtDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }

        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }

        public List<DebtTableDlDto> Tables { get; set; } = new();

        protected override Action<IMappingExpression<TDto, Debt>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Tables, c => c.Ignore());

        public override Debt CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(Debt entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
            Tables.ApplyChangesTo<long, DebtTableDlDto, DebtTable>(entity.Tables);
        }
    }
}
