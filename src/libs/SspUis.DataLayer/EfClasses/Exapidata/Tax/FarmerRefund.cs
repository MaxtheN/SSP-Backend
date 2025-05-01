using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Exapidata.Tax
{
    [Table("farmer_refund", Schema = "exapidata")]
    public class FarmerRefund
    {
        [Key]
        [Required]
        [Column("id")]
        public long Id { get; set; }
        [Column("tin")]
        public string? Tin { get; set; }
        [Column("year")]
        public int? Year { get; set; }
        [Column("summa")]
        public decimal? Summa { get; set; }
        [Column("application_count")]
        public int? ApplicationCount { get; set; }
    }
}
