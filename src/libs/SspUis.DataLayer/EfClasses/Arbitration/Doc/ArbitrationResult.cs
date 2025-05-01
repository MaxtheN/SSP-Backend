using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_arbitration_result", Schema = "arbitration")]
public partial class ArbitrationResult : IHaveIdProp<long>, IHaveStatusId
{
    public ArbitrationResult()
    {
        Files = new HashSet<ArbitrationResultFile>();
        Signs = new HashSet<ArbitrationResultSign>();
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
    [Column("amount")]
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    [Column("can_by_divided")]
	public bool CanByDivided { get; set; }
	[Column("payed_amount")]
    [Precision(18, 2)]
    public decimal PayedAmount { get; set; }
    [Column("arbitration_court_application_id")]
    public long ArbitrationCourtApplicationId { get; set; }
    [Column("contractor_id")]
    public long? ContractorId { get; set; }
    [Column("responsible_contractor_id")]
    public long? ResponsibleContractorId { get; set; }
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

    [ForeignKey(nameof(ArbitrationCourtApplicationId))]
    public virtual ArbitrationCourtApplication ArbitrationCourtApplication { get; set; }
    [ForeignKey(nameof(ContractorId))]
    public virtual Contractor Contractor { get; set; }
    [ForeignKey(nameof(ResponsibleContractorId))]
    public virtual Contractor ResponsibleContractor { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [InverseProperty(nameof(ArbitrationResultFile.Owner))]
    public virtual ICollection<ArbitrationResultFile> Files { get; set; }
    [InverseProperty(nameof(ArbitrationResultSign.Owner))]
    public virtual ICollection<ArbitrationResultSign> Signs { get; set; }
}
