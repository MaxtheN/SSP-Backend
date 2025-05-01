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

public class StateAwardTranslateDto : StateAwardTranslateDlDto, ILinkToEntity<StateAwardsTranslate>
{
    public string Language { get; set; }
}
public class StateAwardTranslateDtoConfig : PerDtoConfig<StateAwardTranslateDto, StateAwardsTranslate>
{
    public override Action<IMappingExpression<StateAwardsTranslate, StateAwardTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<StateAwardsTranslate, StateAwardTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
