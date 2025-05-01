using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class ClaimApplicationListDtoConfig : PerDtoConfig<ClaimApplicationListDto, ClaimApplication>
    {
        public override Action<IMappingExpression<ClaimApplication, ClaimApplicationListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ClaimApplicationType, x => x.MapFrom(ent => ent.ClaimApplicationType.Translates.AsQueryable()
                    .FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.ClaimApplicationType.FullName))

                .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManageId != null ? ent.EmployeeManage.Employee.Person.FullName : null))
                .ForMember(x => x.ClaimResponsibleTypeId, x => x.MapFrom(ent => ent.Tables.Select(a => a.ClaimResponsibleTypeId).FirstOrDefault()))

                .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable()
                    .FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Currency.FullName))

                //.ForMember(x=>x.DurationGivenPerformer , x=>x.MapFrom(ent => ent.DurationGivenPerformer))
                //.ForMember(x=>x.OrganizationId , x=>x.MapFrom(ent=>ent.OrganizationId))
                .ForMember(q=>q.OrganizationName, t=>t.MapFrom(ent => ent.Organization.ShortName))

                .ForMember(x => x.ClaimTheme, x => x.MapFrom(ent => ent.ClaimTheme.Translates.AsQueryable()
                    .FirstOrDefault(ClaimThemeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.ClaimTheme.FullName))
                .ForMember(x => x.InnOrPinfl, x => x.MapFrom(ent => ent.Tables.FirstOrDefault().InnOrPinfl ?? "")) 
            ;
    }
}
