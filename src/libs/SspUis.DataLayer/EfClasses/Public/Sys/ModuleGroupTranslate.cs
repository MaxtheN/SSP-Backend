using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_module_group_translate")]
    public partial class ModuleGroupTranslate : EnumTranslateEntity<ModuleGroupTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ModuleGroup.Translates))]
        public virtual ModuleGroup Owner { get; set; }
    }
}
