using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_timesheet", Schema = "hrm")]
public partial class Timesheet : IHaveIdProp<long>, IHaveStatusId
{
    public Timesheet()
    {
        Tables = new HashSet<TimesheetTable>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(50)]
    public string DocNumber { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Column("month_on")]
    public DateTime MonthOn { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }
    [Column("department_id")]
    public int? DepartmentId { get; set; }
    [Column("timesheet_type_id")]
    public int TimesheetTypeId { get; set; }
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
    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [ForeignKey(nameof(TimesheetTypeId))]
    [InverseProperty(nameof(EfClasses.TimesheetType.Timesheets))]
    public virtual TimesheetType TimesheetType { get; set; }
    [InverseProperty(nameof(TimesheetTable.Owner))]
    public virtual ICollection<TimesheetTable> Tables { get; set; }
}
