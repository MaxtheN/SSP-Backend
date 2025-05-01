using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_organization_settlement_account")]
    [Index(nameof(BankId), nameof(AccountCode), Name = "uc_bank_code", IsUnique = true)]
    public class OrganizationSettlementAccount : IHaveIdProp<long>, IHaveStateId
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Required]
        [Column("account_name")]
        [StringLength(250)]
        public string AccountName { get; set; }
        [Required]
        [Column("account_code")]
        [StringLength(20)]
        public string AccountCode { get; set; }
        [Column("bank_id")]
        public int BankId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }

        [ForeignKey(nameof(BankId))]
        public virtual Bank Bank { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
    }
}
