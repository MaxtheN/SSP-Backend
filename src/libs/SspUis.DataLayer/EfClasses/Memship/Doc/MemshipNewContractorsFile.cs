using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_new_contractors_file", Schema = "memship")]
    [Index(nameof(OwnerId), Name = "ix_doc_memship_new_contractors_files__owner")]
    public partial class MemshipNewContractorsFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MemshipNewContractor.Files))]
        public virtual MemshipNewContractor Owner { get; set; }
    }
}
