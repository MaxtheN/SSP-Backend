using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_prtn_certificate", Schema = "partner")]
    public partial class PrtnCertificate : IHaveStatusId, IHaveIdProp<long>, IJobDocumentEntity
    {
        public PrtnCertificate()
        {
            Signs = new HashSet<PrtnCertificateSign>();
            StateAssetApplications = new HashSet<StateAssetApplication>();
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
        [Column("prtn_contract_id")]
        public long PrtnContractId { get; set; }
        [Required]
        [Column("prtn_contract_doc_number")]
        [StringLength(50)]
        public string PrtnContractDocNumber { get; set; }
        [Column("prtn_contract_doc_on")]
        public DateOnly PrtnContractDocOn { get; set; }
        [Column("prtn_contract_type_id")]
        public int PrtnContractTypeId { get; set; }
        [Column("expire_on")]
        public DateOnly ExpireOn { get; set; }
        [Column("cancel_on")]
        public DateOnly? CancelOn { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Required]
        [Column("contractor_inn")]
        [StringLength(9)]
        public string ContractorInn { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("prev_status_id")]
        public int? PrevStatusId { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("first_sign")]
        [StringLength(250)]
        public string FirstSign { get; set; }
        [Column("second_sign")]
        [StringLength(250)]
        public string SecondSign { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("total_post_count")]
        public int TotalPostCount { get; set; }
        [Column("success_post_count")]
        public int SuccessPostCount { get; set; }
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
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(PrtnContractId))]
        public virtual PrtnContract PrtnContract { get; set; }
        [ForeignKey(nameof(PrtnContractTypeId))]
        public virtual PrtnContractType PrtnContractType { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(PrtnCertificateSign.Owner))]
        public virtual ICollection<PrtnCertificateSign> Signs { get; set; }
        [InverseProperty(nameof(StateAssetApplication.PrtnCertificate))]
        public virtual ICollection<StateAssetApplication> StateAssetApplications { get; set; }

        [InverseProperty(nameof(DocumentChangeLog.PrtnCertificate))]
        public virtual ICollection<DocumentChangeLog> ChangeLogs { get; set; }
    }
}
