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
    [Table("gtd_by_inn", Schema ="exapidata")]
    public class GTDByInn
    {
        public GTDByInn() 
        {
            Goods = new HashSet<GTDByInnGood>();
        }
        [Column("id")]
        [Key]
        public long Id { get; set; }

        [Column("tin")]
        public string Tin { get; set; }

        [Column("year")]
        public int Year { get; set; }

        [Column("external_id")]
        public string ExternalId { get; set; }

        [Column("mode")]
        public string Mode { get; set; }

        [Column("organization1_adress")]
        public string Organization1Adress { get; set; }
        
        [Column("organization1_name")]
        public string Organization1Name { get; set; }
        
        [Column("organization2_adress")]
        public string Organization2Adress { get; set; }

        [Column("organization2_name")]
        public string Organization2Name { get; set; }

        [Column("ekim_country_code")]
        public string EkimCountryCode { get; set; }

        [Column("type_incoterms")]
        public string TypeIncoterms { get; set; }

        [Column("type_transport")]
        public string TypeTransport { get; set; }

        [InverseProperty(nameof(GTDByInnGood.Owner))]
        public virtual ICollection<GTDByInnGood> Goods { get; set; }
        
    }
}
