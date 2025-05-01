using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_access_module")]
    public partial class AccessModule : IHaveSingleUniqueForeignKey<int>, IHaveIdProp<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("access_id")]
        public int AccessId { get; set; }
        [Column("module_id")]
        public int ModuleId { get; set; }
        [Column("created_user_id")]
        public int CreatedUserId { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(AccessId))]
        [InverseProperty(nameof(Accessibility.AccessModules))]
        public virtual Accessibility Access { get; set; }
        [ForeignKey(nameof(CreatedUserId))]
        public virtual User CreatedUser { get; set; }
        [ForeignKey(nameof(ModuleId))]
        [InverseProperty(nameof(Module.AccessModules))]
        public virtual Module SysModule { get; set; }

        public object GetUniqueForeignKey() => ModuleId;

        public void SetUniqueForeignKey(int foreignKey) => ModuleId = foreignKey;
    }
}
