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

public class MilitaryRankTranslateDto : MilitaryRankTranslateDlDto, ILinkToEntity<MilitaryRankTranslate>
{
    public string Language { get; set; }
}
public class MilitaryRankTranslateDtoConfig : PerDtoConfig<MilitaryRankTranslateDto, MilitaryRankTranslate>
{
    public override Action<IMappingExpression<MilitaryRankTranslate, MilitaryRankTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<MilitaryRankTranslate, MilitaryRankTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
