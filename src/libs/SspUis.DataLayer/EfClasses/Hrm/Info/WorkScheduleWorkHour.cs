using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_work_schedule_work_hour", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_info_work_schedule_work_hour_owner_idx")]
    public partial class WorkScheduleWorkHour : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("date_on")]
        public DateOnly DateOn { get; set; }
        [Column("days")]
        public int Days { get; set; }
        [Column("hours")]
        public decimal Hours { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(WorkSchedule.WorkHours))]
        public virtual WorkSchedule Owner { get; set; }
    }
}
