using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("enum_calculate_by_time_type_translate", Schema = "hrm")]
public partial class CalculateByTimeTypeTranslate : TranslateEntity<CalculateByTimeTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(CalculateByTimeType.Translates))]
    public virtual CalculateByTimeType Owner { get; set; }
}
