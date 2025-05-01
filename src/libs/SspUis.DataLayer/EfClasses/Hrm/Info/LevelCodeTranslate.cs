using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_level_code_translate", Schema = "hrm")]
    public partial class LevelCodeTranslate : TranslateEntity<LevelCodeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(LevelCode.Translates))]
        public virtual LevelCode Owner { get; set; }
    }
}
