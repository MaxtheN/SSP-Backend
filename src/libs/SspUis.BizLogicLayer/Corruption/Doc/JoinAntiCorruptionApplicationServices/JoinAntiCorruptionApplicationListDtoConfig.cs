using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public class JoinAntiCorruptionApplicationListDtoConfig : PerDtoConfig<JoinAntiCorruptionApplicationListDto, JoinAntiCorruptionApplication>
    {
        public override Action<IMappingExpression<JoinAntiCorruptionApplication, JoinAntiCorruptionApplicationListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.OkedCode, x => x.MapFrom(ent => ent.Application.Contractor.Oked.Code))
                .ForMember(x => x.CurrentStepId, x => x.MapFrom(ent => ent.Application.CurrentStepId))
                .ForMember(x => x.CurrentStep, x => x.MapFrom(ent => ent.Application.CurrentStep.Translates.AsQueryable().FirstOrDefault(ApplicationTypeStepTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? ent.Application.CurrentStep.FullName))
                .ForMember(x => x.CertificateId, x => x.MapFrom(ent => ent.Application.JoinAntiCorruptionResultTable.JoinAntiCorruptionCertificate.Id))
                .ForMember(x => x.CertificateStatusId, x => x.MapFrom(ent => ent.Application.JoinAntiCorruptionResultTable.Owner.StatusId))
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Application.Contractor.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
                    ?? ent.Application.Contractor.Oked.FullName))
                .ForMember(x => x.CorruptionReviewType, x => x.MapFrom(ent => ent.CorruptionReviewType.Translates.AsQueryable()
                    .FirstOrDefault(CorruptionReviewTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
                    ?? ent.CorruptionReviewType.FullName))
                .ForMember(x => x.ContractorActivityType, x => x.MapFrom(ent => ent.ContractorActivityType.Translates.AsQueryable()
                    .FirstOrDefault(ContractorActivityTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
                    ?? ent.ContractorActivityType.FullName))
                .ForMember(x => x.ContractorUnionActivityType, x => x.MapFrom(ent => ent.ContractorUnionActivityType.Translates.AsQueryable()
                    .FirstOrDefault(ContractorUnionActivityTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
                    ?? ent.ContractorUnionActivityType.FullName));
    }
}
