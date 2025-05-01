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

public class PositionTypeTranslateDto : PositionTypeTranslateDlDto, ILinkToEntity<PositionTypeTranslate>
{
    public string Language { get; set; }
}
public class PositionTypeTranslateDtoConfig : PerDtoConfig<PositionTypeTranslateDto, PositionTypeTranslate>
{
    public override Action<IMappingExpression<PositionTypeTranslate, PositionTypeTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<PositionTypeTranslate, PositionTypeTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
