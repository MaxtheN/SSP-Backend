using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("enum_minimum_value_type_translate", Schema = "hrm")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_employment_type_translate__lang", IsUnique = true)]
    public partial class MinimumValueTypeTranslate : TranslateEntity<MinimumValueTypeTranslate, TranslateColumn>
    {

        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MinimumValueType.Translates))]
        public virtual MinimumValueType Owner { get; set; }
    }
}
