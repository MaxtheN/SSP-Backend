using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_work_day_off", Schema = "hrm")]
public partial class WorkDayOff : IHaveIdProp<long>, IHaveStatusId
{
    public WorkDayOff()
    {
        Signer = new HashSet<WorkDayOffSigner>();
        Tables = new HashSet<WorkDayOffTable>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
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
    [InverseProperty(nameof(WorkDayOffTable.Owner))]
    public virtual ICollection<WorkDayOffTable> Tables { get; set; }
    [InverseProperty(nameof(WorkDayOffSigner.Owner))]
    public virtual ICollection<WorkDayOffSigner> Signer { get; set; }
}
