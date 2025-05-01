using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_source_code_translate", Schema = "hrm")]
    public partial class SourceCodeTranslate : TranslateEntity<SourceCodeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(SourceCode.Translates))]
        public virtual SourceCode Owner { get; set; }
    }
}
