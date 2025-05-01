using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_staffing_indicator_translate", Schema = "hrm")]
    public partial class StaffingIndicatorTranslate : TranslateEntity<StaffingIndicatorTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(StaffingIndicator.Translates))]
        public virtual StaffingIndicator Owner { get; set; }
    }
}
