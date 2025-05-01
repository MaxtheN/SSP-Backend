using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class PlannedCalculationListDtoConfig  :PerDtoConfig<PlannedCalculationListDto, PlannedCalculation>
{
    public override Action<IMappingExpression<PlannedCalculation, PlannedCalculationListDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
          .ForMember(d => d.CalculationKind, c => c.MapFrom(e => e.CalculationKind.Translates.AsQueryable()
                .FirstOrDefault(CalculationKindTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.CalculationKind.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
              //         .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.PlannedCalculationCancel) != null
              //          ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CANCELED, null)
              //          : false))
              //.ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.PlannedCalculationAccept) != null
              //          ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ACCEPTED, null)
              //          : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.PlannedCalculationEdit) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.PlannedCalculationDelete) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.DELETED, null)
                        : false));
}
