using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_user_role")]
    public partial class UserRole : IHaveSingleUniqueForeignKey<int>, IHaveIdProp<int>, IHaveStateId
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(EfClasses.Role.UserRoles))]
        public virtual Role Role { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(EfClasses.User.UserRoles))]
        public virtual User User { get; set; }

        public object GetUniqueForeignKey() => RoleId;
        public void SetUniqueForeignKey(int foreignKey) => RoleId = foreignKey;
    }
}
