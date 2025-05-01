using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_candidates_confirmation_table_file", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_doc_candidates_confirmation_table_file__owner")]
    public class CandidatesConfirmationTableFile : FileEntity<long>, IHaveIdProp<Guid>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(CandidatesConfirmationTable.Files))]
        public virtual CandidatesConfirmationTable Owner { get; set; }
    }
}