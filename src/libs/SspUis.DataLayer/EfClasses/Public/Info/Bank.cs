using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_bank")]
    [Index(nameof(Code), Name = "info_bank_unique_index_bank_code", IsUnique = true)]
    public partial class Bank : IHaveStateId, IHaveIdProp<int>
    {
        public Bank()
        {
            Translates = new HashSet<BankTranslate>();
            Contractors = new HashSet<Contractor>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string? OrderCode { get; set; }
        [Required]
        [Column("bank_code")]
        [StringLength(5)]
        public string Code { get; set; }
        [Required]
        [Column("bank_name")]
        [StringLength(300)]
        public string BankName { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("bank_code_id")]
        public int? BankCodeId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [ForeignKey(nameof(BankCodeId))]
        public virtual BankCode BankCode { get; set; }

        [InverseProperty(nameof(BankTranslate.Owner))]
        public virtual ICollection<BankTranslate> Translates { get; set; }

        [InverseProperty(nameof(Contractor.Bank))]
        public virtual ICollection<Contractor> Contractors { get; set; }

        [InverseProperty(nameof(BusinessActivityType.Bank))]
        public virtual ICollection<BusinessActivityType> BusinessActivityTypes { get; set; }
    }
}
