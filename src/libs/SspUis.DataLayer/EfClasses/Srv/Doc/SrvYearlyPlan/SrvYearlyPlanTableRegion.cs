using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_srv_yearly_plan_table_region", Schema = "srv")]
    [Index(nameof(OwnerId), Name = "ux_doc_srv_yearly_plan_table_region__owner")]
    public partial class SrvYearlyPlanTableRegion : IHaveIdProp<long>
    {
        public SrvYearlyPlanTableRegion()
        {
            Districts = new HashSet<SrvYearlyPlanTableDistrict>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("month_on")]
        public int MonthOn { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual SrvYearlyPlan Owner { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [InverseProperty(nameof(SrvYearlyPlanTableDistrict.Owner))]
        public virtual ICollection<SrvYearlyPlanTableDistrict> Districts { get; set; }
    }
}
