using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_arbitration_court_translate", Schema = "arbitration")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_arbitration_application_type_translate__owner_lang", IsUnique = true)]
public partial class ArbitrationCourtTranslate : EnumTranslateEntity<ArbitrationCourtTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ArbitrationCourt.Translates))]
    public virtual ArbitrationCourt Owner { get; set; }
}
