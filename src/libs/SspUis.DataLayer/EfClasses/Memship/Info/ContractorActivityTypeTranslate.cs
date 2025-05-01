using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_contractor_activity_type_translate", Schema = "memship")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_contractor_activity_type_translate__owner_lang", IsUnique = true)]
    public partial class ContractorActivityTypeTranslate : EnumTranslateEntity<ContractorActivityTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ContractorActivityType.Translates))]
        public virtual ContractorActivityType Owner { get; set; }
    }
}
