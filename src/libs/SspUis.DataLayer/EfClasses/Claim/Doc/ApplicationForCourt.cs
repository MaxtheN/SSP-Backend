using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Claim;

[Table("doc_application_for_court", Schema = "claim")]
public partial class ApplicationForCourt : IHaveIdProp<long>, IHaveStatusId
{
    public ApplicationForCourt()
    {
        Files = new HashSet<ApplicationForCourtFile>();
        Signs = new HashSet<ApplicationForCourtSign>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("id2")]
    public Guid Id2 { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(50)]
    public string DocNumber { get; set; }
    [Column("mediation_id")]
    public long? MediationId { get; set; }
    [Column("application_id")]
    public long? ApplicationId { get; set; }
    [Column("current_step_id")]
    public int? StepId { get; set; }

    //////////////////////////////////////////////////////
    [Required]
    [Column("position_id")]
    public int PositionId { get; set; }
    [ForeignKey(nameof(PositionId))]
    public virtual Position Position { get; set; }

    [Required]
    [Column("employee_manage_id")]
    public long EmployeeManageId { get; set; }
    [ForeignKey(nameof(EmployeeManageId))]
    public virtual EmployeeManage EmployeeManage { get; set; }

    [Required]
    [Column("department_id")]
    public int DepartmentId { get; set; }
    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
    //////////////////////////////////////////////////////

    [Column("claim_organization_id")]
    public int ClaimOrganizationId { get; set; }

    [Column("contractor_id")]
    public long ContractorId { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("claim_application_for_court_type_id")]
    public int? ClaimApplicationForCourtTypeId { get; set; }
    [Column("message")]
    [StringLength(500)]
    public string Message { get; set; }
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
    [ForeignKey(nameof(ClaimOrganizationId))]
    public virtual ClaimOrganization ClaimOrganization { get; set; }
    [ForeignKey(nameof(ApplicationId))]
    public virtual Application Application { get; set; }
    [ForeignKey(nameof(ContractorId))]
    public virtual Contractor Contractor { get; set; }
    [ForeignKey(nameof(MediationId))]
    public virtual Mediation Mediation { get; set; }
    [ForeignKey(nameof(StepId))]
    public virtual ApplicationTypeStep Step { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [ForeignKey(nameof(ClaimApplicationForCourtTypeId))]
    public virtual ClaimApplicationForCourtType ClaimApplicationForCourtType { get; set; }
    [InverseProperty(nameof(ApplicationForCourtFile.Owner))]
    public virtual ICollection<ApplicationForCourtFile> Files { get; set; }
    [InverseProperty(nameof(ApplicationForCourtSign.Owner))]
    public virtual ICollection<ApplicationForCourtSign> Signs { get; set; }
}
