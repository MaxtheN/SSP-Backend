using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Memship;

public class DebtTableDtoConfig : PerDtoConfig<DebtTableDto, DebtTable>
{
    public override Action<IMappingExpression<DebtTable, DebtTableDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(x => x.ContractorInn, c => c.MapFrom(ent => ent.Contractor.Inn))
        .ForMember(x => x.ContractorPinfl, c => c.MapFrom(ent => ent.Contractor.Pinfl))
        .ForMember(x => x.Contractor, c => c.MapFrom(ent => ent.Contractor.FullName))
        .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable()
            .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
        ;
}
