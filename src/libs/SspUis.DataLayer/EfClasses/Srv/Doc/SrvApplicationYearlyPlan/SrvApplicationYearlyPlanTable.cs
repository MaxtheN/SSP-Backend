using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_srv_application_yearly_plan_table", Schema = "srv")]
    [Index(nameof(OwnerId), Name = "ux_doc_srv_application_yearly_plan_table__owner")]
    public partial class SrvApplicationYearlyPlanTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
        [Column("employee_count")]
        public int EmployeeCount { get; set; }
        [Column("free_count")]
        public int FreeCount { get; set; }
        [Column("paid_count")]
        public int PaidCount { get; set; }
        [Column("amount")]
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        [Column("legal_amount")]
        [Precision(18, 2)]
        public decimal LegalAmount { get; set; }
        [Column("economy_amount")]
        [Precision(18, 2)]
        public decimal EconomyAmount { get; set; }
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
        [InverseProperty(nameof(SrvApplicationYearlyPlan.Tables))]
        public virtual SrvApplicationYearlyPlan Owner { get; set; }
    }
}
