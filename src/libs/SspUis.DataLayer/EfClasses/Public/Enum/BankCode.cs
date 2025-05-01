using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_bank_code")]
    public class BankCode
    {
        public BankCode()
        {
            Translates = new HashSet<BankCodeTranslate>();
            //Banks = new HashSet<Bank>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("is_json")]
        public bool IsJson { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [InverseProperty(nameof(BankCodeTranslate.Owner))]
        public virtual ICollection<BankCodeTranslate> Translates { get; set; }
        //public virtual ICollection<Bank> Banks { get; set; }
    }
}
