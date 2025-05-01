using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_military_rank_translate", Schema = "hrm")]
    public partial class MilitaryRankTranslate : TranslateEntity<MilitaryRankTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MilitaryRank.Translates))]
        public virtual MilitaryRank Owner { get; set; }
    }
}
