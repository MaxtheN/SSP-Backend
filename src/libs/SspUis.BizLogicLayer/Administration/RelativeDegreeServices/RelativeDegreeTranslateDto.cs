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

namespace SspUis.BizLogicLayer.RelativeDegreeServices
{
    public class RelativeDegreeTranslateDto : RelativeDegreeTranslateDlDto, ILinkToEntity<RelativeDegreeTranslate>
    {
        public string Language { get; set; }
    }
    public class RelativeDegreeTranslateDtoConfig : PerDtoConfig<RelativeDegreeTranslateDto, RelativeDegreeTranslate>
    {
        public override Action<IMappingExpression<RelativeDegreeTranslate, RelativeDegreeTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<RelativeDegreeTranslate, RelativeDegreeTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
