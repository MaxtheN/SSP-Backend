using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata.Tax
{
    [Table("tax_debt", Schema = "exapidata")]
    public class Debt
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("tin")]
        [Required]
        public string Tin { get; set; }

        [Column("year")]
        [Required]
        public int Year { get; set; }

        [Column("send_id")]
        public string SendId { get; set; }
        
        [Column("send_date")]
        public DateOnly SendDate { get; set; }

        [Column("tax_debt")]
        [Precision(18,2)]
        public decimal? TaxDebt { get; set; }
    }
}
