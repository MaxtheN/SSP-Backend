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

namespace SspUis.BizLogicLayer.CurrencyServices
{
    public class CurrencyTranslateDto : CurrencyTranslateDlDto, ILinkToEntity<CurrencyTranslate>
    {
        public string Language { get; set; }
    }

    public class CurrencyTranslateDtoConfig : PerDtoConfig<CurrencyTranslateDto, CurrencyTranslate>
    {
        public override Action<IMappingExpression<CurrencyTranslate, CurrencyTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<CurrencyTranslate, CurrencyTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }

}
