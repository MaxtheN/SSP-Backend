using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Claim
{
    [Table("doc_mediation_files", Schema = "claim")]
    [Index(nameof(OwnerId), Name = "ix_doc_mediation_files__owner")]
    public partial class MediationFile : FileEntity<long>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Mediation.Files))]
        public virtual Mediation Owner { get; set; }
    }
}
