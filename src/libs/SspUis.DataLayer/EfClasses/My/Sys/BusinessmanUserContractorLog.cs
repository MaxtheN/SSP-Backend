using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_businessman_user_contractor_log", Schema = "my")]
    public class BusinessmanUserContractorLog
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("businessman_user_id")]
        public int BusinessmanUserId { get; set; }

        [Required]
        [Column("unique_key")]
        [StringLength(100)]
        public string UniqueKey { get; set; }

        [Column("date_at", TypeName = "timestamp without time zone")]
        public DateTime DateAt { get; set; }

        [Required]
        [Column("date_of_expire", TypeName = "timestamp without time zone")]
        public DateTime DateOfExpire { get; set; }

        [Column("contractor_id")]
        public long? ContractorId { get; set; }


        [ForeignKey(nameof(BusinessmanUserId))]
        public virtual BusinessmanUser BusinessmanUser { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
    }
}
