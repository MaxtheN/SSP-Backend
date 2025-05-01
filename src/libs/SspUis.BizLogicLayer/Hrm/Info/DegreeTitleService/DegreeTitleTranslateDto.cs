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

public class DegreeTitleTranslateDto : DegreeTitleTranslateDlDto, ILinkToEntity<DegreeTitleTranslate>
{
    public string Language { get; set; }
}
public class DegreeTitleTranslateDtoConfig : PerDtoConfig<DegreeTitleTranslateDto, DegreeTitleTranslate>
{
    public override Action<IMappingExpression<DegreeTitleTranslate, DegreeTitleTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<DegreeTitleTranslate, DegreeTitleTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
