using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_role_module")]
    public partial class RoleModule : IHaveSingleUniqueForeignKey<int>, IHaveIdProp<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("module_id")]
        public int ModuleId { get; set; }
        [Column("created_user_id")]
        public int CreatedUserId { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(CreatedUserId))]
        [InverseProperty(nameof(User.RoleModules))]
        public virtual User CreatedUser { get; set; }
        [ForeignKey(nameof(ModuleId))]
        [InverseProperty(nameof(EfClasses.Module.RoleModules))]
        public virtual Module Module { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(EfClasses.Role.RoleModules))]
        public virtual Role Role { get; set; }

        public object GetUniqueForeignKey() => ModuleId;

        public void SetUniqueForeignKey(int foreignKey) => ModuleId = foreignKey;
    }
}
