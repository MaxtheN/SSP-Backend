using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Corruption
{
    [Table("doc_join_anti_corruption_result_table", Schema = "corruption")]
    public partial class JoinAntiCorruptionResultTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long? OwnerId { get; set; }
        [Column("application_id")]
        public long? ApplicationId { get; set; }
        [Column("join_anti_corruption_result_type_id")]
        public int JoinAntiCorruptionResultTypeId { get; set; }
        [Column("corruption_certificate_number")]
        [StringLength(50)]
        public string CorruptionCertificateNumber { get; set; }
        [Column("corruption_certificate_on")]
        public DateOnly? CorruptionCertificateOn { get; set; }
        [Column("corruption_certificate_expire_on")]
        public DateOnly? CorruptionCertificateExpireOn { get; set; }
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
        [ForeignKey(nameof(JoinAntiCorruptionResultTypeId))]
        public virtual JoinAntiCorruptionResultType JoinAntiCorruptionResultType { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(JoinAntiCorruptionResult.Tables))]
        public virtual JoinAntiCorruptionResult Owner { get; set; }

        [InverseProperty(nameof(EfClasses.Corruption.JoinAntiCorruptionCertificate.JoinAntiCorruptionResultTable))]
        public virtual EfClasses.Corruption.JoinAntiCorruptionCertificate JoinAntiCorruptionCertificate { get; set; }
    }
}
