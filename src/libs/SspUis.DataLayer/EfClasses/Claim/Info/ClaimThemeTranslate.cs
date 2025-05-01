using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Claim
{
    [Table("info_claim_theme_translate", Schema = "claim")]
    public partial class ClaimThemeTranslate : EnumTranslateEntity<ClaimThemeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ClaimTheme.Translates))]
        public virtual ClaimTheme Owner { get; set; }
    }
}
