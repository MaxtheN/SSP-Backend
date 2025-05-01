using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Memship;

public class DebtDtoConfig : PerDtoConfig<DebtDto, Debt>
{
    public override Action<IMappingExpression<Debt, DebtDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.TotalDebtAmount, x => x.MapFrom(x => x.Tables.Sum(t => t.DebtAmount)))
        .ForMember(x => x.TotalEntitlementAmount, x => x.MapFrom(x => x.Tables.Sum(t => t.EntitlementAmount)))
        .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
        .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
     ;
}
