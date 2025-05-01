using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_srv_yearly_plan", Schema = "srv")]
    public partial class SrvYearlyPlan : IHaveIdProp<long>, IHaveStatusId
    {
        public SrvYearlyPlan()
        {
            Files = new HashSet<SrvYearlyPlanFile>();
            Regions = new HashSet<SrvYearlyPlanTableRegion>();
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
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(SrvYearlyPlanFile.Owner))]
        public virtual ICollection<SrvYearlyPlanFile> Files { get; set; }
        [InverseProperty(nameof(SrvYearlyPlanTableRegion.Owner))]
        public virtual ICollection<SrvYearlyPlanTableRegion> Regions { get; set; }
    }
}
