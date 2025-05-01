using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("info_institute_translate", Schema = "dual_edu")]
public partial class InstituteTranslate : TranslateEntity<InstituteTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Institute.Translates))]
    public virtual Institute Owner { get; set; }
}
