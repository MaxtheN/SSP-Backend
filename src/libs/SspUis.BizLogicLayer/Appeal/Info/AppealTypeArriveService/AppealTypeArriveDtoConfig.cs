using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;

namespace SspUis.BizLogicLayer.AppealTypeArriveServices;

public class AppealTypeArriveDtoConfig : PerDtoConfig<AppealTypeArriveDto, AppealTypeArrive>
{
    public override Action<IMappingExpression<AppealTypeArrive, AppealTypeArriveDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
    ;
}
