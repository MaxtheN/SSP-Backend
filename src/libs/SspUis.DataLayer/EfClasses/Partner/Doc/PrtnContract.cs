using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_prtn_contract", Schema = "partner")]
    public partial class PrtnContract : IHaveStatusId, IHaveIdProp<long>
    {
        public PrtnContract()
        {
            Signs = new HashSet<PrtnContractSign>();
            Files = new HashSet<PrtnContractFile>();
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
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("prtn_contract_type_id")]
        public int PrtnContractTypeId { get; set; }
        [Column("new_vacancies_count")]
        public int NewVacanciesCount { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("prtn_reject_reason_id")]
        public int? PrtnRejectReasonId { get; set; }
        [Column("status_change_expire_on" , TypeName = "timestamp without time zone")]
        public DateTime? StatusChangeExpireOn { get; set; }
        [Column("pass_expertise_expire_on" , TypeName = "timestamp without time zone")]
        public DateTime? PassExpertiseExpireOn { get; set; }
        [Column("not_pass_expertise_expire_on", TypeName = "timestamp without time zone")]
        public DateTime? NotPassExpertiseExpireOn { get; set; }
        [Column("resend_expertise_expire_on", TypeName = "timestamp without time zone")]
        public DateTime? ResendExpertiseExpireOn { get; set; }

        [Column("sign_expire_on", TypeName = "timestamp without time zone")]
        public DateTime? SignExpireOn { get; set; }

        [Column("signing_expire_on", TypeName = "timestamp without time zone")]
        public DateTime? SigningExpireOn { get; set; }

        [Column("prepare_certificate_expire_on", TypeName = "timestamp without time zone")]
        public DateTime? PrepareCertificateExpireOn { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(PrtnContractTypeId))]
        public virtual PrtnContractType PrtnContractType { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
        [InverseProperty(nameof(EfClasses.PrtnCertificate.PrtnContract))]
        public virtual PrtnCertificate PrtnCertificate { get; set; }
        [ForeignKey(nameof(PrtnRejectReasonId))]
        public virtual PrtnRejectReason PrtnRejectReason { get; set; }
        [InverseProperty(nameof(PrtnContractSign.Owner))]
        public virtual ICollection<PrtnContractSign> Signs { get; set; }
        [InverseProperty(nameof(PrtnContractFile.Owner))]
        public virtual ICollection<PrtnContractFile> Files { get; set; }

        [InverseProperty(nameof(DocumentChangeLog.PrtnContract))]
        public virtual ICollection<DocumentChangeLog> ChangeLogs { get; set; }
    }
}
