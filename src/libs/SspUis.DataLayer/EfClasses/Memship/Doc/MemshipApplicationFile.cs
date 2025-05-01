using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_application_files", Schema = "memship")]
    [Index(nameof(OwnerId), Name = "ix_doc_memship_application_files__owner")]
    public partial class MemshipApplicationFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        public virtual MemshipApplication Owner { get; set; }
    }
}
