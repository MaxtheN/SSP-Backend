using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceGroupTableListDtoConfig : PerDtoConfig<ServicePriceGroupTableListDto, ServicePriceTable>
    {
        public override Action<IMappingExpression<ServicePriceTable, ServicePriceGroupTableListDto>> AlterReadMapping =>
        cfg => cfg
         .ForMember(d => d.NeedChamberService, c => c.MapFrom(e => e.NeedChamberService.Translates.AsQueryable()
            .FirstOrDefault(NeedChamberServiceTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.NeedChamberService.FullName))

        .ForMember(d => d.ServicePriceTypeId, c => c.MapFrom(e => e.NeedChamberService.ServicePriceTypeId))

        .ForMember(d => d.ServicePriceType, c => c.MapFrom(e => e.NeedChamberService.ServicePriceType.Translates.AsQueryable()
            .FirstOrDefault(ServicePriceTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.NeedChamberService.FullName));
    }
}