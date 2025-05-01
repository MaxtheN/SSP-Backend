using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("enum_missed_days_type_translate", Schema = "hrm")]
    public partial class MissedDaysTypeTranslate : TranslateEntity<MissedDaysTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MissedDaysType.Translates))]
        public virtual MissedDaysType Owner { get; set; }
    }
}
