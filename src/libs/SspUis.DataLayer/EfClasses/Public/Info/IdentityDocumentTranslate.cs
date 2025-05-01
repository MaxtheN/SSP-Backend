using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_identity_document_translate")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_identity_document_translate__lang", IsUnique = true)]
    public partial class IdentityDocumentTranslate : TranslateEntity<IdentityDocumentTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(IdentityDocument.Translates))]
        public virtual IdentityDocument Owner { get; set; }
    }
}
