using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("info_questionnaire_translate", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_questionnaire_translate_lang", IsUnique = true)]
public partial class QuestionnaireTranslate : TranslateEntity<QuestionnaireTranslate, TranslateQuestionnaire, long>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Questionnaire.Translates))]
    public virtual Questionnaire Owner { get; set; }
}
