using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_arbitration_result_sign", Schema = "arbitration")]
[Index(nameof(OwnerId), Name = "ix_doc_arbitration_result_sign__owner")]
public partial class ArbitrationResultSign : IHaveIdProp<long>/*, IHaveStatusId*/
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("sign_file")]
    public Guid? SignFile { get; set; }
    [Column("data_file")]
    public Guid? DataFile { get; set; }
    [Required]
    [Column("arbitration_judge_id")]
    public int? ArbitrationJudgeId { get; set; }
    [Required]
    [Column("signed_user_info")]
    [StringLength(500)]
    public string? SignedUserInfo { get; set; }
    [Column("signed_at", TypeName = "timestamp without time zone")]
    public DateTime? SignedAt { get; set; }
    [Column("status_id")]
    public int? StatusId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ArbitrationResult.Signs))]
    public virtual ArbitrationResult Owner { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status? Status { get; set; }
    [ForeignKey(nameof(ArbitrationJudgeId))]
    public virtual ArbitrationJudge? ArbitrationJudge { get; set; }
}
