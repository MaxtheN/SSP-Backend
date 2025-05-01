using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_role")]
    public partial class Role : IHaveStateId, IHaveIdProp<int>
    {
        public Role()
        {
            RoleModules = new HashSet<RoleModule>();
            Translates = new HashSet<RoleTranslate>();
            UserRoles = new HashSet<UserRole>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(20)]
        public string OrderCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(300)]
        public string FullName { get; set; }
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
        [Column("is_default")]
        public bool IsDefault { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(RoleModule.Role))]
        public virtual ICollection<RoleModule> RoleModules { get; set; }
        [InverseProperty(nameof(RoleTranslate.Owner))]
        public virtual ICollection<RoleTranslate> Translates { get; set; }
        [InverseProperty(nameof(UserRole.Role))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
