using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_inspection_type_translate", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_inspection_type_translate__lang", IsUnique = true)]
public partial class InspectionTypeTranslate : EnumTranslateEntity<InspectionTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(InspectionType.Translates))]
    public virtual InspectionType Owner { get; set; }
}
