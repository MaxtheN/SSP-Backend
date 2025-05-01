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
    [Table("imtiyoz_data_table", Schema ="exapidata")]
    public class ImtiyozDataTable
    {
        [Key]
        [Required]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column ("owner_id")]
        public long OwnerId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("lgota_id")]
        public int? LgotaId { get; set; }

        [Column("summa")]
        public double? Summa { get; set; }

        [Column("cnt")]
        public int? Cnt { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ImtiyozData.Tables))]
        public virtual ImtiyozData Owner { get; set; }

    }
}
