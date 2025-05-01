using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationTableDtoConfig : PerDtoConfig<SrvApplicationTableDto, ServiceApplicationTable>
    {
        public override Action<IMappingExpression<ServiceApplicationTable, SrvApplicationTableDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.NeedChamberService, x => x.MapFrom(ent => ent.NeedChamberService.Translates.AsQueryable()
                    .FirstOrDefault(NeedChamberServiceTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.NeedChamberService.FullName))
                .ForMember(x => x.ServicePriceId, x => x.MapFrom(ent => ent.ServicePriceId))
                .ForMember(x => x.ServicePriceTableId, x => x.MapFrom(ent => ent.ServicePriceTableId))
                .ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files));
    }
}
