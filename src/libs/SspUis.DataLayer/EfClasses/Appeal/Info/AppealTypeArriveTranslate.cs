using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Appeal
{
    [Table("info_appeal_type_arrive_translate", Schema = "appeal")]
    public partial class AppealTypeArriveTranslate : TranslateEntity<AppealTypeArriveTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(AppealTypeArrive.Translates))]
        public virtual AppealTypeArrive Owner { get; set; }
    }
}
