using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Corruption
{
    [Table("enum_join_anti_corruption_result_type_translate", Schema = "corruption")]
    public partial class JoinAntiCorruptionResultTypeTranslate : EnumTranslateEntity<JoinAntiCorruptionResultTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(JoinAntiCorruptionResultType.Translates))]
        public virtual JoinAntiCorruptionResultType Owner { get; set; }
    }
}
