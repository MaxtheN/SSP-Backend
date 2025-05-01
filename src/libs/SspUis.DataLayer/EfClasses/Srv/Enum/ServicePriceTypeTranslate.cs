using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_service_price_type_translate", Schema = "srv")]
    [Index(nameof(OwnerId), nameof(ColumnName), nameof(LanguageId), Name = "ux_enum_service_price_type_translate__owner_lang", IsUnique = true)]
    public class ServicePriceTypeTranslate : TranslateEntity<ServicePriceTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ServicePriceType.Translates))]
        public virtual ServicePriceType Owner { get; set; }
    }
}