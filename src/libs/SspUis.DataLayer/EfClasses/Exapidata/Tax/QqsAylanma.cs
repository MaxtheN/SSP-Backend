using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SspUis.DataLayer.EfClasses.Exapidata.Tax;
[Table("tax_qqs_aylanma", Schema = "exapidata")]
public class QqsAylanma
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("inn")]
    public string Inn { get; set; }
    [Column("year")]
    public int Year { get; set; }
    [Column("month")]
    public int Month { get; set; }
    [Column("net_income_without_vat")]
    public decimal NetIncomeWithoutVat { get; set; }
    [Column("vat_sum")]
    public decimal VatSum { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
}
