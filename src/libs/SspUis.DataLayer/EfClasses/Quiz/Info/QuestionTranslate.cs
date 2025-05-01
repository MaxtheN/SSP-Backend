using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("info_question_translate", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_question_translate_lang", IsUnique = true)]
public partial class QuestionTranslate : TranslateEntity<QuestionTranslate, TranslateQuestion, int>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Question.Translates))]
    public virtual Question Owner { get; set; }
}
