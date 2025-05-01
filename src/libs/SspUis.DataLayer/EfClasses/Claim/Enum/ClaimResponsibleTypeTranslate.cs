using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_claim_responsible_type_translate", Schema = "claim")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_claim_responsible_type_translate__owner_lang", IsUnique = true)]
    public partial class ClaimResponsibleTypeTranslate : EnumTranslateEntity<ClaimResponsibleTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ClaimResponsibleType.Translates))]
        public virtual ClaimResponsibleType Owner { get; set; }
    }
}
