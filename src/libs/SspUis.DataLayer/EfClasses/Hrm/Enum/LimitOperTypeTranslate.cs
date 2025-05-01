using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("enum_limit_oper_type_translate", Schema = "hrm")]
public partial class LimitOperTypeTranslate : TranslateEntity<LimitOperTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(LimitOperType.Translates))]
    public virtual LimitOperType Owner { get; set; }
}
