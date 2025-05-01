using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public class JoinAntiCorruptionApplicationDtoConfig : PerDtoConfig<JoinAntiCorruptionApplicationDto, JoinAntiCorruptionApplication>
    {
        public override Action<IMappingExpression<JoinAntiCorruptionApplication, JoinAntiCorruptionApplicationDto>> AlterReadMapping =>
            cfg => cfg
                  .ForMember(x => x.CurrencyId, x => x.MapFrom(ent => ent.CurrencyId))
                  .ForMember(x => x.PrevYearlyEarnings, x => x.MapFrom(ent => ent.PrevYearlyEarnings))
                  .ForMember(x => x.AvgEmployeesCount, x => x.MapFrom(ent => ent.AvgEmployeesCount))
                  .ForMember(x => x.UnionMemberCount, x => x.MapFrom(ent => ent.UnionMemberCount))
                  .ForMember(x => x.Address, x => x.MapFrom(ent => ent.Address))
                  .ForMember(x => x.Details, x => x.MapFrom(ent => ent.Details))
                  .ForMember(x => x.ContractorActivityTypeId, x => x.MapFrom(ent => ent.ContractorActivityType.Id))
                  .ForMember(x => x.CorruptionReviewTypeId, x => x.MapFrom(ent => ent.CorruptionReviewType.Id))
                  .ForMember(x => x.ContractorUnionActivityTypeId, x => x.MapFrom(ent => ent.ContractorUnionActivityType.Id))
                  .ForMember(x => x.CorruptionReviewType, x => x.MapFrom(ent => ent.CorruptionReviewType.Translates.AsQueryable()
                    .FirstOrDefault(CorruptionReviewTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
                    ?? ent.CorruptionReviewType.FullName))
                  .ForMember(x => x.ContractorActivityType, x => x.MapFrom(ent => ent.ContractorActivityType.Translates.AsQueryable()
                    .FirstOrDefault(ContractorActivityTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
                    ?? ent.ContractorActivityType.FullName))
                  .ForMember(x => x.ContractorUnionActivityType, x => x.MapFrom(ent => ent.ContractorUnionActivityType.Translates.AsQueryable()
                    .FirstOrDefault(ContractorUnionActivityTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText 
                    ?? ent.ContractorUnionActivityType.FullName))
                 .ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files))
                 .ForMember(x => x.Tables, x => x.MapFrom(ent => ent.Tables))
                 .ForMember(x => x.Employees, x => x.MapFrom(ent => ent.Employees))
                 .ForMember(x => x.Participates, x => x.MapFrom(ent => ent.Participates))
            ;
    }
}
