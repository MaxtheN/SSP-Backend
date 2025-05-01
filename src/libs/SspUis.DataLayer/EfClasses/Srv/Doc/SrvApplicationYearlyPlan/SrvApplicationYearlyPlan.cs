using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_srv_application_yearly_plan", Schema = "srv")]
    public partial class SrvApplicationYearlyPlan : IHaveIdProp<long>, IHaveStatusId
    {
        public SrvApplicationYearlyPlan()
        {
            Files = new HashSet<SrvApplicationYearlyPlanFile>();
            Tables = new HashSet<SrvApplicationYearlyPlanTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("year")]
        public int Year { get; set; }
        [Column("month_on")]
        public int MonthOn { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("region_free_count")]
        public int RegionFreeCount { get; set; }
        [Column("region_paid_count")]
        public int RegionPaidCount { get; set; }
        [Column("region_employee_count")]
        public int RegionEmployeeCount { get; set; }
        [Column("region_amount")]
        [Precision(18, 2)]
        public decimal RegionAmount { get; set; }
        [Column("region_legal_amount")]
        [Precision(18, 2)]
        public decimal RegionLegalAmount { get; set; }
        [Column("region_economy_amount")]
        [Precision(18, 2)]
        public decimal RegionEconomyAmount { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(SrvApplicationYearlyPlanTable.Owner))]
        public virtual ICollection<SrvApplicationYearlyPlanTable> Tables { get; set; }
        [InverseProperty(nameof(SrvApplicationYearlyPlanFile.Owner))]
        public virtual ICollection<SrvApplicationYearlyPlanFile> Files { get; set; }
    }
}
