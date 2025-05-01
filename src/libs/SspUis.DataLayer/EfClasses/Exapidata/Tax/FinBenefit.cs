using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata.Tax
{
    [Table("tax_fin_benefit", Schema ="exapidata")]
    public class FinBenefit
    {
        [Key]
        [Required]
        [Column("id")]
        public long Id { get; set; }

        [Column("tin")]
        [Required]
        public string Tin { get; set; }

        [Column("year")]
        [Required]
        public int Year { get; set; }

        [Column("period")]
        [Required]
        public int Period { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("net_income")]
        [Required]
        public decimal NetIncome { get; set; }

      
    }
}
