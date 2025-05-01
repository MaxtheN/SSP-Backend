using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("info_indicator_translate", Schema ="kpi")]
public  class IndicatorTranslate : TranslateEntity<IndicatorTranslate,TranslateColumn>
{
	[ForeignKey(nameof(LanguageId))]
	public virtual Language Language { get; set; }
	[ForeignKey(nameof(OwnerId))]
	[InverseProperty(nameof(Indicator.Translates))]
	public virtual Indicator Owner { get; set; }
}
