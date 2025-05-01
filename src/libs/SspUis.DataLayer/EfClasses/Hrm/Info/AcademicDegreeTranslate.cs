using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_academic_degree_translate", Schema = "hrm")]
    public partial class AcademicDegreeTranslate : TranslateEntity<AcademicDegreeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(AcademicDegree.Translates))]
        public virtual AcademicDegree Owner { get; set; }
    }
}
