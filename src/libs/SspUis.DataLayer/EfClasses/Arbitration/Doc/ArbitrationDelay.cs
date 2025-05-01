using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_arbitration_delay", Schema = "arbitration")]
public partial class ArbitrationDelay : IHaveIdProp<long>, IHaveStatusId
{
    public ArbitrationDelay()
    {
        Signs = new HashSet<ArbitrationDelaySign>();
        Files = new HashSet<ArbitrationDelayFile>();
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
    [Column("delay_date", TypeName = "timestamp without time zone")]
    public DateTime DelayDate { get; set; }
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
    [InverseProperty(nameof(ArbitrationDelaySign.Owner))]
    public virtual ICollection<ArbitrationDelaySign> Signs { get; set; }
    [InverseProperty(nameof(ArbitrationDelaySign.Owner))]
    public virtual ICollection<ArbitrationDelayFile> Files { get; set; }
}
