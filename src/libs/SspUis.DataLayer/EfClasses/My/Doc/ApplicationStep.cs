using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_application_step", Schema = "my")]
[Index(nameof(ApplicationId), Name = "ix_application_id")]
public partial class ApplicationStep : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("application_id")]
    public long ApplicationId { get; set; }
    [Column("application_type_step_id")]
    public int ApplicationTypeStepId { get; set; }
    [Column("message")]
    [StringLength(250)]
    public string Message { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(ApplicationId))]
    //[InverseProperty(nameof(EfClasses.Application.Steps))]
    public virtual Application Application { get; set; }

    public virtual ApplicationTypeStep ApplicationTypeStep { get; set; }

}
