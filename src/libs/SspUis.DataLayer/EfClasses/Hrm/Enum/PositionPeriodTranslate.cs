using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("enum_position_period_translate", Schema = "hrm")]
    public partial class PositionPeriodTranslate : TranslateEntity<PositionPeriodTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(PositionPeriod.Translates))]
        public virtual PositionPeriod Owner { get; set; }
    }
}
