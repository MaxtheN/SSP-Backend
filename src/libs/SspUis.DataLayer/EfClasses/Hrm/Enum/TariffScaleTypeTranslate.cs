using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_tariff_scale_type_translate", Schema = "hrm")]
    public partial class TariffScaleTypeTranslate : TranslateEntity<TariffScaleTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(TariffScaleType.Translates))]
        public virtual TariffScaleType Owner { get; set; }
    }
}