using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_module_sub_group_translate")]
    public partial class ModuleSubGroupTranslate : TranslateEntity<ModuleSubGroupTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ModuleSubGroup.Translates))]
        public virtual ModuleSubGroup Owner { get; set; }
    }
}
