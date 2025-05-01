using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.Memship;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Memship;


public class ContractorRatingListDtoConfig : PerDtoConfig<ContractorRatingListDto, ContractorRating>
{
public override Action<IMappingExpression<ContractorRating, ContractorRatingListDto>> AlterReadMapping => cfg => cfg
.ForMember(x => x.State, x => x.MapFrom(ent =>
        ent.State.Translates.AsQueryable().FirstOrDefault(
            StateTranslate.GetExpr(
                TranslateColumn.full_name,
                ServiceProvider.CultureHelper.CurrentCulture.Id))
        .TranslateText ?? ent.State.FullName))


.ForMember(x => x.ContractorType, x => x.MapFrom(ent =>
        ent.ContractorType.Translates.AsQueryable().FirstOrDefault(
            ContractorTypeTranslate.GetExpr(
                TranslateColumn.full_name,
                ServiceProvider.CultureHelper.CurrentCulture.Id))
        .TranslateText ?? ent.ContractorType.FullName))


;
}