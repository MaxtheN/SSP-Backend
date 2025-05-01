using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Claim;

[Table("doc_application_for_court_files", Schema = "claim")]
[Index(nameof(OwnerId), Name = "ix_doc_application_for_court_files__owner")]
public partial class ApplicationForCourtFile : FileEntity<long>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ApplicationForCourt.Files))]
    public virtual ApplicationForCourt Owner { get; set; }
}
