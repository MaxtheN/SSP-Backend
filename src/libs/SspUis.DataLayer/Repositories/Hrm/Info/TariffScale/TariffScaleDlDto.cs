using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class TariffScaleDlDto<TDto> : EntityDto<TDto, TariffScale>
        where TDto : TariffScaleDlDto<TDto>
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
        [LocalizedRequired]
        [LocalizedRange(1,int.MaxValue)]
        public int TariffScaleTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1,int.MaxValue)]
        public int MinimumValueTypeId { get; set; }
        public List<TariffScaleTranslateDlDto> Translates { get; set; }
        public List<TariffScaleTableDlDto> Tables { get; set; }

        protected override Action<IMappingExpression<TDto, TariffScale>> AlterMapping => cfg => cfg
            .ForMember(x => x.Tables, x => x.Ignore());

        public override TariffScale CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(TariffScale entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
            Tables.ApplyChangesTo<int, TariffScaleTableDlDto, TariffScaleTable>(entity.Tables);
        }
    }
}
