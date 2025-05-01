using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Exapidata.Fund;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata.Boj
{
    [Table("gtd_by_inn_good", Schema = "exapidata")]
    public class GTDByInnGood
    {
        [Column("id")]
        [Key]
        public long Id { get; set; }

        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Column("net_mass_goods")]
        public decimal NetMassGoods { get; set; }

        [Column("number_goods")]
        public string NumberGoods { get; set; }

        [Column("code_tiftn_goods")]
        public string CodeTiftnGoods { get; set; }

        [Column("additional_unit_goods")]
        public decimal AdditionalUnitgoods { get; set; }

        [Column("num_contract")]
        public string NumContract { get; set; }

        [Column("unit_goods")]
        public string UnitGoods { get; set; }

        [Column("value_goods")]
        public decimal ValueGoods { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(GTDByInn.Goods))]
        public virtual GTDByInn Owner { get; set; }
    }
}
