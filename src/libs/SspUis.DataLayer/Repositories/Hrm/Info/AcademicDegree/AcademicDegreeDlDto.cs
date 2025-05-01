using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AcademicDegreeDlDto<TDto> : EntityDto<TDto, AcademicDegree>
        where TDto : AcademicDegreeDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public List<AcademicDegreeTranslateDlDto> Translates { get; set; } = new List<AcademicDegreeTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, AcademicDegree>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override AcademicDegree CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(AcademicDegree entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
