using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationDtoConfig : PerDtoConfig<SrvApplicationDto, ServiceApplication>
    {
        public override Action<IMappingExpression<ServiceApplication, SrvApplicationDto>> AlterReadMapping =>
            cfg => cfg

                .ForMember(x => x.Region, x => x.MapFrom(e => e.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Region.FullName))

                 .ForMember(x => x.Organization, x => x.MapFrom(e => e.Organization.FullName))
                 .ForMember(x => x.ToRegionalOffice, x => x.MapFrom(e => e.ToRegionalOffice))

                .ForMember(x => x.District, x => x.MapFrom(e => e.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.District.FullName))

                .ForMember(x => x.CanSend, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.SENT)
                        : false))

                .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.MODIFIED)
                        : false))

                .ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? (StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.CANCELED))
                        : false))
                //.ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                //        ? false
                //        : (StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.REJECTED)
                //            && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServiceApplicationCancel))))

                .ForMember(x => x.CanReceived, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                            ? false
                            : (StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.RECEIVED)
                                && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServiceApplicationReceived))))

                .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? ent.Application.StatusId == StatusIdConst.RECEIVED
                        : (StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.ACCEPTED)
                            && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServiceApplicationAccept))))

                .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : (StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.REJECTED)
                            && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServiceApplicationCancel))))
                //.ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                //        ? (StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.CANCELED))
                //        : false))

                .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : (StatusIdConst.CanApplySrvApplicationStatus(ent.Application.StatusId, StatusIdConst.DELETED)
                            && ServiceProvider.AuthService.HasPermission(ModuleCode.SrvServiceApplicationDelete))))

                .ForMember(x => x.CanCreateContract, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? ent.Application.StatusId == StatusIdConst.ACCEPTED
                        : ent.Application.StatusId == StatusIdConst.ACCEPTED))
                ;
    }
}
