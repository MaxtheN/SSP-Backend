using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureDashboardDto : ILinkToEntity<OrganizationalStructure>, IHaveIdProp<int>
    {
        public int Id { get; set; }    
        public string FullName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string CodeSymbol { get; set; } = null!;
        public int StateId { get; set; }
        public bool? IsParent { get; set; }
        public int OrganizationCount { get; set; }
        public int PositionCount { get; set; }
        public int ManagementPositionCount { get; set; }
        public int AssistantPositionCount { get; set; }
        public int ProductionPositionCount { get; set; }
        public int TechnicalPositionCount { get; set; }

    }

    public class OrganizationalStructureDashboardDtoConfig : PerDtoConfig<OrganizationalStructureDashboardDto, OrganizationalStructure>
    {
        public override Action<IMappingExpression<OrganizationalStructure, OrganizationalStructureDashboardDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ManagementPositionCount, x => x.MapFrom(ent => ent.StructurePosition.Where(x => x.PositionCategoryId == 1).Count()))
                .ForMember(x => x.OrganizationCount, x => x.MapFrom(ent => ent.Organizations.Count))
                .ForMember(x => x.PositionCount, x => x.MapFrom(ent => ent.StructurePosition.Count()))
                .ForMember(x => x.AssistantPositionCount, x => x.MapFrom(ent => ent.StructurePosition.Where(x => x.PositionCategoryId == 2).Count()))
                .ForMember(x => x.TechnicalPositionCount, x => x.MapFrom(ent => ent.StructurePosition.Where(x => x.PositionCategoryId == 4).Count()))
                .ForMember(x => x.ProductionPositionCount, x => x.MapFrom(ent => ent.StructurePosition.Where(x => x.PositionCategoryId == 3).Count()))
            ;
    }
}
