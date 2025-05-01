using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_module")]
    [Index(nameof(Code), Name = "sys_module_unique_index_code", IsUnique = true)]
    public partial class Module : IModule<ModuleTranslate, ModuleSubGroup, ModuleSubGroupTranslate>, IHaveStateId, IHaveIdProp<int>
    {
        public Module()
        {
            Translates = new HashSet<ModuleTranslate>();
            RoleModules = new HashSet<RoleModule>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("code")]
        [StringLength(100)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(300)]
        public string FullName { get; set; }
        [Column("sub_group_id")]
        public int SubGroupId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(SubGroupId))]
        [InverseProperty(nameof(ModuleSubGroup.Modules))]
        public virtual ModuleSubGroup SubGroup { get; set; }
        [InverseProperty(nameof(ModuleTranslate.Owner))]
        public virtual ICollection<ModuleTranslate> Translates { get; set; }
        [InverseProperty(nameof(RoleModule.Module))]
        public virtual ICollection<RoleModule> RoleModules { get; set; }
        [InverseProperty(nameof(AccessModule.SysModule))]
        public virtual ICollection<AccessModule> AccessModules { get; set; }
    }
}
