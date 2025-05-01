using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_memship_payment_order", Schema = "public")]
[Index(nameof(OrganizationId), nameof(StatusId), nameof(DocOn), Name = "ix_doc_memship_payment_order_org_status_docon")]
public partial class MemshipPaymentOrder : IHaveIdProp<long>, IHaveStatusId
{
    public MemshipPaymentOrder()
    {
        Files = new HashSet<MemshipPaymentOrderFile>();
        Tables = new HashSet<MemshipPaymentOrderTable>();
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
    [StringLength(1024)]
    public string Details { get; set; }
    [Column("contractor_id")]
    public long ContractorId { get; set; }
    [Column("memship_contract_id")]
    public long? MemshipContractId { get; set; }
    [Column("bank_id")]
    public int BankId { get; set; }
    [Column("amount")]
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    [Column("currency_id")]
    public int CurrencyId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }

    [Column("application_type_id")]
    public int ApplicationTypeId { get; set; }
    [Column("service_contract_id")]
    public long? ServiceContractId { get; set; }

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
    [Column("message")]
    public string Message { get; set; }

    [ForeignKey(nameof(BankId))]
    public virtual Bank Bank { get; set; }
    [ForeignKey(nameof(ContractorId))]
    public virtual Contractor Contractor { get; set; }
    [ForeignKey(nameof(CurrencyId))]
    public virtual Currency Currency { get; set; }
    [ForeignKey(nameof(MemshipContractId))]
    public virtual MemshipContract? MemshipContract { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }

    [ForeignKey(nameof(ApplicationTypeId))]
    public virtual ApplicationType ApplicationType { get; set; }

    [ForeignKey(nameof(ServiceContractId))]
    public virtual ServiceContract? ServiceContract { get; set; }
    [InverseProperty(nameof(MemshipPaymentOrderFile.Owner))]
    public virtual ICollection<MemshipPaymentOrderFile> Files { get; set; }
	[InverseProperty(nameof(MemshipPaymentOrderTable.Owner))]
	public virtual ICollection<MemshipPaymentOrderTable> Tables { get; set; }
}
