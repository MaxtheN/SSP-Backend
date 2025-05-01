using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class FixedMinimumValueDlDto<TDto> : EntityDto<TDto, FixedMinimumValue>
        where TDto : FixedMinimumValueDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(350)]
        public string NormativeDoc { get; set; }
        [LocalizedRequired]
        public DateOnly DateOn { get; set; } = DateTime.Today.AsDateOnly();
        [LocalizedRequired]
        [LocalizedRange(1,int.MaxValue)]
        public int MinimumValueTypeId { get; set; }
        [Precision(18, 2)]
        [LocalizedRequired]
        public decimal FixedValue { get; set; }
        [Precision(18, 2)]
        [LocalizedRequired]
        public decimal ChangePercentage { get; set; }


        public override FixedMinimumValue CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            return entity;
        }
    }
}
