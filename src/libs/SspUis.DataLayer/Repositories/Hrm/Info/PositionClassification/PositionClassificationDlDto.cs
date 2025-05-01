using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class PositionClassificationDlDto<TDto> : EntityDto<TDto, PositionClassification>
        where TDto : PositionClassificationDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string PnRu { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string NumRu { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string ClassRu { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string NskzCodeRu { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string CategoryCodeRu { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string RangeCodeRu { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string MinedCodeRu { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string SpecialityCodeRu { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string TypeCodeRu { get; set; }
        public List<PositionClassificationTranslateDlDto> Translates { get; set; } = new List<PositionClassificationTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, PositionClassification>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override PositionClassification CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(PositionClassification entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
