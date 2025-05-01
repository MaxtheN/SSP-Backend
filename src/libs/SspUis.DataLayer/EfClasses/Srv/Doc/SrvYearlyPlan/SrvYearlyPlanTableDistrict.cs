using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_srv_yearly_plan_table_district", Schema = "srv")]
    [Index(nameof(OwnerId), Name = "ux_doc_srv_yearly_plan_table_district__owner")]
    public partial class SrvYearlyPlanTableDistrict : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("month_on")]
        public int MonthOn { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
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

        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual SrvYearlyPlanTableRegion Owner { get; set; }
    }
}
