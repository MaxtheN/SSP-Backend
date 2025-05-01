using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("info_dual_education_type_translate", Schema = "dual_edu")]
public partial class DualEducationTypeTranslate : TranslateEntity<DualEducationTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(DualEducationType.Translates))]
    public virtual DualEducationType Owner { get; set; }
}
