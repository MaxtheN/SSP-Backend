using SspUis.DataLayer.EfClasses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_mono_application_bandlik_result", Schema = "my")]
    public class MonoApplicationBandlikResult : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("subsidy_amount")]
        public string SubsidyAmount { get; set; }
        [Column("responsible_fio")]
        public string ResponsibleFio { get; set; }
        [Column("responsible_phone")]
        public string ResponsiblePhone { get; set; }
        [Column("reject_reason")]
        public string RejectReason { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public virtual MonoApplication Application { get; set; }
        [ForeignKey(nameof(Status))]
        public virtual MonoApplicationBandlikStatus BandlikStatus { get; set; }
    }
}
