using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("sys_employee_turnstile_log", Schema = "hrm")]
[Index(nameof(OrganizationId), Name = "ix_sys_employee_turnstile_org_id")]
[Index(nameof(EventAt), Name = "ix_sys_employee_turnstile_event_at")]
[Index(nameof(EmployeeId), Name = "ix_sys_employee_turnstile_emp_id")]
public class EmployeeTurnstileLog : IHaveIdProp<Guid>
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("event_at", TypeName = "timestamp without time zone")]
    public DateTime EventAt { get; set; }
    [Column("event_on")]
    public DateOnly EventOn { get; set; }
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("date_of_created", TypeName = "timestamp without time zone")]
    public DateTime DateOfCreated { get; set; }
    [Column("employee_turnstile_log_type_id")]
    public int EmployeeTurnstileLogTypeId { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; }
    [ForeignKey(nameof(EmployeeTurnstileLogTypeId))]
    public virtual EmployeeTurnstileLogType EmployeeTurnstileLogType { get; set; }
}
