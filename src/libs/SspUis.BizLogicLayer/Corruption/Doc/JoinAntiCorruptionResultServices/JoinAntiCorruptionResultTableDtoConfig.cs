using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Corruption;

public class JoinAntiCorruptionResultTableDtoConfig : PerDtoConfig<JoinAntiCorruptionResultTableDto, JoinAntiCorruptionResultTable>
{
    public override Action<IMappingExpression<JoinAntiCorruptionResultTable, JoinAntiCorruptionResultTableDto>> AlterReadMapping =>
        cfg => cfg
                .ForMember(x => x.Application, x => x.MapFrom(ent => ent.Application.ApplicationType.FullName))
                .ForMember(x => x.ApplicationDocNumber, x => x.MapFrom(ent => ent.Application.DocNumber))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Application.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Application.Contractor.Inn))
                .ForMember(x => x.JoinAntiCorruptionResultType, x => x.MapFrom(ent => ent.JoinAntiCorruptionResultType.Translates.AsQueryable()
                    .FirstOrDefault(JoinAntiCorruptionResultTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.JoinAntiCorruptionResultType.FullName));
}
