using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSendStudyListDtoConfig : PerDtoConfig<EmployeeSendStudyListDto, EmployeeSendStudy>
{
    public override Action<IMappingExpression<EmployeeSendStudy, EmployeeSendStudyListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(
    dest => dest.University,
    opt => opt.MapFrom(src => string.Join(", ", src.Tables.Select(a => a.University)))
)
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
                       .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeSendStudyCancel) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CANCELED, null)
                        : false))
        .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeLeaveOrderSign) && StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.ACCEPTED, null) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
              .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeSendStudyAccept) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ACCEPTED, null)
                        : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeSendStudyEdit) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeSendStudyDelete) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.DELETED, null)
                        : false));
}
