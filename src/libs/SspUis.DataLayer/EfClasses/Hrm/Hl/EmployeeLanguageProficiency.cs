using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("hl_employee_language_proficiency", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_hl_employee_languagperoficiency__employee")]
    public partial class EmployeeLanguageProficiency : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("languagperoficiency_id")]
        public int LanguagperoficiencyId { get; set; }
        [Column("language_degree_id")]
        public int? LanguageDegreeId { get; set; }
        [Required]
        [Column("language_degree")]
        [StringLength(50)]
        public string LanguageDegree { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(LanguageDegreeId))]
        public virtual LanguageDegree? LanguageDegrees { get; set; }
        [ForeignKey(nameof(LanguagperoficiencyId))]
        public virtual LanguageProficiency Languagperoficiency { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual Employee Owner { get; set; }
    }
}
