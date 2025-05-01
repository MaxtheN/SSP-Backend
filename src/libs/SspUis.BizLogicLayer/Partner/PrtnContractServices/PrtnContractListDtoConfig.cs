using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;
using SspUis.Core;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class PrtnContractListDtoConfig : PerDtoConfig<PrtnContractListDto, PrtnContract>
    {
        public override Action<IMappingExpression<PrtnContract, PrtnContractListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Mfy, x => x.MapFrom(ent => ent.Application.PrtnApplication.Mfy.FullName))
                .ForMember(x => x.MfyId, x => x.MapFrom(ent => ent.Application.PrtnApplication.MfyId))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.IsRead, x => x.MapFrom(ent => ent.IsRead))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.PrtnContractType, x => x.MapFrom(ent => ent.PrtnContractType.Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContractType.FullName))
                .ForMember(x => x.ApplicationDocNumber, x => x.MapFrom(ent => ent.Application.DocNumber))
                .ForMember(x => x.ApplicationDocOn, x => x.MapFrom(ent => ent.Application.DocOn))
                .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                    .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
                .ForMember(x => x.OrganizationInn, x => x.MapFrom(ent => ent.Organization.Inn))
                .ForMember(x => x.PrtnCertificateId, x => x.MapFrom(ent => ent.PrtnCertificate.Id))
                .ForMember(x => x.PrtnCertificateStatusId, x => x.MapFrom(ent => ent.PrtnCertificate.StatusId))
                .ForMember(x => x.PrtnCertificateStatus, x => x.MapFrom(ent => ent.PrtnCertificate.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnCertificate.Status.FullName))
                .ForMember(x => x.ContractorRegionId, x => x.MapFrom(ent => ent.Application.PrtnApplication.ChooseLocation ? ent.Application.PrtnApplication.ChoosedRegionId : ent.Contractor.RegionId))
                .ForMember(x => x.ContractorRegestrationDate, x => x.MapFrom(ent => ent.Contractor.RegistrationDate))
                .ForMember(x => x.ModifiedAt, x => x.MapFrom(ent => ent.ModifiedAt))
                .ForMember(x => x.CreatedAt, x => x.MapFrom(ent => ent.CreatedAt))
                .ForMember(x => x.ContractorRegion, x => x.MapFrom(ent => ent.Application.PrtnApplication.ChooseLocation ?
                    ent.Application.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.PrtnApplication.ChoosedRegion.FullName
                    : ent.Contractor.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Region.FullName))
                .ForMember(x => x.ContractorDistrictId, x => x.MapFrom(ent => ent.Application.PrtnApplication.ChooseLocation ? ent.Application.PrtnApplication.ChoosedDistrictId : ent.Contractor.DistrictId))
                .ForMember(x => x.ContractorDistrict, x => x.MapFrom(ent => ent.Application.PrtnApplication.ChooseLocation ?
                    ent.Application.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.PrtnApplication.ChoosedDistrict.FullName
                    : ent.Contractor.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.District.FullName))
                .ForMember(x => x.LastSigner, x => x.MapFrom(ent =>
                    ent.Signs.Where(a => a.IsSigned).OrderBy(a => a.PrtnContractTypeTable.OrderNumber).LastOrDefault().PrtnContractTypeTable.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                        ? ent.Contractor.FullName
                        : ent.Signs.Where(a => a.IsSigned).OrderBy(a => a.PrtnContractTypeTable.OrderNumber).LastOrDefault().OrganizationSign.FullName
                    )
                )
                .ForMember(x => x.NextSigner, x => x.MapFrom(ent =>
                    ent.Signs.Where(a => !a.IsSigned).OrderBy(a => a.PrtnContractTypeTable.OrderNumber).FirstOrDefault().PrtnContractTypeTable.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                        ? ent.Contractor.FullName
                        : ent.Signs.Where(a => !a.IsSigned).OrderBy(a => a.PrtnContractTypeTable.OrderNumber).FirstOrDefault().OrganizationSign.FullName
                    )
                )
                .ForMember(x => x.Signed, x => x.MapFrom(ent =>
                    ent.Signs.Where(a => a.IsSigned).OrderBy(a => a.PrtnContractTypeTable.OrderNumber)
                        .Select(a => new PrtnContractListSignedDto
                        {
                            OrderNumber = a.PrtnContractTypeTable.OrderNumber,
                            FullName = a.PrtnContractTypeTable.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                                ? a.PrtnContractTypeTable.SignOrganizationType.Translates.AsQueryable().FirstOrDefault(SignOrganizationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnContractTypeTable.SignOrganizationType.FullName
                                : a.OrganizationSign.PrtnContractTypeTable.Position.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.OrganizationSign.PrtnContractTypeTable.Position.ShortName,
                            Fio = a.OrganizationSign.FullName,
                            SignedAt = a.SignedAt
                        })
                    )
                )
                .ForMember(x => x.NotSigned, x => x.MapFrom(ent =>
                    ent.Signs.Where(a => !a.IsSigned).OrderBy(a => a.PrtnContractTypeTable.OrderNumber)
                        .Select(a => new PrtnContractListSignedDto
                        {
                            OrderNumber = a.PrtnContractTypeTable.OrderNumber,
                            FullName = a.PrtnContractTypeTable.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                                ? a.PrtnContractTypeTable.SignOrganizationType.Translates.AsQueryable().FirstOrDefault(SignOrganizationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnContractTypeTable.SignOrganizationType.FullName
                                : a.OrganizationSign.PrtnContractTypeTable.Position.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.OrganizationSign.PrtnContractTypeTable.Position.ShortName,
                            Fio = a.OrganizationSign.FullName,
                            SignedAt = null
                        })
                    )
                )
                .ForMember(x => x.ContractorHasGovShare, x => x.MapFrom(ent => ent.Contractor.GovShare.HasValue))
                .ForMember(x => x.ContractorGovShare, x => x.MapFrom(ent => ent.Contractor.GovShare))
                .ForMember(x => x.OkedCode, x => x.MapFrom(ent => ent.Contractor.Oked.Code))
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Contractor.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Oked.FullName))
                ;

    }
}
