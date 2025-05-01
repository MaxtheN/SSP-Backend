using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses.Memship;
using WEBASE.Models;
using GenericServices;

namespace SspUis.DataLayer.Repositories.Memship
{
    public class MemshipYearlyPlanFileDlDto : EntityDto<MemshipYearlyPlanFileDlDto, MemshipYearlyPlanFile>, IHaveIdProp<Guid>, ILinkToEntity<MemshipYearlyPlanFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    
    }
}
