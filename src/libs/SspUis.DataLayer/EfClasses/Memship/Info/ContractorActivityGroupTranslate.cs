using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_contractor_activity_group_translate", Schema = "memship")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_contractor_activity_group_translate__owner_lang", IsUnique = true)]
    public partial class ContractorActivityGroupTranslate : EnumTranslateEntity<ContractorActivityGroupTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ContractorActivityGroup.Translates))]
        public virtual ContractorActivityGroup Owner { get; set; }
    }
}
