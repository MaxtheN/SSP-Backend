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

namespace SspUis.BizLogicLayer.CitizenshipServices
{
    public class CitizenshipTranslateDto : CitizenshipTranslateDlDto, ILinkToEntity<CitizenshipTranslate>
    {
        public string Language { get; set; }
    }
    public class CitizenshipTranslateDtoConfig : PerDtoConfig<CitizenshipTranslateDto, CitizenshipTranslate>
    {
        public override Action<IMappingExpression<CitizenshipTranslate, CitizenshipTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<CitizenshipTranslate, CitizenshipTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
