using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEBASE.Models;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Corruption;
using System;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_join_anti_corruption_result_sign", Schema = "corruption")]
[Index(nameof(OwnerId), Name = "ix_doc_join_anti_corruption_result_sign__owner")]
public partial class JoinAntiCorruptionResultSign : IHaveIdProp<long>
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
    [Column("signed_user_info")]
    public string SignedUserInfo { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("sign_user_id")]
    public int SignUserId { get; set; }
    [Column("signed_at", TypeName = "timestamp without time zone")]
    public DateTime? SignedAt { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(JoinAntiCorruptionResult.Signs))]
    public virtual JoinAntiCorruptionResult Owner { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
}