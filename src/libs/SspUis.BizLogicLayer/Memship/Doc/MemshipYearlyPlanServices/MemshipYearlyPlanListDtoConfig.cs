using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipYearlyPlanListDtoConfig : PerDtoConfig<MemshipYearlyPlanListDto, MemshipYearlyPlan>
{
    public override Action<IMappingExpression<MemshipYearlyPlan, MemshipYearlyPlanListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
                       .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipYearlyPlanCancel) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CANCELED, null)
                        : false))
              .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipYearlyPlanAccept) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ACCEPTED, null)
                        : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipYearlyPlanEdit) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipYearlyPlanDelete) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.DELETED, null)
                        : false));
}
