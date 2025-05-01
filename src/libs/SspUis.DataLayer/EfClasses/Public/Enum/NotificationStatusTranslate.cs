using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_notification_status_translate")]
    public partial class NotificationStatusTranslate : EnumTranslateEntity<NotificationStatusTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(NotificationStatus.Translates))]
        public virtual NotificationStatus Owner { get; set; }
    }
}
