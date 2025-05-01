using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class StaffingIndicatorDlDto<TDto> : EntityDto<TDto,StaffingIndicator>
        where TDto : StaffingIndicatorDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; } = null!;
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; } = null!;
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; } = null!;
        [LocalizedRequired]
        public DateOnly StartOn { get; set; } = DateTime.Today.AsDateOnly();
        public DateOnly? EndOn { get; set; }
        public List<StaffingIndicatorTranslateDlDto> Translates { get; set; } = new List<StaffingIndicatorTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, StaffingIndicator>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());
        public override StaffingIndicator CreateEntity()
        {
            var entity = base.CreateEntity();
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(StaffingIndicator entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
