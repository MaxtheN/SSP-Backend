using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class TariffScaleCoefDlDto<TDto> : EntityDto<TDto, TariffScaleCoef>
        where TDto : TariffScaleCoefDlDto<TDto>
    {

        [LocalizedRequired]
        public DateOnly DateOn { get; set; } = DateTime.Today.AsDateOnly();
        [LocalizedRequired]
        public int TariffScaleId { get; set; }
        public List<TariffScaleCoefTranslateDlDto> Translates { get; set; }
        public List<TariffScaleCoefTableDlDto> Tables { get; set; }

        protected override Action<IMappingExpression<TDto, TariffScaleCoef>> AlterMapping => cfg => cfg
            .ForMember(x => x.Tables, x => x.Ignore());

        public override TariffScaleCoef CreateEntity()
        {
            var entity = base.CreateEntity();
            Tables.AddTo(entity.Tables);
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(TariffScaleCoef entity)
        {
            base.UpdateEntity(entity);
            Tables.ApplyChangesTo<int, TariffScaleCoefTableDlDto, TariffScaleCoefTable>(entity.Tables);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
