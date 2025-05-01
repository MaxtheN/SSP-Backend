using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("info_question_group_translate", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_question_group_translate_lang", IsUnique = true)]
public partial class QuestionGroupTranslate : TranslateEntity<QuestionGroupTranslate, TranslateQuestionGroup, int>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(QuestionGroup.Translates))]
    public virtual QuestionGroup Owner { get; set; }
}
