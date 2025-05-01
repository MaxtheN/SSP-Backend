using SspUis.DataLayer.EfClasses.Exapidata.Boj;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata.Tax
{
    [Table("imtiyoz_data", Schema ="exapidata")]
    public class ImtiyozData
    {
        public ImtiyozData()
        {
            Tables = new HashSet<ImtiyozDataTable>();
        }
        [Key]
        [Required]
        [Column("id")]
        public long Id { get; set; }
        [Column("tin")]
        public string? Tin { get; set; }
        [Column("year")]
        public int? Year { get; set; }

        [InverseProperty(nameof(ImtiyozDataTable.Owner))]
        public ICollection<ImtiyozDataTable> Tables {get; set; }
    }
}
