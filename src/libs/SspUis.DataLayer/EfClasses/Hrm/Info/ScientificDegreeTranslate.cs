using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_scientific_degree_translate", Schema = "hrm")]
    public partial class ScientificDegreeTranslate : TranslateEntity<ScientificDegreeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ScientificDegree.Translates))]
        public virtual ScientificDegree Owner { get; set; }
    }
}
