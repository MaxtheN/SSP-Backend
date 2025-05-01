using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_bank_code_translate")]
    public partial class BankCodeTranslate : EnumTranslateEntity<BankCodeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(BankCode.Translates))]
        public virtual BankCode Owner { get; set; }
    }
}
