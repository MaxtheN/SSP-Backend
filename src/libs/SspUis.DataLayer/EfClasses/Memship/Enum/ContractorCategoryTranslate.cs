using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_contractor_category_translate", Schema = "memship")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_contractor_category_translate__owner_lang", IsUnique = true)]
    public partial class ContractorCategoryTranslate : EnumTranslateEntity<ContractorCategoryTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ContractorCategory.Translates))]
        public virtual ContractorCategory Owner { get; set; }
    }
}
