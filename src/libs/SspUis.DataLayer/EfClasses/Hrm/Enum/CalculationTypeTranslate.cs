using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("enum_calculation_type_translate", Schema = "hrm")]
public partial class CalculationTypeTranslate : TranslateEntity<CalculationTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(CalculationType.Translates))]
    public virtual CalculationType Owner { get; set; }
}
