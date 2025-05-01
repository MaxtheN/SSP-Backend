using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{

    [Table("enum_claim_need_court_translate", Schema = "claim")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_claim_need_court_translate__owner_lang", IsUnique = true)]
    public partial class ClaimNeedCourtTranslate : EnumTranslateEntity<ClaimNeedCourtTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ClaimNeedCourt.Translates))]
        public virtual ClaimNeedCourt Owner { get; set; }
    }
}