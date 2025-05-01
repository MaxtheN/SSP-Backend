using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NeedChamberServiceGroupDlDto<TDto> : EntityDto<TDto, NeedChamberServiceGroup>
        where TDto : NeedChamberServiceGroupDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; } = null!;

        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string ShortName { get; set; } = null!;

        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; } = null!;

        public List<NeedChamberServiceGroupTranslateDlDto> Translates { get; set; } = new();

        protected override Action<IMappingExpression<TDto, NeedChamberServiceGroup>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Translates, x => x.Ignore());

        public override NeedChamberServiceGroup CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(NeedChamberServiceGroup entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
