using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Integration.Bojxona
{
    [Table("boj_bojxona_imtiyoz", Schema = "exapidata")]
    public class BojxonaImtiyoz
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("inn")]
        public string Inn { get; set; }
        [Column("app_count")]
        public int AppCount { get; set; }
        [Column("rej_count")]
        public int RejCount { get; set; }
        [Column("dev_count")]
        public int DevCount { get; set; }
        [Column("sum")]
        public decimal Sum { get; set; }
        [Column("gr_chan_count")]
        public int GrChanCount { get; set; }
    }
}
