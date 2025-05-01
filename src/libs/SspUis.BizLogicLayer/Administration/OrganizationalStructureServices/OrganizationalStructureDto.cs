using GenericServices;
using SspUis.BizLogicLayer.Info.OrganizationalStructureServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureDto : UpdateOrganizationalStructureDlDto, ILinkToEntity<OrganizationalStructure>, IInfoHl
    {
        public string State { get; internal set; }
        new public List<OrganizationalStructureTranslateDto> Translates { get; set; } = new();
        new public List<OrganizationalStructureCalculationKindDto> StructureCalculationKind { get; set; } = new();
        new public List<OrganizationalStructurePositionDto> StructurePosition { get; set; } = new();
        new public List<OrganizationalStructureStaffingIndicatorDto> StructureStaffingIndicator { get; set; } = new();
    }
}
