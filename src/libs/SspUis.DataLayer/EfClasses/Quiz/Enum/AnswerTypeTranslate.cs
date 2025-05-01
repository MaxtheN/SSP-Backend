using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Quiz.Enum;

[Table("enum_answer_type_translate", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_answer_type_translate__lang", IsUnique = true)]
public partial class AnswerTypeTranslate : EnumTranslateEntity<AnswerTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(AnswerType.Translates))]
    public virtual AnswerType Owner { get; set; }
}
