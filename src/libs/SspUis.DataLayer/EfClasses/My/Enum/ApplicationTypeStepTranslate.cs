using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_application_type_step_translate", Schema = "my")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_language_degree_translate__owner_lang", IsUnique = true)]
public partial class ApplicationTypeStepTranslate : EnumTranslateEntity<ApplicationTypeStepTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ApplicationTypeStep.Translates))]
    public virtual ApplicationTypeStep Owner { get; set; }
}
