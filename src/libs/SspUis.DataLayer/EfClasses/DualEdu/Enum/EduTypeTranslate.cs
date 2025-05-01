using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.DualEdu
{
    [Table("enum_edu_type_translate", Schema = "dual_edu")]
    public partial class EduTypeTranslate : TranslateEntity<EduTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(EduType.Translates))]
        public virtual EduType Owner { get; set; }
    }
}