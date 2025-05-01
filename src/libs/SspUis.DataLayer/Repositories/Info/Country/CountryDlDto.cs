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
    public class CountryDlDto<TDto> : EntityDto<TDto, Country>
        where TDto : CountryDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string TextCode { get; set; }
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }
        public List<CountryTranslateDlDto> Translates { get; set; } = new List<CountryTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Country>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Country CreateEntity()
        {
            ShortName = FullName;
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Country entity)
        {
            ShortName = FullName;
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }

    }
}
