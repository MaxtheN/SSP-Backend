using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_prtn_reject_reason_translate", Schema = "partner")]
    public partial class PrtnRejectReasonTranslate : TranslateEntity<PrtnRejectReasonTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(PrtnRejectReason.Translates))]
        public virtual PrtnRejectReason Owner { get; set; }
    }
}
