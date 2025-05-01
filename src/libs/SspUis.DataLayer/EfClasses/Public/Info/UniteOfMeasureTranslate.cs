using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer;

[Table("info_unite_of_measure_translate", Schema = "public")]
public partial class UniteOfMeasureTranslate : TranslateEntity<UniteOfMeasureTranslate, TranslateColumn>
{
	[ForeignKey(nameof(LanguageId))]
	public virtual Language Language { get; set; }
	[ForeignKey(nameof(OwnerId))]
	[InverseProperty(nameof(UniteOfMeasure.Translates))]
	public virtual UniteOfMeasure Owner { get; set; }
}
