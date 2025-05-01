using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Memship
{
    [Table("doc_memship_yearly_plan_file", Schema = "memship")]
    [Index(nameof(OwnerId), Name = "ix_doc_memship_yearly_plan_files__owner")]
    public partial class MemshipYearlyPlanFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MemshipYearlyPlan.Files))]
        public virtual MemshipYearlyPlan Owner { get; set; }
    }
}
