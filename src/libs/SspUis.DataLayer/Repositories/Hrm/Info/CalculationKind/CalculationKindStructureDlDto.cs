using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.EF;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;

namespace SspUis.DataLayer.Repositories
{
        public class CalculationKindStructureDlDto : EntityDto<CalculationKindStructureDlDto, CalculationKindStructure>, IHaveIdProp<long>
        {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int OrganizationalStructureId { get; set; }
    }
    
}
