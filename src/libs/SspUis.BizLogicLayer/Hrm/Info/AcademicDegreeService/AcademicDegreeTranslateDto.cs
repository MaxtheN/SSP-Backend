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

public class AcademicDegreeTranslateDto : AcademicDegreeTranslateDlDto, ILinkToEntity<AcademicDegreeTranslate>
{
    public string Language { get; set; }
}
public class AcademicDegreeTranslateDtoConfig : PerDtoConfig<AcademicDegreeTranslateDto, AcademicDegreeTranslate>
{
    public override Action<IMappingExpression<AcademicDegreeTranslate, AcademicDegreeTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<AcademicDegreeTranslate, AcademicDegreeTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
