using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_staff_type_basic_tariff_translate", Schema = "hrm")]
//[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_staff_type_basic_tariff_translate__owner_lang", IsUnique = true)]
public partial class StaffTypeBasicTariffTranslate : TranslateEntity<StaffTypeBasicTariffTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(StaffTypeBasicTariff.Translates))]
    public virtual StaffTypeBasicTariff Owner { get; set; }
}
