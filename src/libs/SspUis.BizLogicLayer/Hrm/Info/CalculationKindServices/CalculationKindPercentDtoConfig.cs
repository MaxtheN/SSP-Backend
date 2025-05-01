using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindPercentDtoConfig : PerDtoConfig<CalculationKindPercentDto, CalculationKindPercent>
    {
        public override Action<IMappingExpression<CalculationKindPercent, CalculationKindPercentDto>> AlterReadMapping => cfg => cfg
               .ForMember(x => x.LimitOperType, x => x.MapFrom(ent => ent.LimitOperTypeId.HasValue ? ent.LimitOperType.Translates.AsQueryable().FirstOrDefault(LimitOperTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.LimitOperType.ShortName : null));
    }
}
        

    
