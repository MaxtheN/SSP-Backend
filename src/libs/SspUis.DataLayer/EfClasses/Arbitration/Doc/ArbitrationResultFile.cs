using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_arbitration_result_file", Schema = "arbitration")]
[Index(nameof(OwnerId), Name = "ix_doc_arbitration_result_files__owner")]
public partial class ArbitrationResultFile : FileEntity<long>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ArbitrationResult.Files))]
    public virtual ArbitrationResult Owner { get; set; }
}
