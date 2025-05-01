using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class SrvDeedGroupDtoConfig : PerDtoConfig<SrvDeedGroupDto, ServiceDeedGroup>
    {
        public override Action<IMappingExpression<ServiceDeedGroup, SrvDeedGroupDto>> AlterReadMapping =>
        cfg => cfg
         .ForMember(d => d.Group, c => c.MapFrom(e => e.NeedChamberServiceGroup != null
            ? e.NeedChamberServiceGroup.Translates.AsQueryable()
                .FirstOrDefault(NeedChamberServiceGroupTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? e.NeedChamberServiceGroup.FullName
            : string.Empty));
    }
}
