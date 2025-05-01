using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_join_anti_corruption_application", Schema = "corruption")]
    public partial class JoinAntiCorruptionApplication : IBaseApplicationEntity, IHaveIdProp<long>
    {
        public JoinAntiCorruptionApplication()
        {
            Employees = new HashSet<JoinAntiCorruptionApplicationEmployee>();
            Files = new HashSet<JoinAntiCorruptionApplicationFile>();
            Participates = new HashSet<JoinAntiCorruptionApplicationParticipate>();
            Tables = new HashSet<JoinAntiCorruptionApplicationTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("corruption_review_type_id")]
        public int CorruptionReviewTypeId { get; set; }
        [Column("address")]
        [StringLength(600)]
        public string Address { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("contractor_activity_type_id")]
        public int ContractorActivityTypeId { get; set; }
        [Column("contractor_union_activity_type_id")]
        public int? ContractorUnionActivityTypeId { get; set; }
        [Column("union_member_count")]
        public int UnionMemberCount { get; set; }
        [Column("avg_employees_count")]
        public int AvgEmployeesCount { get; set; }
        [Column("prev_yearly_earnings")]
        [Precision(18, 2)]
        public decimal PrevYearlyEarnings { get; set; }
        [Column("currency_id")]
        public int CurrencyId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(ContractorActivityTypeId))]
        public virtual ContractorActivityType ContractorActivityType { get; set; }
        [ForeignKey(nameof(ContractorUnionActivityTypeId))]
        public virtual ContractorUnionActivityType ContractorUnionActivityType { get; set; }
        [ForeignKey(nameof(CorruptionReviewTypeId))]
        public virtual CorruptionReviewType CorruptionReviewType { get; set; }
        [ForeignKey(nameof(CreatedUserId))]
        public virtual BusinessmanUser CreatedUser { get; set; }
        [InverseProperty(nameof(JoinAntiCorruptionApplicationEmployee.Owner))]
        public virtual ICollection<JoinAntiCorruptionApplicationEmployee> Employees { get; set; }
        [InverseProperty(nameof(JoinAntiCorruptionApplicationFile.Owner))]
        public virtual ICollection<JoinAntiCorruptionApplicationFile> Files { get; set; }
        [InverseProperty(nameof(JoinAntiCorruptionApplicationParticipate.Owner))]
        public virtual ICollection<JoinAntiCorruptionApplicationParticipate> Participates { get; set; }
        [InverseProperty(nameof(JoinAntiCorruptionApplicationTable.Owner))]
        public virtual ICollection<JoinAntiCorruptionApplicationTable> Tables { get; set; }
    }
}
