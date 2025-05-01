using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public class DualApplicationListDtoConfig : PerDtoConfig<DualApplicationListDto, DualApplication>
    {
        public override Action<IMappingExpression<DualApplication, DualApplicationListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Application.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.Status.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Application.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Application.Contractor.Inn))
                .ForMember(x => x.DocNumber, x => x.MapFrom(ent => ent.Application.DocNumber))
                .ForMember(x => x.DocOn, x => x.MapFrom(ent => ent.Application.DocOn))

				.ForMember(x => x.ContractorPhoneNumber, x => x.MapFrom(ent => ent.Application.Contractor.BusinessmanUserInContractors.FirstOrDefault(a => a.BusinessmanUserId == ent.CreatedUserId).BusinessmanUser.UserName))
                .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Application.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.Region.FullName))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.Application.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.District.FullName))
                .ForMember(x => x.OkedCode, x => x.MapFrom(ent => ent.Application.Contractor.Oked.Code))
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Application.Contractor.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.Contractor.Oked.FullName))
                .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.ACCEPTED)
                        : StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.ACCEPTED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.DualApplicationAccept)))

                .ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.REJECTED)
                        : StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.REJECTED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.DualApplicationReject)))

                .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.CANCELED)
                        : StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.CANCELED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.DualApplicationCancel)))

                .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.MODIFIED)
                        : false))

                .ForMember(x => x.CanSend, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.SENT)
                        : false))

                .ForMember(x => x.CanRevoke, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.REVOKED)
                        : false))

                .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanDualApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.DELETED)
                        : false))
                ;
    }
}
