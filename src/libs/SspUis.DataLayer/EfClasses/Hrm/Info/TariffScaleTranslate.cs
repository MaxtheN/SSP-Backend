using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_tariff_scale_translate", Schema = "hrm")]
public partial class TariffScaleTranslate : TranslateEntity<TariffScaleTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(TariffScale.Translates))]
    public virtual TariffScale Owner { get; set; }
}
