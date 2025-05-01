using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_language_proficiency_translate", Schema = "hrm")]
    public partial class LanguageProficiencyTranslate : TranslateEntity<LanguageProficiencyTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(LanguageProficiency.Translates))]
        public virtual LanguageProficiency Owner { get; set; }
    }
}
