using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_notification")]
    public partial class Notification : IHaveIdProp<long>
    {
        public Notification()
        {
            NotificationUsers = new HashSet<NotificationUser>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("title")]
        [StringLength(500)]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("type_id")]
        public int TypeId { get; set; }
        [Column("doc_status_id")]
        public int? DocStatusId { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("doc_id")]
        public long? DocId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        
        [ForeignKey(nameof(DocStatusId))]
        public virtual Status DocStatus { get; set; }
        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }
        [ForeignKey(nameof(TypeId))]
        [InverseProperty(nameof(NotificationType.Notifications))]
        public virtual NotificationType Type { get; set; }
        [InverseProperty(nameof(NotificationUser.Owner))]
        public virtual ICollection<NotificationUser> NotificationUsers { get; set; }
    }
}
