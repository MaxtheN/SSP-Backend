using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Exapidata.BankCredit
{
    [Table("bank_credit_application", Schema ="exapidata")]
    public class BankCreditApplication
    {
        public BankCreditApplication() 
        {
            Tables = new HashSet<BankCreditApplicationTable>();
        }

        [Key]
        [Required]
        [Column("id")]
        public long Id { get; set; }
        [Column("tin")]
        public string Tin { get; set; }
        [Column("bank_name")]
        public string BankName { get; set; }
        [Column("bank_mfo")]
        public string BankMfo { get; set; }

        [InverseProperty(nameof(BankCreditApplicationTable.Owner))]
        public virtual ICollection<BankCreditApplicationTable> Tables { get; set; }
    }
}
