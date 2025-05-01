using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("hl_employee_military_rank", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_hl_employee_military_rank__employee")]
    public partial class EmployeeMilitaryRank : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("military_rank_id")]
        public int MilitaryRankId { get; set; }
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
        [ForeignKey(nameof(MilitaryRankId))]
        public virtual MilitaryRank MilitaryRanks { get; set; }
    }
}
