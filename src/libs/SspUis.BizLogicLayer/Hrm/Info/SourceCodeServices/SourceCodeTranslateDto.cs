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

namespace SspUis.BizLogicLayer.Hrm.SourceCodeServices;

public class SourceCodeTranslateDto : SourceCodeTranslateDlDto, ILinkToEntity<SourceCodeTranslate>
{
    public string Language { get; set; }
}
public class SourceCodeTranslateDtoConfig : PerDtoConfig<SourceCodeTranslateDto, SourceCodeTranslate>
{
    public override Action<IMappingExpression<SourceCodeTranslate, SourceCodeTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<SourceCodeTranslate, SourceCodeTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
