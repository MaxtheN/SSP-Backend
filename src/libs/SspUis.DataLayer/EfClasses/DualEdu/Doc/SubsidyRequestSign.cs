using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu.Doc;



[Table("doc_subsidy_request_sign", Schema = "dual_edu")]
[Index(nameof(OwnerId), Name = "doc_subsidy_request_sign")]
public partial class SubsidyRequestSign : IHaveIdProp<long>
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
    [InverseProperty(nameof(SubsidyRequest.Signs))]
    public virtual SubsidyRequest Owner { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
}


