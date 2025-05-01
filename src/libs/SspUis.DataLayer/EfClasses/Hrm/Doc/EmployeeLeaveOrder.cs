using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_employee_leave_order", Schema = "hrm")]
public partial class EmployeeLeaveOrder : IHaveIdProp<long>, IHaveStatusId
{
    public EmployeeLeaveOrder()
    {
        Signer = new HashSet<EmployeeLeaveOrderSigner>();
        Tables = new HashSet<EmployeeLeaveOrderTable>();
        Files = new HashSet<EmployeeLeaveOrderFile>();
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
    [Column("conclusion_for_print")]
    public string ConclusionForPrint { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("employee_sick_leave_type_id")]
    public int? EmployeeSickLeaveTypeId { get; set; }
    [Column("employee_sick_leave_id")]
    public long? EmployeeSickLeaveId { get; set; }
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
    [ForeignKey(nameof(EmployeeSickLeaveTypeId))]
    public virtual EmployeeSickLeaveType EmployeeSickLeaveType { get; set; }
    [ForeignKey(nameof(EmployeeSickLeaveId))]
    public virtual EmployeeSickLeave EmployeeSickLeave { get; set; }
    [InverseProperty(nameof(EmployeeLeaveOrderTable.Owner))]
    public virtual ICollection<EmployeeLeaveOrderTable> Tables { get; set; }
    [InverseProperty(nameof(EmployeeLeaveOrderSigner.Owner))]
    public virtual ICollection<EmployeeLeaveOrderSigner> Signer { get; set; }
    [InverseProperty(nameof(EmployeeLeaveOrderFile.Owner))]
    public virtual ICollection<EmployeeLeaveOrderFile> Files { get; set; }
}
