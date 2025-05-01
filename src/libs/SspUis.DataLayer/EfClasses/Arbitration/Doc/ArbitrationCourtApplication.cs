using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_arbitration_court_application", Schema = "arbitration")]
[Index(nameof(ApplicationId), Name = "uc_application_id", IsUnique = true)]
public partial class ArbitrationCourtApplication : IHaveIdProp<long>, IBaseApplicationEntity
{
    public ArbitrationCourtApplication()
    {
        Files = new HashSet<ArbitrationCourtApplicationFile>();
        Signer = new HashSet<ArbitrationCourtApplicationSigner>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("application_id")]
    public long ApplicationId { get; set; }
    [Column("e_court_number")]
    [StringLength(50)]
    public string ECourtNumber { get; set; }
    [Required]
    [Column("contractor_phone_number")]
    [StringLength(50)]
    public string ContractorPhonber { get; set; }
    [Column("contractor_responsible_type_id")]
    public int ContractorResponsibleTypeId { get; set; }
    [Required]
    [Column("contractor_address")]
    [StringLength(250)]
    public string ContractorAddress { get; set; }
    [Required]
    [Column("responsible_phone_number")]
    [StringLength(50)]
    public string ResponsiblePhonber { get; set; }
    [Column("responsible_type_id")]
    public int ResponsibleTypeId { get; set; }
    [Required]
    [Column("responsible_address")]
    [StringLength(250)]
    public string ResponsibleAddress { get; set; }
    [Column("responsible_contractor_id")]
    public long? ResponsibleContractorId { get; set; }
    [Column("amount")]
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    [Required]
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("arbitration_amount")]
    [Precision(18, 2)]
    public decimal ArbitrationAmount { get; set; }
    [Column("currency_id")]
    public int CurrencyId { get; set; }
    [Column("is_created_by_erp")]
    public bool IsCreatedByErp { get; set; }
    [Column("arbitration_application_type_id")]
    public int ArbitrationApplicationTypeId { get; set; }
    [Column("arbitration_court_id")]
    public int? ArbitrationCourtId { get; set; }
    [Column("discussion_date")]
    public DateTime? DiscussionDate { get; set; }
    [Column("arbitration_court_result_id")]
    public int? ArbitrationCourtResultId { get; set; }
    [Column("can_by_divided")]
    public bool CanByDivided { get; set; } = false;
    [Column("allocated_divided")]
    public decimal? AllocatedDivided { get; set; }

    #region Foreign props
    [Column("is_foreign_contractor")]
    public bool IsForeignContractor { get; set; }
    [Column("is_foreign_responsible")]
    public bool IsForeignResponsible { get; set; }
    [StringLength(50)]
    [Column("foreign_contractor_inn")]
    public string ForeignContractorInn { get; set; }
    [StringLength(50)]
    [Column("foreign_responsible_inn")]
    public string ForeignResponsibleInn { get; set; }
    [StringLength(500)]
    [Column("foreign_contractor_name")]
    public string ForeignContractorName { get; set; }
    [StringLength(500)]
    [Column("foreign_responsible_name")]
    public string ForeignResponsibleName { get; set; }
    #endregion
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }
    [Column("web_imzo_secret_key")]
    public string? WebImzoSecretKey { get; set; }
    [Column("web_imzo_request_id")]
    public Guid? WebImzoRequestId { get; set; }

    #region Refs
    [ForeignKey(nameof(ApplicationId))]
    [InverseProperty(nameof(EfClasses.Application.ArbitrationCourtApplication))]
    public virtual Application Application { get; set; }
    [InverseProperty(nameof(EfClasses.ArbitrationDiscussion.ArbitrationCourtApplication))]
    public virtual ArbitrationDiscussion ArbitrationDiscussion { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [InverseProperty(nameof(EfClasses.ArbitrationDelay.ArbitrationCourtApplication))]
    public virtual ArbitrationDelay ArbitrationDelay { get; set; }
    [InverseProperty(nameof(EfClasses.ArbitrationResult.ArbitrationCourtApplication))]
    public virtual ArbitrationResult ArbitrationResult { get; set; }
    [ForeignKey(nameof(ContractorResponsibleTypeId))]
    public virtual ClaimResponsibleType ContractorResponsibleType { get; set; }
    [ForeignKey(nameof(CurrencyId))]
    public virtual Currency Currency { get; set; }
    [ForeignKey(nameof(ResponsibleContractorId))]
    public virtual Contractor ResponsibleContractor { get; set; }
    [ForeignKey(nameof(ResponsibleTypeId))]
    public virtual ClaimResponsibleType ResponsibleType { get; set; }
    [ForeignKey(nameof(ArbitrationApplicationTypeId))]
    public virtual ArbitrationApplicationType ArbitrationApplicationType { get; set; }
    [ForeignKey(nameof(ArbitrationCourtId))]
    public virtual ArbitrationCourt ArbitrationCourt { get; set; }
    [ForeignKey(nameof(ArbitrationCourtResultId))]
    public virtual ArbitrationCourtResult ArbitrationCourtResult { get; set; }
    [InverseProperty(nameof(ArbitrationCourtApplicationFile.Owner))]
    public virtual ICollection<ArbitrationCourtApplicationFile> Files { get; set; }
    [InverseProperty(nameof(ArbitrationCourtApplicationSigner.Owner))]
    public virtual ICollection<ArbitrationCourtApplicationSigner> Signer { get; set; }
    #endregion
}
