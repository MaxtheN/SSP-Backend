using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SspUis.DataLayer.EfClasses.Exapidata.Tax;
[Table("tax_aos_aylanma", Schema = "exapidata")]
public class AosAylanma
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("inn")]
    public string Inn { get; set; }
    [Column("year")]
    public int Year { get; set; }
    
    [Column("net_income")]
    public decimal NetIncome { get; set; }
    
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}
