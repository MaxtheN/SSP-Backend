using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_position_classification_translate", Schema = "hrm")]
public partial class PositionClassificationTranslate : TranslateEntity<PositionClassificationTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(PositionClassification.Translates))]
    public virtual PositionClassification Owner { get; set; }
}
