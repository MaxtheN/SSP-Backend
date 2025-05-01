using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_mediation_result_translate", Schema = "claim")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_mediation_result_translate__owner_lang", IsUnique = true)]
    public partial class MediationResultTranslate : EnumTranslateEntity<MediationResultTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MediationResult.Translates))]
        public virtual MediationResult Owner { get; set; }
    }
}
