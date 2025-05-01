using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("doc_dual_application", Schema = "dual_edu")]
[Index(nameof(ApplicationId), Name = "uc_application_id", IsUnique = true)]
public partial class DualApplication : IHaveIdProp<long>, IBaseApplicationEntity
{
    public DualApplication()
    {
        Tables = new HashSet<DualApplicationTable>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("id2")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id2 { get; set; }
    [Column("application_id")]
    public long ApplicationId { get; set; }
    [Column("dual_education_type_id")]
    public int DualEducationTypeId { get; set; }
    [Column("message")]
    [StringLength(1024)]
    public string Message { get; set; }
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
    [ForeignKey(nameof(ApplicationId))]
    [InverseProperty(nameof(EfClasses.Application.DualApplication))]
    public virtual Application Application { get; set; }
    [ForeignKey(nameof(CreatedUserId))]
    public virtual BusinessmanUser CreatedUser { get; set; }
    [ForeignKey(nameof(DualEducationTypeId))]
    public virtual DualEducationType DualEducationType { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [InverseProperty(nameof(DualApplicationTable.Owner))]
    public virtual ICollection<DualApplicationTable> Tables { get; set; }
}
