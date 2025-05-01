using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_module_group")]
    public partial class ModuleGroup : IHaveIdProp<int>
    {
        public ModuleGroup()
        {
            Translates = new HashSet<ModuleGroupTranslate>();
            ModuleSubGroups = new HashSet<ModuleSubGroup>();
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
        [StringLength(300)]
        public string FullName { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime? CreatedAt { get; set; }

        [InverseProperty(nameof(ModuleGroupTranslate.Owner))]
        public virtual ICollection<ModuleGroupTranslate> Translates { get; set; }
        [InverseProperty(nameof(ModuleSubGroup.Group))]
        public virtual ICollection<ModuleSubGroup> ModuleSubGroups { get; set; }
    }
}
