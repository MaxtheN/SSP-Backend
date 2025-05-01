using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices
{
    public class ArbitrationCourtApplicationDtoConfig : PerDtoConfig<ArbitrationCourtApplicationDto, ArbitrationCourtApplication>
    {
        public override Action<IMappingExpression<ArbitrationCourtApplication, ArbitrationCourtApplicationDto>> AlterReadMapping =>
                //base.AlterReadMapping
                cfg => cfg
                    .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable()
                        .FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Currency.FullName))
                    .ForMember(x => x.ContractorResponsibleType, x => x.MapFrom(ent => ent.ContractorResponsibleType.Translates.AsQueryable()
                        .FirstOrDefault(ClaimResponsibleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ContractorResponsibleType.FullName))
                    .ForMember(x => x.ArbitrationCourtResult, x => x.MapFrom(ent => ent.ArbitrationCourtResult.Translates.AsQueryable()
                        .FirstOrDefault(ArbitrationCourtResultTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ArbitrationCourtResult.FullName))
                    .ForMember(x => x.Responsible, x => x.MapFrom(ent => ent.ResponsibleContractor.FullName))
                    .ForMember(x => x.ContractorId, x => x.MapFrom(ent => ent.Application.ContractorId))
                    .ForMember(x => x.ArbitrationResultId, x => x.MapFrom(ent => ent.ArbitrationResult.Id))
                    .ForMember(x => x.ArbitrationDiscussionId, x => x.MapFrom(ent => ent.ArbitrationDiscussion.Id))
                    .ForMember(x => x.ArbitrationDelayId, x => x.MapFrom(ent => ent.ArbitrationDelay.Id))
                    .ForMember(x => x.ResponsibleInnPnfl, x => x.MapFrom(ent => ent.ResponsibleContractor.Inn != null ? ent.ResponsibleContractor.Inn : ent.ResponsibleContractor.Pinfl))
                    .ForMember(x => x.ContractorInnPnfl, x => x.MapFrom(ent => ent.Application.Contractor.Inn != null ? ent.Application.Contractor.Inn : ent.Application.Contractor.Pinfl))
                    .ForMember(x => x.ClaimResponsibleType, x => x.MapFrom(ent => ent.ResponsibleType.Translates.AsQueryable()
                        .FirstOrDefault(ClaimResponsibleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ResponsibleType.FullName));
    }
}
