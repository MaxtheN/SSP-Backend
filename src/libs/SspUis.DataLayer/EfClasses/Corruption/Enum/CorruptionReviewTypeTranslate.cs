using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Corruption
{
    [Table("enum_corruption_review_type_translate", Schema = "corruption")]
    public partial class CorruptionReviewTypeTranslate : EnumTranslateEntity<CorruptionReviewTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(CorruptionReviewType.Translates))]
        public virtual CorruptionReviewType Owner { get; set; }
    }
}
