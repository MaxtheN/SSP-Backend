using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.SourceCodeServices;

public class SourceCodeListDtoConfig : PerDtoConfig<SourceCodeListDto, SourceCode>
{
    public override Action<IMappingExpression<SourceCode, SourceCodeListDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
            .FirstOrDefault(SourceCodeTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName))
        .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
            .FirstOrDefault(SourceCodeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
        .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        ;
}
