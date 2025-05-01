using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_claim_application_type_translate", Schema = "claim")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_claim_application_type_translate__owner_lang", IsUnique = true)]
public partial class ClaimApplicationTypeTranslate : EnumTranslateEntity<ClaimApplicationTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ClaimApplicationType.Translates))]
    public virtual ClaimApplicationType Owner { get; set; }
}
