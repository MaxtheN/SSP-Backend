using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata.Tax
{
    [Table("contractor_aylanma", Schema ="exapidata")]
    public class ContractorAylanma
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("inn")]
        public string? Inn { get; set; }

        [Column("pinfl")]
        public string? Pinfl { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }
    }
}
