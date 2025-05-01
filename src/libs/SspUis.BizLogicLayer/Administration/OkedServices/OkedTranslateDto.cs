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

namespace SspUis.BizLogicLayer.OkedServices
{
    public class OkedTranslateDto : OkedTranslateDlDto, ILinkToEntity<OkedTranslate>
    {
        public string Language { get; set; }
    }

    public class OkedTranslateDtoConfig : PerDtoConfig<OkedTranslateDto, OkedTranslate>
    {
        public override Action<IMappingExpression<OkedTranslate, OkedTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<OkedTranslate, OkedTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
