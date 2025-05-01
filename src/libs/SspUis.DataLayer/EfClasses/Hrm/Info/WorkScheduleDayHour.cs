using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_work_schedule_day_hour", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_info_work_schedule_day_hour_owner_idx")]
    public partial class WorkScheduleDayHour : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("day_number")]
        public int DayNumber { get; set; }
        [Column("is_day_off")]
        public bool IsDayOff { get; set; }
        [Required]
        [Column("begin_at")]
        [StringLength(10)]
        public string BeginAt { get; set; }
        [Required]
        [Column("end_at")]
        [StringLength(10)]
        public string EndAt { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(WorkSchedule.DayHours))]
        public virtual WorkSchedule Owner { get; set; }
    }
}
