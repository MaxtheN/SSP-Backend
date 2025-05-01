using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_timesheet_type_translate", Schema = "hrm")]
    public partial class TimesheetTypeTranslate :EnumTranslateEntity<TimesheetTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(TimesheetType.Translates))]
        public virtual TimesheetType Owner { get; set; }
    }
}
