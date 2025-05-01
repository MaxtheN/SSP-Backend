using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_qualification_category_translate", Schema = "hrm")]
public partial class QualificationCategoryTranslate : TranslateEntity<QualificationCategoryTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(QualificationCategory.Translates))]
    public virtual QualificationCategory Owner { get; set; }
}
