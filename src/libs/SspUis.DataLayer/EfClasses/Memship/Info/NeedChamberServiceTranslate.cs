using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_need_chamber_service_translate", Schema = "public")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_need_chamber_service_translate__owner_lang", IsUnique = true)]
    public partial class NeedChamberServiceTranslate : EnumTranslateEntity<NeedChamberServiceTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(NeedChamberService.Translates))]
        public virtual NeedChamberService Owner { get; set; }
    }
}
