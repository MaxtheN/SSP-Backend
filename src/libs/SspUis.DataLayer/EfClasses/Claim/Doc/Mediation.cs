using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Claim
{
    [Table("doc_mediation", Schema = "claim")]
    public partial class Mediation : IHaveIdProp<long>, IHaveStatusId
    {
        public Mediation()
        {
            Files = new HashSet<MediationFile>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("id2")]
        public Guid Id2 { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("mediation_plan_id")]
        public long MediationPlanId { get; set; }
        [Required]
        [Column("contractor_details")]
        public string ContractorDetails { get; set; }
        [Required]
        [Column("responsible_details")]
        public string ResponsibleDetails { get; set; }
        [Column("responsible_person_name")]
        public string ResponsiblePersonName { get; set; }
        [Column("claimant_person_name")]
        public string ClaimantPersonName { get; set; }
        [Column("mediation_result_id")]
        public int MediationResultId { get; set; }
        [Column("claim_need_court_id")]
        public int ClaimNeedCourtId { get; set; }
        [Column("chamber_person")]
        [StringLength(200)]
        public string ChamberPerson { get; set; }
        [Column("court_at", TypeName = "timestamp without time zone")]
        public DateTime? CourtAt { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("message")]
        public string? Message { get; set; }

        [ForeignKey(nameof(ClaimNeedCourtId))]
        public virtual ClaimNeedCourt ClaimNeedCourt { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(MediationPlanId))]
        [InverseProperty(nameof(EfClasses.Claim.MediationPlan.Mediations))]
        public virtual MediationPlan MediationPlan { get; set; }
        [ForeignKey(nameof(MediationResultId))]
        public virtual MediationResult MediationResult { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(ApplicationForCourt.Mediation))]
        public ApplicationForCourt ApplicationForCourts { get; set; }
        [InverseProperty(nameof(MediationFile.Owner))]
        public virtual ICollection<MediationFile> Files { get; set; }
    }
}
