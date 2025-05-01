using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("info_settlement_account_source_translate")]
public partial class SettlementAccountSourceTranslate : TranslateEntity<SettlementAccountSourceTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(SettlementAccountSource.Translates))]
    public virtual SettlementAccountSource Owner { get; set; }
}
