using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_partisanship_translate", Schema = "hrm")]
    public partial class PartisanshipTranslate : TranslateEntity<PartisanshipTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Partisanship.Translates))]
        public virtual Partisanship Owner { get; set; }
    }
}
