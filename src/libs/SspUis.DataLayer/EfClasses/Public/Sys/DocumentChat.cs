using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;
using WEBASE;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_document_chat", Schema = "public")]
    public class DocumentChat : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("message_text")]
        public string MessageText { get; set; }

        [Required]
        [Column("table_id")]
        public int TableId { get; set; }

        [Required]
        [Column("document_id")]
        public long DocumentId { get; set; }

        [Required]
        [Column("application_model_code_id")]
        public int AppId { get; set; }

        [Column("organization_id")]
        public int? OrganizationId { get; set; }

        [Column("state_id")]
        public int StateId { get; set; }

        [Column("contractor_id")]
        public long? ContractorId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }

        [ForeignKey(nameof(AppId))]
        public virtual ApplicationModelCode App { get; set; }

        public void Delete(ref DocumentChat entity)
        {
            entity.StateId = StateIdConst.PASSIVE;
        }
    }
}
