using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_restriction_of_sending_applications", Schema = "public")]
    public class RestrictionOfSendingApplication : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("start_at")]
        public DateTime StartAt { get; set; }

        [Required]
        [Column("end_at")]
        public DateTime EndAt { get; set; }

        [Column("details")]
        [StringLength(5000)]
        public string Details { get; set; }

        [Column("message_text")]
        public string MessageText { get; set; }

        [Column("table_id")]
        public int TableId { get; set; }

        [Column("app_id")]
        public int AppId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(AppId))]
        public virtual ApplicationModelCode ApplicationModelCode { get; set; }

        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
    }
}
