using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_employee_sick_leave_table", Schema = "hrm")]
    public class EmployeeSickLeaveTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("department_id")]
        public int DepartmentId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("temp_employee_manage_id")]
        public long? TempEmployeeManageId { get; set; }
        [Column("employee_manage_id")]
        public long? EmployeeManageId { get; set; }
        [Column("give_on")]
        public DateOnly GiveOn { get; set; }
        [Required]
        [Column("document_seria")]
        [StringLength(5)]
        public string DocumentSeria { get; set; }
        [Required]
        [Column("document_number")]
        [StringLength(10)]
        public string DocumentNumber { get; set; }
        [Column("detail_for_print")]
        public string DetailForPrint { get; set; }
        [Required]
        [Column("diagnosis")]
        [StringLength(300)]
        public string Diagnosis { get; set; }
        [Required]
        [Column("given_organization")]
        [StringLength(300)]
        public string GivenOrganization { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("end_on")]
        public DateOnly EndOn { get; set; }
        [Column("year_work_exp")]
        public int YearWorkExp { get; set; }
        [Column("calc_perc")]
        [Precision(18, 2)]
        public decimal CalcPerc { get; set; }
        [Column("is_maternity_leave")]
        public bool IsMaternityLeave { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(EmployeeSickLeave.Tables))]
        public virtual EmployeeSickLeave Owner { get; set; }
    }
}
