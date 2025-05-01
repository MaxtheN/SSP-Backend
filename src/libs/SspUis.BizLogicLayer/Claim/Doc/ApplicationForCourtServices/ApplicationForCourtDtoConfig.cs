using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.Claim;

public class ApplicationForCourtDtoConfig : PerDtoConfig<ApplicationForCourtDto, ApplicationForCourt>
{
    public override Action<IMappingExpression<ApplicationForCourt, ApplicationForCourtDto>> AlterReadMapping =>
        cfg=>cfg
            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
            .ForMember(x => x.Step, x => x.MapFrom(ent => ent.Step.Translates.AsQueryable()
                    .FirstOrDefault(ApplicationTypeStepTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Step.FullName))
            .ForMember(x => x.ClaimOrganization, x => x.MapFrom(ent => ent.ClaimOrganization.Translates.AsQueryable()
                .FirstOrDefault(ClaimOrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ClaimOrganization.FullName))
            .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
            .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.Translates.AsQueryable()
                    .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Position.FullName))
            .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files))
            .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName))
            .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
            .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ApplicationForCourtCancel) != null ? true : false));
}