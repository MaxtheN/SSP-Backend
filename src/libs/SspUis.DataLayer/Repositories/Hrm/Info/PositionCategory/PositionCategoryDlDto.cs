using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class PositionCategoryDlDto<TDto> : EntityDto<TDto, PositionCategory>
        where TDto : PositionCategoryDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public List<PositionCategoryTranslateDlDto> Translates { get; set; } = new List<PositionCategoryTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, PositionCategory>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override PositionCategory CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(PositionCategory entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
