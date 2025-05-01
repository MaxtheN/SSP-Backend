using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("hl_employee_degree_title", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_hl_employee_degree_title__employee")]
    public partial class EmployeeDegreeTitle : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("degree_title_id")]
        public int DegreeTitleId { get; set; }
        [Column("year")]
        public DateOnly Year { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(DegreeTitleId))]
        public virtual DegreeTitle DegreeTitle { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual Employee Owner { get; set; }
    }
}
