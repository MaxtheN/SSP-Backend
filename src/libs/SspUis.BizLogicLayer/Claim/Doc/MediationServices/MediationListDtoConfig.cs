using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using AutoMapper;
using GenericServices.Configuration;
using OpenXmlPowerTools;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.Claim;

public class MediationListDtoConfig : PerDtoConfig<MediationListDto, Mediation>
{
    public override Action<IMappingExpression<Mediation, MediationListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
            .ForMember(x => x.MediationResult, x => x.MapFrom(ent => ent.MediationResult.Translates.AsQueryable()
                .FirstOrDefault(MediationResultTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.MediationResult.FullName))
            .ForMember(x => x.ClaimNeedCourt, x => x.MapFrom(ent => ent.ClaimNeedCourt.Translates.AsQueryable()
                .FirstOrDefault(ClaimNeedCourtTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ClaimNeedCourt.FullName))
            .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
            .ForMember(x => x.MediationPlanDocNumber, x => x.MapFrom(ent => ent.MediationPlan.DocNumber))
            .ForMember(d => d.CanCancel, c => c.MapFrom(e => ServiceProvider.AuthService.Contractor == null && e.StatusId == StatusIdConst.ACCEPTED))
            .ForMember(d => d.CanEdit, c => c.MapFrom(e => ServiceProvider.AuthService.Contractor == null
                    && e.MediationResultId != MediationResultIdConst.GIVEN_TIME
                    && e.ClaimNeedCourtId != ClaimNeedCourtIdConst.REVIEWING))
            //.ForMember(d => d.CanViewApplicationForCourt, c => c.MapFrom(e => ServiceProvider.AuthService.Contractor == null
            //    && e.MediationResultId == MediationResultIdConst.NO_AGREEMANT_REACHED
            //    && e.ClaimNeedCourtId == ClaimNeedCourtIdConst.ACCEPTING
            //    && e.StatusId == StatusIdConst.ACCEPTED))
            .ForMember(d => d.CanCreateApplicationForCourt, c => c.MapFrom(ent => ent.ApplicationForCourts == null))

            .ForMember(d => d.CanAccept, c => c.MapFrom(e => ServiceProvider.AuthService.Contractor == null
                && (e.StatusId == StatusIdConst.CREATED || e.StatusId == StatusIdConst.MODIFIED)))

            .ForMember(d => d.EmployeeManageId, c => c.MapFrom(e => e.MediationPlan.Application.ClaimApplication.EmployeeManageId))
            .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.MediationPlan.Application.ClaimApplication.EmployeeManageId != null
                ? ent.MediationPlan.Application.ClaimApplication.EmployeeManage.Employee.Person.FullName : null));
}
