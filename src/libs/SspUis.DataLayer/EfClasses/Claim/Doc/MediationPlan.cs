using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Claim
{
    [Table("doc_mediation_plan", Schema = "claim")]
    public partial class MediationPlan : IHaveIdProp<long>, IHaveStatusId
    {
        public MediationPlan()
        {
            Mediations = new HashSet<Mediation>();
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
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("meeting_type_id")]
        public int MeetingTypeId { get; set; }
        [Column("medition_at", TypeName = "timestamp without time zone")]
        public DateTime MeditionAt { get; set; }
        [Required]
        [Column("address_or_url")]
        [StringLength(250)]
        public string AddressOrUrl { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("chamber_person")]
        [StringLength(200)]
        public string ChamberPerson { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        /// <summary>
        /// Har doim Back dan set qilinadi frontga umuman chiqmidi.
        /// </summary>
        [Column("next_mediation_plan_id")]
        public long? NextMediationPlanId { get; set; }
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

        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(MeetingTypeId))]
        public virtual MeetingType MeetingType { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        [ForeignKey(nameof(NextMediationPlanId))]
        [InverseProperty(nameof(PreviousMediationPlan))]
        public virtual MediationPlan NextMediationPlan { get; set; }

        [InverseProperty(nameof(NextMediationPlan))]
        public virtual MediationPlan PreviousMediationPlan { get; set; }
        [InverseProperty(nameof(Mediation.MediationPlan))]
        public virtual ICollection<Mediation> Mediations { get; set; }
    }
}
