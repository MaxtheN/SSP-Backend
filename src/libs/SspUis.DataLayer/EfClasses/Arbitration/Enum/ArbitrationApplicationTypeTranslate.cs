using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace SspUis.DataLayer.EfClasses;

[Table("enum_arbitration_application_type_translate", Schema = "arbitration")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_arbitration_application_type_translate__owner_lang", IsUnique = true)]
public partial class ArbitrationApplicationTypeTranslate : EnumTranslateEntity<ArbitrationApplicationTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ArbitrationApplicationType.Translates))]
    public virtual ArbitrationApplicationType Owner { get; set; }
}