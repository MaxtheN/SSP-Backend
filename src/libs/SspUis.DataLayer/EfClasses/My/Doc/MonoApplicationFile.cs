using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_mono_application_file", Schema = "my")]
    [Index(nameof(OwnerId), Name = "ix_doc_mono_application__files__owner")]
    public partial class MonoApplicationFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [Column("column_name")]
        [StringLength(128)]
        public string ColumnName { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MonoApplication.Files))]
        public virtual MonoApplication Owner { get; set; }
    }
}
