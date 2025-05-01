using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_candidates_confirmation", Schema = "hrm")]
    public class CandidatesConfirmation : IHaveIdProp<long>, IHaveStatusId
    {
        public CandidatesConfirmation()
        {
            Tables = new HashSet<CandidatesConfirmationTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }

        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }

        [Required]
        [Column("doc_content")]
        [StringLength(600)]
        public string DocContent { get; set; }

        [Required]
        [Column("status_id")]
        public int StatusId { get; set; }

        [Required]
        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Required]
        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Required]
        [Column("position_id")]
        public int PositionId { get; set; }

        [Column("general_conclusion")]
        [StringLength(600)]
        public string GeneralConclusion { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [ForeignKey(nameof(PositionId))]
        public virtual Position Position { get; set; }

        [InverseProperty(nameof(CandidatesConfirmationTable.Owner))]
        public virtual ICollection<CandidatesConfirmationTable> Tables { get; set; }
    }
}