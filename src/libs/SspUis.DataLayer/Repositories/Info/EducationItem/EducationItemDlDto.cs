using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class EducationItemDlDto<TDto> : EntityDto<TDto, EducationItem>
        where TDto : EducationItemDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public List<EducationItemTranslateDlDto> Translates { get; set; } = new List<EducationItemTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, EducationItem>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override EducationItem CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(EducationItem entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
