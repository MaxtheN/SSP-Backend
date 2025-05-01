using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("enum_work_schedule_kind_translate", Schema = "hrm")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_work_schedule_kind_translate__lang", IsUnique = true)]
    public partial class WorkScheduleKindTranslate : TranslateEntity<WorkScheduleKindTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(WorkScheduleKind.Translates))]
        public virtual WorkScheduleKind Owner { get; set; }
    }
}
