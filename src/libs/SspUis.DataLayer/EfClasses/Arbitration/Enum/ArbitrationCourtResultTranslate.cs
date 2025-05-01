using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_arbitration_court_result_translate", Schema = "arbitration")]
public partial class ArbitrationCourtResultTranslate : EnumTranslateEntity<ArbitrationCourtResultTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ArbitrationCourtResult.Translates))]
    public virtual ArbitrationCourtResult Owner { get; set; }
}
