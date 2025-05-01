using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_applicant_type_translate")]
    public partial class ApplicantTypeTranslate : EnumTranslateEntity<ApplicantTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ApplicantType.Translates))]
        public virtual ApplicantType Owner { get; set; }
    }
}
