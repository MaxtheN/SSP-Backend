using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Memship;


public class MemshipNewContractorTableDtoConfig : PerDtoConfig<MemshipNewContractorTableDto, MemshipNewContractorsTable>
{
    public override Action<IMappingExpression<MemshipNewContractorsTable, MemshipNewContractorTableDto>> AlterReadMapping =>
        cfg => cfg
                   //.ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                   // .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName));
}
