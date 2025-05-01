using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class MemshipCertificateDtoConfig : PerDtoConfig<MemshipCertificateDto, MemshipCertificate>
{
    public override Action<IMappingExpression<MemshipCertificate, MemshipCertificateDto>> AlterReadMapping =>
        cfg => cfg 
        .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
            .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
        .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
        .ForMember(x => x.IsRead, x => x.MapFrom(ent => ent.IsRead))
        .ForMember(d => d.ContractorInn, c => c.MapFrom(e => e.Contractor.Inn))
        .ForMember(d => d.ContractorSettlementAccount, c => c.MapFrom(e => e.ContractorSettlementAccount.AccountName))
        .ForMember(d => d.Files, c => c.MapFrom(e => e.Files));
}