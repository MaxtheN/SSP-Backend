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

namespace SspUis.BizLogicLayer.LandingPageDatumServices
{
    public class LandingPageDatumTranslateDto : LandingPageDatumTranslateDlDto, ILinkToEntity<LandingPageDatumTranslate>
    {
        public string Language { get; set; }
    }
    public class LandingPageDatumTranslateDtoConfig : PerDtoConfig<LandingPageDatumTranslateDto, LandingPageDatumTranslate>
    {
        public override Action<IMappingExpression<LandingPageDatumTranslate, LandingPageDatumTranslateDto>> AlterReadMapping => cfg => cfg
               .IncludeBase<LandingPageDatumTranslate, LandingPageDatumTranslateDlDto>()
               .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
