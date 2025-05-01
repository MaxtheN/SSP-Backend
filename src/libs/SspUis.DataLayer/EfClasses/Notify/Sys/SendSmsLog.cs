using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_send_sms_log", Schema = "notify")]
    public class SendSmsLog : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("phone_numer")]
        [StringLength(50)]
        public string PhoneNumer { get; set; }

        [Required]
        [Column("sms_text")]
        [StringLength(500)]
        public string SmsText { get; set; }

        [Column("error_text")]
        public string? ErrorText { get; set; }

        [Column("table_id")]
        public int TableId { get; set; }

        [Column("doc_id")]
        public long DocId { get; set; }

        [Column("from_status_id")]
        public int? FromStatusId { get; set; }

        [Column("to_status_id")]
        public int? ToStatusId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(FromStatusId))]
        public virtual Status Status { get; set; }

        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }

        [ForeignKey(nameof(ToStatusId))]
        public virtual Status ToStatus { get; set; }
    }
}
