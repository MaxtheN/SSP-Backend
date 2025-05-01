using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_notification_type")]
    public partial class NotificationType : IHaveIdProp<int>
    {
        public NotificationType()
        {
            Translates = new HashSet<NotificationTypeTranslate>();
            Notifications = new HashSet<Notification>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(300)]
        public string FullName { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime? CreatedAt { get; set; }

        [InverseProperty(nameof(NotificationTypeTranslate.Owner))]
        public virtual ICollection<NotificationTypeTranslate> Translates { get; set; }

        [InverseProperty(nameof(Notification.Type))]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
