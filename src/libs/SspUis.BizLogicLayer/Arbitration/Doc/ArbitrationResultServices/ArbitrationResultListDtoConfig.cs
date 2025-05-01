using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ArbitrationResultListDtoConfig : PerDtoConfig<ArbitrationResultListDto, ArbitrationResult>
{
    public override Action<IMappingExpression<ArbitrationResult, ArbitrationResultListDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(x => x.Contractor, c => c.MapFrom(ent => ent.ArbitrationCourtApplication.IsForeignContractor ? ent.ArbitrationCourtApplication.ForeignContractorName : ent.Contractor.FullName))
        .ForMember(x => x.ResponsibleContractor, c => c.MapFrom(ent => ent.ArbitrationCourtApplication.IsForeignResponsible ? ent.ArbitrationCourtApplication.ForeignResponsibleName : ent.ResponsibleContractor.FullName))
		.ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))

        ;
}
