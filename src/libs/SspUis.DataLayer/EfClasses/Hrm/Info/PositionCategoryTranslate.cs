using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_position_category_translate", Schema = "hrm")]
public partial class PositionCategoryTranslate : TranslateEntity<PositionCategoryTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(PositionCategory.Translates))]
    public virtual PositionCategory Owner { get; set; }
}
