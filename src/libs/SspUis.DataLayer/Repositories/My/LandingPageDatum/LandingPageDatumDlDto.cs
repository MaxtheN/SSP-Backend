using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class LandingPageDatumDlDto<TDto> : EntityDto<TDto, LandingPageDatum>
        where TDto : LandingPageDatumDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public List<LandingPageDatumTranslateDlDto> Translates { get; set; } = new List<LandingPageDatumTranslateDlDto>();
        protected override Action<IMappingExpression<TDto, LandingPageDatum>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());
        public override LandingPageDatum CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }
        public override void UpdateEntity(LandingPageDatum entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
