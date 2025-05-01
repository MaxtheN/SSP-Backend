using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("info_specialty_translate", Schema = "dual_edu")]
public partial class SpecialtyTranslate : TranslateEntity<SpecialtyTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Specialty.Translates))]
    public virtual Specialty Owner { get; set; }
}
