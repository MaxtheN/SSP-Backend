using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class SrvContractTableDtoConfig : PerDtoConfig<SrvContractTableDto, ServiceContractTable>
    {
        public override Action<IMappingExpression<ServiceContractTable, SrvContractTableDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.NeedChamberService, x => x.MapFrom(ent => ent.NeedChamberService.Translates.AsQueryable()
                    .FirstOrDefault(NeedChamberServiceTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.NeedChamberService.FullName));
    }
}
