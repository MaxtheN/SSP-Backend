using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_employee_missed_day_table", Schema = "hrm")]
    public partial class EmployeeMissedDayTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("employee_manage_id")]
        public long EmployeeManageId { get; set; }
        [Column("missed_days_type_id")]
        public int MissedDaysTypeId { get; set; }
        [Column("missed_days")]
        public int MissedDays { get; set; }
        [Column("start_at", TypeName = "timestamp without time zone")]
        public DateTime StartAt { get; set; }
        [Column("end_at", TypeName = "timestamp without time zone")]
        public DateTime EndAt { get; set; }
        [Column("without_reason")]
        public bool? WithoutReason { get; set; }

        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }
        [ForeignKey(nameof(MissedDaysTypeId))]
        public virtual MissedDaysType MissedDaysType { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual EmployeeMissedDay Owner { get; set; }
    }
}
