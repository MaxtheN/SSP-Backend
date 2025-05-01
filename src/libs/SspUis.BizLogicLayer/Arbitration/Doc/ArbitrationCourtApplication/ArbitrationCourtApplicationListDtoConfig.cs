using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices
{
    public class ArbitrationCourtApplicationListDtoConfig : PerDtoConfig<ArbitrationCourtApplicationListDto, ArbitrationCourtApplication>
    {
        public override Action<IMappingExpression<ArbitrationCourtApplication, ArbitrationCourtApplicationListDto>> AlterReadMapping =>
                //base.AlterReadMapping
                cfg => cfg
                    .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable()
                        .FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Currency.FullName))
                    .ForMember(x => x.ContractorResponsibleType, x => x.MapFrom(ent => ent.ContractorResponsibleType.Translates.AsQueryable()
                        .FirstOrDefault(ClaimResponsibleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ContractorResponsibleType.FullName))
                    .ForMember(x => x.ClaimResponsibleType, x => x.MapFrom(ent => ent.ResponsibleType.Translates.AsQueryable()
                        .FirstOrDefault(ClaimResponsibleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ResponsibleType.FullName))
                    .ForMember(x => x.ArbitrationCourtResult, x => x.MapFrom(ent => ent.ArbitrationCourtResult.Translates.AsQueryable()
                        .FirstOrDefault(ArbitrationCourtResultTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ArbitrationCourtResult.FullName))
                    .ForMember(x => x.ArbitrationApplicationType, x => x.MapFrom(ent => ent.ArbitrationApplicationType.Translates.AsQueryable()
                        .FirstOrDefault(ArbitrationApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ArbitrationApplicationType.FullName))
                    .ForMember(x => x.ArbitrationCourt, x => x.MapFrom(ent => ent.ArbitrationCourt.Translates.AsQueryable()
                        .FirstOrDefault(ArbitrationCourtTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ResponsibleType.FullName))
                    .ForMember(x => x.ResponsibleContractor, x => x.MapFrom(ent => ent.IsForeignResponsible ? ent.ForeignResponsibleName : ent.ResponsibleContractor.FullName))
                    .ForMember(x => x.AplicationContractor, x => x.MapFrom(ent => ent.IsForeignContractor ? ent.ForeignContractorName : ent.Application.Contractor.FullName))
                    .ForMember(x => x.CanChangeStep, x => x.MapFrom(ent => ent.Application.CurrentStepId < StepIdConst.COURT_DECISION))
                    .ForMember(x => x.CanSelectJudge, x => x.MapFrom(ent => ent.Application.CurrentStepId == StepIdConst.NOTIFIED))
                ;
    }
}
