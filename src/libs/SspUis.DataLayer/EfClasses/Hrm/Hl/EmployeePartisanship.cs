using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("hl_employee_partisanship", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_hl_employee_partisanship__employee")]
    public partial class EmployeePartisanship : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("partisanship_id")]
        public int PartisanshipId { get; set; }
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

        [ForeignKey(nameof(OwnerId))]
        public virtual Employee Owner { get; set; }
        [ForeignKey(nameof(PartisanshipId))]
        public virtual Partisanship Partisanship { get; set; }
    }
}
