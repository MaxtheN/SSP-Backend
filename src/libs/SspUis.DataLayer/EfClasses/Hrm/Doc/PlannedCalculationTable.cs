using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_planned_calculation_table", Schema = "hrm")]
    public partial class PlannedCalculationTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("percentage")]
        [Precision(18, 2)]
        public decimal? Percentage { get; set; }
        [Column("amount")]
        [Precision(18, 2)]
        public decimal? Amount { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("end_date")]
        public DateOnly? EndDate { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("employee_manage_id")]
        public long? EmployeeManageId { get; set; }
        [Column("position_id")]
        public int PositionId { get; set; }
        [Column("temp_employee_manage_id")]
        public long? TempEmployeeManageId { get; set; }
        [Column("temp_calc_kind_type_id")]
        public int TempCalcKindTypeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(PlannedCalculation.Tables))]
        public virtual PlannedCalculation Owner { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual Position Position { get; set; }
        [ForeignKey(nameof(TempCalcKindTypeId))]
        public virtual TempCalcKindType TempCalcKindType { get; set; }
    }
}
