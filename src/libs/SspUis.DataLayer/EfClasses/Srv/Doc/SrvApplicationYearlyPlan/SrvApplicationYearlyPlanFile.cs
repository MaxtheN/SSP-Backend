using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_srv_application_yearly_plan_file", Schema = "srv")]
    [Index(nameof(OwnerId), Name = "ix_doc_srv_yearly_plan_files__owner")]
    public partial class SrvApplicationYearlyPlanFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(SrvApplicationYearlyPlan.Files))]
        public virtual SrvApplicationYearlyPlan Owner { get; set; }
    }
}
