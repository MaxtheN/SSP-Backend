using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ArbitrationResultSignDtoConfig : PerDtoConfig<ArbitrationResultSignDto, ArbitrationResultSign>
{
    public override Action<IMappingExpression<ArbitrationResultSign, ArbitrationResultSignDto>> AlterReadMapping =>
        cfg => cfg
        //.ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
        //    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
        
        .ForMember(x => x.FirstName, x => x.MapFrom(ent => ent.ArbitrationJudge.FirstName))
        .ForMember(x => x.LastName, x => x.MapFrom(ent => ent.ArbitrationJudge.LastName))
        .ForMember(x => x.MiddleName, x => x.MapFrom(ent => ent.ArbitrationJudge.MiddleName));
        
}
