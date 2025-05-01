using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_access_service_history")]
    public partial class AccessServiceHistory
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Required]
        [Column("user_info")]
        public string UserInfo { get; set; }
        [Column("data")]
        public string Data { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int CreatedUserId { get; set; }
        [Column("modified_date", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedDate { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Accessibility.SysAccessServiceHistories))]
        public virtual Accessibility Owner { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
