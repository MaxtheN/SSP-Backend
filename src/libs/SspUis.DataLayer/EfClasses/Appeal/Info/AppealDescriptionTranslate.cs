using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Appeal
{
    [Table("info_appeal_description_translate", Schema = "appeal")]
    public partial class AppealDescriptionTranslate : TranslateEntity<AppealDescriptionTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(AppealDescription.Translates))]
        public virtual AppealDescription Owner { get; set; }
    }
}
