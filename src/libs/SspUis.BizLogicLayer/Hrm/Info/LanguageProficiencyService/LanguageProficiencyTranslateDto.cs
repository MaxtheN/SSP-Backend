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

public class LanguageProficiencyTranslateDto : LanguageProficiencyTranslateDlDto, ILinkToEntity<LanguageProficiencyTranslate>
{
    public string Language { get; set; }
}
public class LanguageProficiencyTranslateDtoConfig : PerDtoConfig<LanguageProficiencyTranslateDto, LanguageProficiencyTranslate>
{
    public override Action<IMappingExpression<LanguageProficiencyTranslate, LanguageProficiencyTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<LanguageProficiencyTranslate, LanguageProficiencyTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
