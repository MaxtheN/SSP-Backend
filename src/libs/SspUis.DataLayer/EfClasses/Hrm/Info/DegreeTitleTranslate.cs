using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_degree_title_translate", Schema = "hrm")]
    public partial class DegreeTitleTranslate : TranslateEntity<DegreeTitleTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(DegreeTitle.Translates))]
        public virtual DegreeTitle Owner { get; set; }
    }
}
