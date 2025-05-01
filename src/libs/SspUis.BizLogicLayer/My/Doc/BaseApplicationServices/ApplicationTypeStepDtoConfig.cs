using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ApplicationTypeStepDtoConfig : PerDtoConfig<ApplicationTypeStepDto, ApplicationTypeStep>
{
    public override Action<IMappingExpression<ApplicationTypeStep, ApplicationTypeStepDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(x => x.ApplicationType, c => c.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable()
            .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
        .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName));
}
