using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_recall_leave", Schema = "hrm")]
public partial class RecallLeave : IHaveIdProp<long>, IHaveStatusId
{
    public RecallLeave()
    {
        Tables = new HashSet<RecallLeaveTable>();
        Signer = new HashSet<RecallLeaveSigner>();
        Files = new HashSet<ReCallLeaveFile>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("id2")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id2 { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(30)]
    public string DocNumber { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
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
    [Column("message")]
    public string? Message { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [InverseProperty(nameof(RecallLeaveTable.Owner))]
    public virtual ICollection<RecallLeaveTable> Tables { get; set; }
    [JsonIgnore]
    [InverseProperty(nameof(RecallLeaveSigner.Owner))]
    public virtual ICollection<RecallLeaveSigner> Signer { get; set; }
    [InverseProperty(nameof(ReCallLeaveFile.Owner))]
    public virtual ICollection<ReCallLeaveFile> Files { get; set; }
}
