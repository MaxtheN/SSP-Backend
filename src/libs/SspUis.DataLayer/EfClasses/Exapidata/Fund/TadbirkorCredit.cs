using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Exapidata.Fund
{
    [Table("fund_tadbirkor_credit", Schema = "exapidata")]

    public class FundTadbirkorCredit
    {
        [Column("id")]
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        [Column("owner_id")]
        public long OwnerId { get; set; }   

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("currency_id")]
        public int CurrencyId { get; set; }

        [Column("currency")]
        public string Currency { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(FundTadbirkor.Credits))]
        public virtual FundTadbirkor Owner { get; set; }
    }
}
