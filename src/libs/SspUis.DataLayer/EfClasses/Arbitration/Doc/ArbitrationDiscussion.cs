using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_arbitration_discussion", Schema = "arbitration")]

public class ArbitrationDiscussion : IHaveIdProp<long>, IHaveStatusId
{
    public ArbitrationDiscussion()
    {
        Files = new HashSet<ArbitrationDiscussionFile>();
        Signs = new HashSet<ArbitrationDiscussionSign>();
    }
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Column("doc_number")]
    public string DocNumber { get; set; }
    [Column("discussion_date")]
    public DateTime DiscussionDate { get; set; }
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
    [InverseProperty(nameof(ArbitrationDiscussionSign.Owner))]
    public virtual ICollection<ArbitrationDiscussionSign> Signs { get; set; }
    [InverseProperty(nameof(ArbitrationDiscussionFile.Owner))]
    public virtual ICollection<ArbitrationDiscussionFile> Files { get; set; } 
}
