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
    public class TaxBenefitTypeDlDto<TDto> : EntityDto<TDto, TaxBenefitType>
        where TDto : TaxBenefitTypeDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [Precision(18, 2)]
        public decimal? Factor { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(350)]
        public string NormativeDoc { get; set; }
        public DateOnly StartOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly? EndOn { get; set; }
        public int MinimumValueTypeId { get; set; }
        public int StateId { get; set; }
        public List<TaxBenefitTypeTranslateDlDto> Translates { get; set; }

        protected override Action<IMappingExpression<TDto, TaxBenefitType>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override TaxBenefitType CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(TaxBenefitType entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
