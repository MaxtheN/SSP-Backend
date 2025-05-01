using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_join_anti_corruption_application_files", Schema = "corruption")]
    public partial class JoinAntiCorruptionApplicationFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(JoinAntiCorruptionApplication.Files))]
        public virtual JoinAntiCorruptionApplication Owner { get; set; }
    }
}
