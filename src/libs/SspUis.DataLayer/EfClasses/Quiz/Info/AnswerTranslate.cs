using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("info_answer_translate", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_answer_translate_lang", IsUnique = true)]
public partial class AnswerTranslate : TranslateEntity<AnswerTranslate, TranslateAnswer, long>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Answer.Translates))]
    public virtual Answer Owner { get; set; }
}
