using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureListDto : ILinkToEntity<OrganizationalStructure>, IHaveIdProp<int>
    {
        public int Id { get; set; }

        public int? OrderCode { get; set; }
        public string Code { get; set; } = null!;
        public int StructureType { get; set; }
        public bool? IsParent { get; set; }
        public int CodeNumber { get; set; }
        public string CodeSymbol { get; set; }
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int StateId { get; set; }
        public string State { get; set; }
        public int OrganizationCount { get; set; }
        public int PositionCount { get; set; }
        public int CalculationKindCount { get; set; }
    }
}
