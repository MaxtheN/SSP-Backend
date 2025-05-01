using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.MemshipApplicationServices
{
    public class MemshipApplicationListDtoConfig : PerDtoConfig<MemshipApplicationListDto, MemshipApplication>
    {
        public override Action<IMappingExpression<MemshipApplication, MemshipApplicationListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.OkedCode, x => x.MapFrom(ent => ent.Application.Contractor.Oked.Code))
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Application.Contractor.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.Contractor.Oked.FullName))
                .ForMember(x => x.ContractorActivityType, x => x.MapFrom(ent => ent.Application.MemshipApplication.ContractorActivityType.FullName))
                .ForMember(x => x.ContractorCategory, x => x.MapFrom(ent => ent.Application.MemshipApplication.ContractorCategory.Translates.AsQueryable()
                    .FirstOrDefault(ContractorCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.MemshipApplication.ContractorCategory.FullName))
                .ForMember(x => x.ChoosedRegion, x => x.MapFrom(ent => ent.ChoosedRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ChoosedRegion.FullName))
                .ForMember(x => x.ChoosedDistrict, x => x.MapFrom(ent => ent.ChoosedDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ChoosedDistrict.FullName))
                .ForMember(x => x.YearlyEarnings, x => x.MapFrom(ent => ent.Application.MemshipApplication.YearlyEarnings))
                .ForMember(x => x.YearlyTaxes, x => x.MapFrom(ent => ent.Application.MemshipApplication.YearlyTaxes))
                .ForMember(x => x.YearlyExport, x => x.MapFrom(ent => ent.Application.MemshipApplication.YearlyExport))
                .ForMember(x => x.IsRead, x => x.MapFrom(ent => ent.IsRead))
                .ForMember(x => x.YearlyImport, x => x.MapFrom(ent => ent.Application.MemshipApplication.YearlyImport))
                .ForMember(x => x.YearlyManufacture, x => x.MapFrom(ent => ent.Application.MemshipApplication.YearlyManufacture))
                .ForMember(x => x.OpfId, x => x.MapFrom(ent => ent.Application.Contractor.OpfId))
                .ForMember(x => x.Opf, x => x.MapFrom(ent => ent.Application.Contractor.Opf.Translates.AsQueryable()
                    .FirstOrDefault(OpfTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.Contractor.Opf.FullName))
                .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.ACCEPTED)
                        : StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.ACCEPTED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipApplicationAccept)))

                //.ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                //        ? StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.REJECTED)
                //        : StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.REJECTED)
                //                  && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipApplicationReject)))

                .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.CANCELED)
                        : StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.CANCELED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipApplicationCancel)))

                .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.MODIFIED)
                        : false))

                .ForMember(x => x.CanSend, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.SENT)
                        : false))

                .ForMember(x => x.CanRevoke, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.REVOKED)
                        : false))

                .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, StatusIdConst.DELETED)
                        : false))
                ;
    }
}
