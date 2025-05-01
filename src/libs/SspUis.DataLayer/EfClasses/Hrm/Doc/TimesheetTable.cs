using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Hrm;
//using SspUis.DataLayer.Interfaces;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_timesheet_table", Schema = "hrm")]
public partial class TimesheetTable : IHaveIdProp<long>, IHaveIsDeleted
{
    public TimesheetTable()
    {
        TableDays = new HashSet<TimesheetTableDay>();
        TempTableDays = new HashSet<TimesheetTableDay>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("department_id")]
    public int? DepartmentId { get; set; }
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("position_id")]
    public int PositionId { get; set; }
    [Column("employee_manage_id")]
    public long? EmployeeManageId { get; set; }
    [Column("work_schedule_id")]
    public int WorkScheduleId { get; set; }
    [Column("employment_type_id")]
    public int EmploymentTypeId { get; set; }
    [Column("start_on")]
    public DateOnly? StartOn { get; set; }
    [Column("end_on")]
    public DateOnly? EndOn { get; set; }
    [Column("employment_rate")]
    public decimal EmploymentRate { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }
    [Column("plan_days")]
    public int PlanDays { get; set; }
    [Column("plan_hours")]
    public decimal PlanHours { get; set; }
    [Column("fact_days")]
    public int FactDays { get; set; }
    [Column("fact_hours")]
    public decimal FactHours { get; set; }
    [Column("day_off_hours")]
    public decimal DayOffHours { get; set; }
    [Column("night_hours")]
    public decimal NightHours { get; set; }
    [Column("hourly")]
    public decimal? Hourly { get; set; }
    [Column("maintenance_hours")]
    public decimal? MaintenanceHours { get; set; }
    [Column("experience_percentage")]
    public decimal? ExperiencePercentage { get; set; }
    [Column("document_id")]
    public long? DocumentId { get; set; }
    [Column("document_table_id")]
    public int? DocumentTableId { get; set; }
    [Column("document_info")]
    [StringLength(300)]
    public string DocumentInfo { get; set; }
    /// <summary>
    /// hodimni ishga olish hujjati CANCEL boganda, SysEmployeeManage dan zapis ochadi. Usha payt FK ga urishmasligi uchun, hujjat DELETED boganda EmployeeManageId`ni trigger bilan shu ustunga yozib qoyamiz
    /// </summary>
    [Column("temp_employee_manage_id")]
    public long? TempEmployeeManageId { get; set; }
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; }
    [ForeignKey(nameof(EmployeeManageId))]
    public virtual EmployeeManage EmployeeManage { get; set; }
    [ForeignKey(nameof(EmploymentTypeId))]
    public virtual EmploymentType EmploymentType { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual Timesheet Owner { get; set; }
    [ForeignKey(nameof(PositionId))]
    public virtual Position Position { get; set; }
    [ForeignKey(nameof(WorkScheduleId))]
    public virtual WorkSchedule WorkSchedule { get; set; }
    [InverseProperty(nameof(TimesheetTableDay.Owner))]
    public virtual ICollection<TimesheetTableDay> TableDays { get; set; }
    [NotMapped]
    public ICollection<TimesheetTableDay> TempTableDays { get; set; }
    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}
