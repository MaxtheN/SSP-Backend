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

namespace SspUis.BizLogicLayer.Hrm.SettlementAccountSourceServices;

public class SettlementAccountSourceTranslateDto : SettlementAccountSourceTranslateDlDto, ILinkToEntity<SettlementAccountSourceTranslate>
{
    public string Language { get; set; }
}
public class SettlementAccountSourceTranslateDtoConfig : PerDtoConfig<SettlementAccountSourceTranslateDto, SettlementAccountSourceTranslate>
{
    public override Action<IMappingExpression<SettlementAccountSourceTranslate, SettlementAccountSourceTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<SettlementAccountSourceTranslate, SettlementAccountSourceTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
