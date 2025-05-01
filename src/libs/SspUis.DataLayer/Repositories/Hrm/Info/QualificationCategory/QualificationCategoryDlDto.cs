using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class QualificationCategoryDlDto<TDto> : EntityDto<TDto,QualificationCategory>
        where TDto : QualificationCategoryDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; } = null!;
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; } = null!;
        [LocalizedStringLength(500)]
        [LocalizedRequired]
        public string FullName { get; set; } = null!;
        [LocalizedStringLength(600)]
        [LocalizedRequired]
        public string Details { get; set; } = null!;

        public List<QualificationCategoryTranslateDlDto> Translates { get; set; } = new();

        protected override Action<IMappingExpression<TDto, QualificationCategory>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());
        public override QualificationCategory CreateEntity()
        {
            var entity = base.CreateEntity();
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(QualificationCategory entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
