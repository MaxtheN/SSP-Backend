using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.Claim;

public class MediationClaimApplicationTableDtoConfig : PerDtoConfig<MediationClaimApplicationTableDto, ClaimApplicationTable>
{
    public override Action<IMappingExpression<ClaimApplicationTable, MediationClaimApplicationTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.ClaimResponsibleType, c => c.MapFrom(e => e.ClaimResponsibleType.Translates.AsQueryable()
                .FirstOrDefault(ClaimResponsibleTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.ClaimResponsibleType.FullName))
        ;


}
