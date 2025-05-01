using SspUis.DataLayer.EfClasses.Hrm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_candidates_confirmation_table", Schema = "hrm")]
    public class CandidatesConfirmationTable : IHaveIdProp<long>, IHaveStatusId
    {
        public CandidatesConfirmationTable()
        {
            Files = new HashSet<CandidatesConfirmationTableFile>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }
        
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        [InverseProperty(nameof(CandidatesConfirmation.Tables))]
        public virtual CandidatesConfirmation Owner { get; set; }

        [InverseProperty(nameof(CandidatesConfirmationTableFile.Owner))]
        public virtual ICollection<CandidatesConfirmationTableFile> Files { get; set; }
    }
}