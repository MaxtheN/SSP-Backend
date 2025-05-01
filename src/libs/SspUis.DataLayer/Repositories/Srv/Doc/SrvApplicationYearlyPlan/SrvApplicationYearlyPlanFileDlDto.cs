using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;
using GenericServices;

namespace SspUis.DataLayer.Repositories
{
    public class SrvApplicationYearlyPlanFileDlDto : EntityDto<SrvApplicationYearlyPlanFileDlDto, SrvApplicationYearlyPlanFile>, IHaveIdProp<Guid>, ILinkToEntity<SrvApplicationYearlyPlanFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    
    }
}
