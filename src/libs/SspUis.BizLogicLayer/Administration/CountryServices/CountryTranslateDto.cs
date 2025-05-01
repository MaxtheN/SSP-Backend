using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.CountryServices
{
    public class CountryTranslateDto : CountryTranslateDlDto, ILinkToEntity<CountryTranslate>
    {
        public string Language { get; set; }
    }

    public class CountryTranslateDtoConfig : PerDtoConfig<CountryTranslateDto, CountryTranslate>
    {
        public override Action<IMappingExpression<CountryTranslate, CountryTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<CountryTranslate, CountryTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
