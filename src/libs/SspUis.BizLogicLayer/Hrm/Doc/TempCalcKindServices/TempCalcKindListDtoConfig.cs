using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class TempCalcKindListDtoConfig : PerDtoConfig<TempCalcKindListDto, TempCalcKind>
{
    public override Action<IMappingExpression<TempCalcKind, TempCalcKindListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
               .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(d => d.CalculationKind, c => c.MapFrom(e => e.CalculationKind.Translates.AsQueryable()
                .FirstOrDefault(CalculationKindTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.CalculationKind.FullName))
                .ForMember(d => d.TempCalcKindType, c => c.MapFrom(e => e.TempCalcKindType.Translates.AsQueryable()
                .FirstOrDefault(TempCalcKindTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.TempCalcKindType.FullName))
         .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.TempCalcKindCancel) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.CANCELED, null)
                        : false))
               .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.TempCalcKindSign) && StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.ACCEPTED, null) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.TempCalcKindEdit) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.TempCalcKindDelete) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.DELETED, null)
                        : false))
        ;
}
