using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_timesheet_indicator_translate", Schema = "hrm")]
    public partial class TimesheetIndicatorTranslate : TranslateEntity<TimesheetIndicatorTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(TimesheetIndicator.Translates))]
        public virtual TimesheetIndicator Owner { get; set; }
    }
}
