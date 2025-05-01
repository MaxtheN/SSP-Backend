using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_meeting_type_translate", Schema = "public")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_meeting_type_translate__owner_lang", IsUnique = true)]
    public class MeetingTypeTranslate : EnumTranslateEntity<MeetingTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MeetingType.Translates))]
        public virtual MeetingType Owner { get; set; }
    }
}
