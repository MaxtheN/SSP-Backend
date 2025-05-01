using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_need_chamber_service_group_translate", Schema = "public")]
    public partial class NeedChamberServiceGroupTranslate : TranslateEntity<NeedChamberServiceGroupTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(NeedChamberServiceGroup.Translates))]
        public virtual NeedChamberServiceGroup Owner { get; set; }
    }
}