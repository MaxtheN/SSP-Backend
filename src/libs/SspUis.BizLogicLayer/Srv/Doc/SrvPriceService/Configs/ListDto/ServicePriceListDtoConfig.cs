using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceListDtoConfig : PerDtoConfig<ServicePriceListDto, ServicePrice>
    {
        public override Action<IMappingExpression<ServicePrice, ServicePriceListDto>> AlterReadMapping =>
            cfg => cfg
            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? ent.Status.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? ent.Organization.FullName))

            .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                ? false
                : StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.ACCEPTED)
                          && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServicePriceAccept)))

            .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                    ? false
                    : StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.CANCELED)
                              && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServicePriceCancel)))

            .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                    ? false
                    : StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.MODIFIED)
                        && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServicePriceEdit)))

            .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                    ? false
                    : StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.DELETED)
                        && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServicePriceDelete)))
        ;
    }
}
