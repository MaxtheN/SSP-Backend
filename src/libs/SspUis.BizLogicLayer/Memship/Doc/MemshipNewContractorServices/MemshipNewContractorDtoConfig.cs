

using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Memship;



public class MemshipNewContractorDtoConfig : PerDtoConfig<MemshipNewContractorDto, MemshipNewContractor>
{
    public override Action<IMappingExpression<MemshipNewContractor, MemshipNewContractorDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(x => x.Tables, x => x.MapFrom(ent => ent.Tables))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
            .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
               .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
           ;


}