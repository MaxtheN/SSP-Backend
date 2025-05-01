using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_additional_agreement")]
public class AdditionalAgreement : IHaveIdProp<long>, IHaveStatusId
{

    public AdditionalAgreement()
    {
        Signs = new HashSet<AdditionalAgreementSign>();
    }
    [Column("id")]
    public long Id { get; set; }
    [Column("id2")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id2 { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Column("doc_number")]
    [StringLength(50)]
    public string DocNumber { get; set; }
    [Column("base_fixed_minimum_value")]
    public decimal BaseFixedMinimumValue { get; set; }
    [Column("amount")]
    public decimal Amount { get; set; }
    [Column("contractor_id")]
    public long ContractorId { get; set; }
    [Column("application_type_id")]
    public int ApplicationTypeId { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("memship_contract_id")]
    public long MemshipContractId { get; set; }
    [Column("can_pay_divided")]
    public bool CanPayDivided { get; set; }
    [Column("details")]
    public string Details { get; set; }

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
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [ForeignKey(nameof(ApplicationTypeId))]
    public virtual ApplicationType ApplicationType { get; set; }
    [ForeignKey(nameof(MemshipContractId))]
    public virtual MemshipContract MemshipContract { get; set; }
    [InverseProperty(nameof(AdditionalAgreementSign.Owner))]
    public virtual ICollection<AdditionalAgreementSign> Signs { get; set; }

}
