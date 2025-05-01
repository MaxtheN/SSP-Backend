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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class ScientificDegreeTranslateDto : ScientificDegreeTranslateDlDto, ILinkToEntity<ScientificDegreeTranslate>
{
    public string Language { get; set; }
}
public class ScientificDegreeTranslateDtoConfig : PerDtoConfig<ScientificDegreeTranslateDto, ScientificDegreeTranslate>
{
    public override Action<IMappingExpression<ScientificDegreeTranslate, ScientificDegreeTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<ScientificDegreeTranslate, ScientificDegreeTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
