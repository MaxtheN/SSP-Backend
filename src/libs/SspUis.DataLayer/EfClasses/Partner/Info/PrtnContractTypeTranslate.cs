using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_prtn_contract_type_translate", Schema = "partner")]
    public partial class PrtnContractTypeTranslate : TranslateEntity<PrtnContractTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(PrtnContractType.Translates))]
        public virtual PrtnContractType Owner { get; set; }
    }
}
