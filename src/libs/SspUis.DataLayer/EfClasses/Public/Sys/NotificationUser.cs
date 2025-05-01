using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using WEBASE.Models;
using SspUis.Core;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_notification_user")]
    public partial class NotificationUser : IHaveIdProp<long>, IHaveSingleUniqueForeignKey<int>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("notification_status_id")]
        public int NotificationStatusId { get; set; } = NotificationStatusIdConst.UNREAD;

        [ForeignKey(nameof(NotificationStatusId))]
        [InverseProperty(nameof(NotificationStatus.NotificationUsers))]
        public virtual NotificationStatus Status { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Notification.NotificationUsers))]
        public virtual Notification Owner { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public object GetUniqueForeignKey() => UserId;

        public void SetUniqueForeignKey(int foreignKey) => UserId = foreignKey;
    }
}
