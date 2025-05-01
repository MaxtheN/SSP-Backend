using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("enum_calculation_method_translate", Schema = "hrm")]
public partial class CalculationMethodTranslate : TranslateEntity<CalculationMethodTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual CalculationMethod Owner { get; set; }
}
