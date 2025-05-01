using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_notification_status")]
    public partial class NotificationStatus
    {
        public NotificationStatus()
        {
            Translates = new HashSet<NotificationStatusTranslate>();
            NotificationUsers = new HashSet<NotificationUser>();
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
        [StringLength(250)]
        public string FullName { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [InverseProperty(nameof(NotificationStatusTranslate.Owner))]
        public virtual ICollection<NotificationStatusTranslate> Translates { get; set; }
        [InverseProperty(nameof(NotificationUser.Status))]
        public virtual ICollection<NotificationUser> NotificationUsers { get; set; }
    }
}
