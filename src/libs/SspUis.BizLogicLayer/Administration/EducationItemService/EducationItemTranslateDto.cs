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

namespace SspUis.BizLogicLayer;

public class EducationItemTranslateDto : EducationItemTranslateDlDto, ILinkToEntity<EducationItemTranslate>
{
    public string Language { get; set; }
}
public class EducationItemTranslateDtoConfig : PerDtoConfig<EducationItemTranslateDto, EducationItemTranslate>
{
    public override Action<IMappingExpression<EducationItemTranslate, EducationItemTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<EducationItemTranslate, EducationItemTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
