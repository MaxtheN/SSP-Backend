using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Corruption
{
    [Table("info_contractor_union_activity_type_translate", Schema = "corruption")]
    public partial class ContractorUnionActivityTypeTranslate : TranslateEntity<ContractorUnionActivityTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ContractorUnionActivityType.Translates))]
        public virtual ContractorUnionActivityType Owner { get; set; }
    }
}
