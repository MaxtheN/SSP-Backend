using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_appoint_employee", Schema = "hrm")]
public partial class AppointEmployee : IHaveIdProp<long>, IHaveStatusId
{
    public AppointEmployee()
    {
        Signer = new HashSet<AppointEmployeeSigner>();
        Signs = new HashSet<AppointEmployeeSign>();
        Tables = new HashSet<AppointEmployeeTable>();
        Files = new HashSet<AppointEmployeeFile>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("id2")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id2 { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(250)]
    public string DocNumber { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }
    [Column("conclusion_for_print")]
    public string ConclusionForPrint { get; set; }
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
    public string Message { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [JsonIgnore]
    [InverseProperty(nameof(AppointEmployeeSign.Owner))]
    public virtual ICollection<AppointEmployeeSign> Signs { get; set; }
    [JsonIgnore]
    [InverseProperty(nameof(AppointEmployeeSigner.Owner))]
    public virtual ICollection<AppointEmployeeSigner> Signer { get; set; }
    [InverseProperty(nameof(AppointEmployeeTable.Owner))]
    public virtual ICollection<AppointEmployeeTable> Tables { get; set; }
    [InverseProperty(nameof(AppointEmployeeFile.Owner))]
    public virtual ICollection<AppointEmployeeFile> Files { get; set; }
}