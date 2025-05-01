using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceGroupTableDtoConfig : PerDtoConfig<ServicePriceGroupTableDto, ServicePriceTable>
    {
        public override Action<IMappingExpression<ServicePriceTable, ServicePriceGroupTableDto>> AlterReadMapping =>
        cfg => cfg
         .ForMember(d => d.NeedChamberService, c => c.MapFrom(e => e.NeedChamberService.Translates.AsQueryable()
            .FirstOrDefault(NeedChamberServiceTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.NeedChamberService.FullName));
    }
}
