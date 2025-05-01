using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_language_degree_translate")]
    public partial class LanguageDegreeTranslate : EnumTranslateEntity<LanguageDegreeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(LanguageDegree.Translates))]
        public virtual LanguageDegree Owner { get; set; }
    }
}
