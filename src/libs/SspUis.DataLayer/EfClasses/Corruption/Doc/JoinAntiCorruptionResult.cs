using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Corruption
{
    [Table("doc_join_anti_corruption_result", Schema = "corruption")]
    public partial class JoinAntiCorruptionResult : IHaveIdProp<long>, IHaveStatusId
    {
        public JoinAntiCorruptionResult()
        {
            Files = new HashSet<JoinAntiCorruptionResultFile>();
            Tables = new HashSet<JoinAntiCorruptionResultTable>();
            Signs = new HashSet<JoinAntiCorruptionResultSign>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("id2")]
        public Guid Id2 { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Required]
        [Column("details")]
        public string Details { get; set; }
        [Column("chairmen_id")]
        public int? ChairmenId { get; set; }
        [Column("chairmen_organization_id")]
        public int ChairmenOrganizationId { get; set; }
        [Column("chairmen_position_id")]
        public int ChairmenPositionId { get; set; }
        [Required]
        [Column("chairmen_fio")]
        [StringLength(250)]
        public string ChairmenFio { get; set; }
        [Column("member1_id")]
        public int? Member1Id { get; set; }
        [Column("member1_organization_id")]
        public int Member1OrganizationId { get; set; }
        [Column("member1_position_id")]
        public int Member1PositionId { get; set; }
        [Required]
        [Column("member1_fio")]
        [StringLength(250)]
        public string Member1Fio { get; set; }
        [Column("member2_id")]
        public int? Member2Id { get; set; }
        [Column("member2_organization_id")]
        public int? Member2OrganizationId { get; set; }
        [Column("member2_position_id")]
        public int? Member2PositionId { get; set; }
        [Column("member2_fio")]
        [StringLength(250)]
        public string Member2Fio { get; set; }
        [Column("member3_id")]
        public int? Member3Id { get; set; }
        [Column("member3_organization_id")]
        public int? Member3OrganizationId { get; set; }
        [Column("member3_position_id")]
        public int? Member3PositionId { get; set; }
        [Column("member3_fio")]
        [StringLength(250)]
        public string Member3Fio { get; set; }
        [Column("member4_id")]
        public int? Member4Id { get; set; }
        [Column("member4_organization_id")]
        public int? Member4OrganizationId { get; set; }
        [Column("member4_position_id")]
        public int? Member4PositionId { get; set; }
        [Column("member4_fio")]
        [StringLength(250)]
        public string Member4Fio { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("is_chairmen_sign")]
        public bool? IsChairmenSign { get; set; }
        [Column("is_member1_sign")]
        public bool? IsMember1Sign { get; set; }
        [Column("is_member2_sign")]
        public bool? IsMember2Sign { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(JoinAntiCorruptionResultFile.Owner))]
        public virtual ICollection<JoinAntiCorruptionResultFile> Files { get; set; }
        [InverseProperty(nameof(JoinAntiCorruptionResultTable.Owner))]
        public virtual ICollection<JoinAntiCorruptionResultTable> Tables { get; set; }
        [InverseProperty(nameof(JoinAntiCorruptionResultSign.Owner))]
        public virtual ICollection<JoinAntiCorruptionResultSign> Signs { get; set; }
    }
}
