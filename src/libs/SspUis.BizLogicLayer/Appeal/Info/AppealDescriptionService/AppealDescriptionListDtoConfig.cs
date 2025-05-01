using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;

namespace SspUis.BizLogicLayer.AppealDescriptionServices;

public class AppealDescriptionListDtoConfig : PerDtoConfig<AppealDescriptionListDto, AppealDescription>
{
    public override Action<IMappingExpression<AppealDescription, AppealDescriptionListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
               .ForMember(x => x.Parent, x => x.MapFrom(ent => ent.Parent.FullName))
               .ForMember(x => x.HasParent, x => x.MapFrom(ent => ent.HasParent))
    ;
}
