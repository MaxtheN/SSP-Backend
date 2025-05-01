using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices
{
    public class ExecutionApplicationListDtoConfig : PerDtoConfig<ExecutionApplicationListDto, ExecutionApplication>
    {
        public override Action<IMappingExpression<ExecutionApplication, ExecutionApplicationListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
               .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Status.FullName))
            .ForMember(d => d.Region, c => c.MapFrom(e => e.Region.Translates.AsQueryable()
               .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Region.FullName))
            .ForMember(d => d.District, c => c.MapFrom(e => e.District.Translates.AsQueryable()
               .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.District.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
               .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Organization.FullName))

             .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ExecutionApplicationCancel) != null ? StatusIdConst.CanExecutionApplicationApplyStatus(ent.StatusId, StatusIdConst.NOT_ACCEPTED, null) : false))
               .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ExecutionApplicationSign) && StatusIdConst.CanExecutionApplicationApplyStatus(ent.StatusId, StatusIdConst.SIGNED, null) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
            .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ExecutionApplicationAccept) && StatusIdConst.CanExecutionApplicationApplyStatus(ent.StatusId, StatusIdConst.ACCEPTED, null) && ent.StatusId != StatusIdConst.ACCEPTED ? true : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ExecutionApplicationEdit) != null ? StatusIdConst.CanExecutionApplicationApplyStatus(ent.StatusId, StatusIdConst.MODIFIED, null) : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ExecutionApplicationDelete) != null ? StatusIdConst.CanExecutionApplicationApplyStatus(ent.StatusId, StatusIdConst.DELETED, null) : false))
            ;
    }
}
