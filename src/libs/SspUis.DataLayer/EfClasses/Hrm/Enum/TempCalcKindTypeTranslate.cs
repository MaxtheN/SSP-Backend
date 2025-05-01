using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("enum_temp_calc_kind_type_translate", Schema = "hrm")]
    public partial class TempCalcKindTypeTranslate : TranslateEntity<TempCalcKindTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(TempCalcKindType.Translates))]
        public virtual TempCalcKindType Owner { get; set; }
    }
}
