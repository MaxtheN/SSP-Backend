using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_relative_degree_translate")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_relative_degree_translate__lang", IsUnique = true)]
    public partial class RelativeDegreeTranslate : TranslateEntity<RelativeDegreeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(RelativeDegree.Translates))]
        public virtual RelativeDegree Owner { get; set; }
    }
}
