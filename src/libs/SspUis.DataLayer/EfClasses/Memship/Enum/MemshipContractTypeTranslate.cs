using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_memship_contract_type_translate", Schema = "memship")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_memship_contract_type_translate__owner_lang", IsUnique = true)]
    public partial class MemshipContractTypeTranslate : EnumTranslateEntity<MemshipContractTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MemshipContractType.Translates))]
        public virtual MemshipContractType Owner { get; set; }
    }
}
