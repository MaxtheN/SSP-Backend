using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Exapidata.BankCredit
{
    [Table("bank_credit_application_table", Schema = "exapidata")]
    public class BankCreditApplicationTable
    {
        [Column("id")]
        [Key]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("doc_num")]
        public int DocNum { get; set; }
        [Column("doc_date")]
        public string DocDate { get; set; }
        [Column("credit_sum")]
        public decimal? CreditSum { get; set; }
        [Column("doc_status")]
        public string DocStatus { get; set; }
        [Column("issuance_sum")]
        public decimal? IssuanceSum { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(BankCreditApplication.Tables))]
        public virtual BankCreditApplication Owner { get; set; }

    }
}