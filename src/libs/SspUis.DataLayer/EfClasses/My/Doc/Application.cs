using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_application", Schema = "my")]
    public partial class Application : IHaveIdProp<long>, IHaveStatusId, IJobDocumentEntity, IDocument<long>
    {

        public Application()
        {
            Steps = new HashSet<ApplicationStep>();
            MediationPlans = new HashSet<MediationPlan>();
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
        [Column("application_type_id")]
        public int ApplicationTypeId { get; set; }
        [Column("contractor_id")]
        public long? ContractorId { get; set; }
        [Column("contractor_settlement_account_id")]
        public long? ContractorSettlementAccountId { get; set; }
        [Column("current_step_id")]
        public int? CurrentStepId {  get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("prev_status_id")]
        public int? PrevStatusId { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Required]
        [Column("region")]
        [StringLength(500)]
        public string RegionName { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
        [Required]
        [Column("district")]
        [StringLength(500)]
        public string DistrictName { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Required]
        [Column("contractor_position_name")]
        [StringLength(250)]
        public string ContractorPositionName { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }

        [ForeignKey(nameof(ContractorSettlementAccountId))]
        public virtual ContractorSettlementAccount ContractorSettlementAccount { get; set; }

        [ForeignKey(nameof(CreatedUserId))]
        public virtual BusinessmanUser CreatedUser { get; set; }

        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }

        [ForeignKey(nameof(PrevStatusId))]
        public virtual Status PrevStatus { get; set; }

        [ForeignKey(nameof(ApplicationTypeId))]
        public virtual ApplicationType ApplicationType { get; set; }

        [InverseProperty(nameof(EfClasses.PrtnApplication.Application))]
        public virtual PrtnApplication PrtnApplication { get; set; }

        [InverseProperty(nameof(EfClasses.PrtnContract.Application))]
        public virtual PrtnContract PrtnContract { get; set; }

        [InverseProperty(nameof(DualEdu.DualApplication.Application))]
        public virtual DualApplication DualApplication { get; set; }

        [InverseProperty(nameof(EfClasses.StateAssetApplication.Application))]
        public virtual StateAssetApplication StateAssetApplication { get; set; }

        [InverseProperty(nameof(EfClasses.MemshipApplication.Application))]
        public virtual MemshipApplication MemshipApplication { get; set; }

        [InverseProperty(nameof(EfClasses.MemshipContract.Application))]
        public virtual MemshipContract MemshipContract { get; set; }

        [InverseProperty(nameof(Claim.MediationPlan.Application))]
        public virtual ICollection<MediationPlan> MediationPlans { get; set; }

        [InverseProperty(nameof(Claim.ClaimApplication.Application))]
        public virtual ClaimApplication ClaimApplication { get; set; }

        [InverseProperty(nameof(MonoApplication.Application))]
        public virtual MonoApplication MonoApplications { get; set; }

        [InverseProperty(nameof(EfClasses.ArbitrationCourtApplication.Application))]
        public virtual ArbitrationCourtApplication ArbitrationCourtApplication { get; set; }

        [InverseProperty(nameof(EfClasses.ServiceApplication.Application))]
        public virtual EfClasses.ServiceApplication ServiceApplication { get; set; }

        [InverseProperty(nameof(DocumentChangeLog.Application))]
        public virtual ICollection<DocumentChangeLog> ChangeLogs { get; set; }

        [InverseProperty(nameof(ApplicationStep.Application))]
        public virtual ICollection<ApplicationStep> Steps { get; set; }
        [InverseProperty(nameof(EfClasses.Corruption.JoinAntiCorruptionResultTable.Application))]
        public virtual EfClasses.Corruption.JoinAntiCorruptionResultTable JoinAntiCorruptionResultTable { get; set; }
        public ApplicationTypeStep CurrentStep { get; set; }
    }
}
