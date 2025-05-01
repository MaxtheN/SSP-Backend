using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_work_schedule_translate", Schema = "hrm")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_work_schedule_translate__lang", IsUnique = true)]
    public partial class WorkScheduleTranslate : TranslateEntity<WorkScheduleTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(WorkSchedule.Translates))]
        public virtual WorkSchedule Owner { get; set; }
    }
}
