using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureListDtoConfig : PerDtoConfig<OrganizationalStructureListDto, OrganizationalStructure>
    {
        public override Action<IMappingExpression<OrganizationalStructure, OrganizationalStructureListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                 .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
                .ForMember(x => x.CalculationKindCount, x => x.MapFrom(ent => ent.StructureCalculationKind.Count))
                .ForMember(x => x.OrganizationCount, x => x.MapFrom(ent => ent.Organizations.Count))
                .ForMember(x => x.PositionCount, x => x.MapFrom(ent => ent.StructurePosition.Count))
            ;
    }
}
