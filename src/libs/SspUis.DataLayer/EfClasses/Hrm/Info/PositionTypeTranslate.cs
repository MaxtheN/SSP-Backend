using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_position_type_translate", Schema = "hrm")]
    public partial class PositionTypeTranslate : TranslateEntity<PositionTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(PositionType.Translates))]
        public virtual PositionType Owner { get; set; }
    }
}
