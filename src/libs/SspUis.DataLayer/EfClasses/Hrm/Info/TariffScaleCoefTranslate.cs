using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_tariff_scale_coef_translate", Schema = "hrm")]
public partial class TariffScaleCoefTranslate : TranslateEntity<TariffScaleCoefTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(TariffScaleCoef.Translates))]
    public virtual TariffScaleCoef Owner { get; set; }
}
