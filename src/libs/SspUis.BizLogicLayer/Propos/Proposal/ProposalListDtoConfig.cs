using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.Proposal;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Propos
{
    public class ProposalListDtoConfig : PerDtoConfig<ProposalListDto, Proposal>
    {
        public override Action<IMappingExpression<Proposal, ProposalListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.CompanyTypeName, x => x.MapFrom(ent => ent.CompanyType.Translates.AsQueryable()
                    .FirstOrDefault(CompanyTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.CompanyType.FullName))
                .ForMember(x => x.GenderName, x =>  x.MapFrom(ent => !ent.GenderId.HasValue ? "" : ent.Gender.Translates.AsQueryable()
                    .FirstOrDefault(GenderTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Gender.FullName))
                .ForMember(x => x.ProposalTypeName, x => x.MapFrom(ent => !ent.ProposalTypeId.HasValue ? "" : ent.ProposalType.Translates.AsQueryable()
                    .FirstOrDefault(ApplicantTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ProposalType.FullName))
                .ForMember(x => x.NameLatin, x => x.MapFrom(ent => ent.NameLatin))
                .ForMember(x => x.MfyName, x => x.MapFrom(ent => ent.Mfy.FullName))
                .ForMember(x => x.AddressName, x => x.MapFrom(ent => ent.Address))
                .ForMember(x => x.ExternalSourceTypeName, x => x.MapFrom(ent => ent.ExternalSourceType.FullName))
                .ForMember(x => x.BusinessSectorName, x => x.MapFrom(ent => !ent.BusinessSectorId.HasValue ? "" :  ent.BusinessSector.Translates.AsQueryable()
                    .FirstOrDefault(BusinessSectorTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.BusinessSector.FullName))
                .ForMember(x => x.RegionName, x => x.MapFrom(ent => !ent.RegionId.HasValue ? "" :  ent.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.DistrictName, x => x.MapFrom(ent => !ent.DistrictId.HasValue ? "" : ent.District.Translates.AsQueryable()
                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))
                .ForMember(x => x.EmployementTypeName, x => x.MapFrom(ent => !ent.EmployementTypeId.HasValue ? "" : ent.EmployementType.Translates.AsQueryable()
                    .FirstOrDefault(EmploymentTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.EmployementType.FullName))
                .ForMember(x => x.ProposalSubjectName, x => x.MapFrom(ent => !ent.ProposalSubjectId.HasValue ? "" : ent.ProposalSubject.Translates.AsQueryable()
                    .FirstOrDefault(ProposalSubjectTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ProposalSubject.FullName))
                .ForMember(x => x.ProposalDisclosureName, x => x.MapFrom(ent => !ent.ProposalDisclosureId.HasValue ? "" : ent.ProposalDisclosure.Translates.AsQueryable()
                    .FirstOrDefault(ProposalDisclosureTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ProposalDisclosure.FullName))
                .ForMember(x => x.ToOrganizationName, x => x.MapFrom(ent => !ent.ToOrganizationId.HasValue ? "" : ent.ToOrganization.Translates.AsQueryable()
                    .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ToOrganization.FullName))
                ;
    }
}
