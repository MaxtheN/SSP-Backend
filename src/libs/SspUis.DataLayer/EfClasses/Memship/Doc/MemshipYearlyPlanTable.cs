using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Memship
{
    [Table("doc_memship_yearly_plan_table", Schema = "memship")]
    [Index(nameof(OwnerId), Name = "ux_doc_memship_yearly_plan_table__owner")]
    public partial class MemshipYearlyPlanTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("month_on")]
        public int MonthOn { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("district_id")]
        public int? DistrictId { get; set; }
        [Column("members_count")]
        public int MembersCount { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District? District { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual MemshipYearlyPlan Owner { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
    }
}
