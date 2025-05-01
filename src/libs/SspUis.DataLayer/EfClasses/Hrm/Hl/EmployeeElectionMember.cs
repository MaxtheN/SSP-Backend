using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("hl_employee_election_member", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_hl_employee_election_member__employee")]
    public partial class EmployeeElectionMember : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("election_member_id")]
        public int ElectionMemberId { get; set; }
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

        [ForeignKey(nameof(ElectionMemberId))]
        public virtual ElectionMember ElectionMember { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual Employee Owner { get; set; }
    }
}
