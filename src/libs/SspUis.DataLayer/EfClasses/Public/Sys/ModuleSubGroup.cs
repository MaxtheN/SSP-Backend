using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_module_sub_group")]
    [Index(nameof(Code), Name = "sys_module_sub_group_unique_index_code", IsUnique = true)]
    public partial class ModuleSubGroup : IModuleSubGroup<ModuleSubGroupTranslate>, IHaveIdProp<int>
    {
        public ModuleSubGroup()
        {
            Translates = new HashSet<ModuleSubGroupTranslate>();
            Modules = new HashSet<Module>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
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
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty(nameof(ModuleGroup.ModuleSubGroups))]
        public virtual ModuleGroup Group { get; set; }
        [InverseProperty(nameof(ModuleSubGroupTranslate.Owner))]
        public virtual ICollection<ModuleSubGroupTranslate> Translates { get; set; }
        [InverseProperty(nameof(Module.SubGroup))]
        public virtual ICollection<Module> Modules { get; set; }
    }
}
