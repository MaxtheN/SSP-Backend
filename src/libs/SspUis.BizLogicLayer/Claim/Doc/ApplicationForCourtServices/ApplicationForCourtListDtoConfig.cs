using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.Claim;

public class ApplicationForCourtListDtoConfig : PerDtoConfig<ApplicationForCourtListDto, ApplicationForCourt>
{
    public override Action<IMappingExpression<ApplicationForCourt, ApplicationForCourtListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
            .ForMember(x => x.ClaimOrganization, x => x.MapFrom(ent => ent.ClaimOrganization.Translates.AsQueryable()
                .FirstOrDefault(ClaimOrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ClaimOrganization.FullName))
            .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
            .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
            .ForMember(x => x.EmployeeManageId, x => x.MapFrom(ent => ent.EmployeeManageId))
            .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName))
            .ForMember(x => x.DepartmentId, x => x.MapFrom(ent => ent.DepartmentId))
            .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.Step, x => x.MapFrom(ent => ent.Step.Translates.AsQueryable()

                .FirstOrDefault(ApplicationTypeStepTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Step.FullName))

            .ForMember(d => d.ClaimApplicationEmployeeManageId, c => c.MapFrom(e => e.Mediation.MediationPlan.Application.ClaimApplication.EmployeeManageId))

            .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.Mediation.MediationPlan.Application.ClaimApplication.EmployeeManageId != null
                ? ent.Mediation.MediationPlan.Application.ClaimApplication.EmployeeManage.Employee.Person.FullName : null))

            .ForMember(d => d.CanAccept, c => c.MapFrom(e => e.StepId == StepIdConst.APPLICATION_FOR_COURT_CREATE))

             .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ApplicationForCourtCancel) != null ? StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.NOT_ACCEPTED) && ent.StepId == StepIdConst.ACCEPT : false))

          .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.SrvApplicationYearlyPlanEdit) != null ? StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.MODIFIED) && ent.StepId == StepIdConst.APPLICATION_FOR_COURT_CREATE : false))

        .ForMember(x => x.CanSend, x => x.MapFrom(ent => StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.SENT)
                                                  && ent.StepId == StepIdConst.APPLICATION_FOR_COURT_CREATE))
        .ForMember(x => x.CanReject, x => x.MapFrom(ent => StatusIdConst.CanApplicationForCourtApplyStatus(ent.StatusId, StatusIdConst.REJECTED)
                                                  && ent.StepId == StepIdConst.ACCEPT_THAT_EMPLOYEE));
}
