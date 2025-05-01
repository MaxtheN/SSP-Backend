using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_education_item_translate")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_education_item_translate__owner_lang", IsUnique = true)]
    public partial class EducationItemTranslate : TranslateEntity<EducationItemTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(EducationItem.Translates))]
        public virtual EducationItem Owner { get; set; }
    }
}
