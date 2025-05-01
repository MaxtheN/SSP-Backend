using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Corruption
{
    [Table("doc_join_anti_corruption_result_files", Schema = "corruption")]
    public partial class JoinAntiCorruptionResultFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(JoinAntiCorruptionResult.Files))]
        public virtual JoinAntiCorruptionResult Owner { get; set; }
    }
}
