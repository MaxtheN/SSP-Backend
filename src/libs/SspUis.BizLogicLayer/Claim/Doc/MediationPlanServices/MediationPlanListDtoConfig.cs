using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Claim;

public class MediationPlanListDtoConfig : PerDtoConfig<MediationPlanListDto, MediationPlan>
{
    public override Action<IMappingExpression<MediationPlan, MediationPlanListDto>> AlterReadMapping =>
        cfg => cfg

            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate
                    .GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
            ?? e.Status.FullName))

            .ForMember(d => d.MeetingType, c => c.MapFrom(e => e.MeetingType.Translates.AsQueryable()
                .FirstOrDefault(MeetingTypeTranslate
                    .GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
            ?? e.MeetingType.FullName))

            .ForMember(d => d.Contractor, c => c.MapFrom(e => e.Contractor.FullName))

            .ForMember(d => d.EmployeeManageId, c => c.MapFrom(e => e.Application.ClaimApplication.EmployeeManageId))
            .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.Application.ClaimApplication.EmployeeManageId != null
                ? ent.Application.ClaimApplication.EmployeeManage.Employee.Person.FullName : null))

            .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                            ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ACCEPTED, null)
                            : StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ACCEPTED, null)
                                      && ServiceProvider.AuthService.HasPermission(ModuleCode.MediationPlanAccept)))

            .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CANCELED, null)
                        : StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CANCELED, null)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.MediationPlanCancel)))

            //.ForMember(x => x.CanCreateMediation, x => x.MapFrom(ent => !ent.Mediations
            //    .Any(mediation => mediation.StatusId != StatusIdConst.DELETED && mediation.StatusId != StatusIdConst.NOT_ACCEPTED && mediation.StatusId != StatusIdConst.CANCELED)))
            .ForMember(x => x.CanCreateMediation, x => x.MapFrom(ent => !ent.Mediations.Any()))

            .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                            ? false
                            : StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.MODIFIED, null)
                                && ServiceProvider.AuthService.HasPermission(ModuleCode.MediationPlanEdit)))

            .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                            ? false
                            : StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.DELETED, null)
                                && ServiceProvider.AuthService.HasPermission(ModuleCode.MediationPlanDelete)));
}