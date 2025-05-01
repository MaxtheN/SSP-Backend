using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_work_schedule", Schema = "hrm")]
    public partial class WorkSchedule : IHaveIdProp<int>, IHaveStateId
    {
        public WorkSchedule()
        {
            DayHours = new HashSet<WorkScheduleDayHour>();
            Translates = new HashSet<WorkScheduleTranslate>();
            WorkHours = new HashSet<WorkScheduleWorkHour>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("work_schedule_kind_id")]
        public int WorkScheduleKindId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(WorkScheduleKindId))]
        public virtual WorkScheduleKind WorkScheduleKind { get; set; }
        [InverseProperty(nameof(WorkScheduleDayHour.Owner))]
        public virtual ICollection<WorkScheduleDayHour> DayHours { get; set; }
        [InverseProperty(nameof(WorkScheduleTranslate.Owner))]
        public virtual ICollection<WorkScheduleTranslate> Translates { get; set; }
        [InverseProperty(nameof(WorkScheduleWorkHour.Owner))]
        public virtual ICollection<WorkScheduleWorkHour> WorkHours { get; set; }
    }
}
