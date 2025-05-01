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

namespace SspUis.BizLogicLayer.Hrm.LevelCodeServices;

public class LevelCodeTranslateDto : LevelCodeTranslateDlDto, ILinkToEntity<LevelCodeTranslate>
{
    public string Language { get; set; }
}
public class LevelCodeTranslateDtoConfig : PerDtoConfig<LevelCodeTranslateDto, LevelCodeTranslate>
{
    public override Action<IMappingExpression<LevelCodeTranslate, LevelCodeTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<LevelCodeTranslate, LevelCodeTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
