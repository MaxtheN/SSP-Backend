

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SspUis.DataLayer.EfClasses;
[Table("soliq_imtiyoz", Schema = "exapidata")]
public class SoliqImtiyoz
{
    [Key]
    [Column("id")]
    public long Id {  get; set; }
    [Column("tin")]
    public long Tin {  get; set; }
    [Column("imtiyoz_count")]
    public int ImtiyozCount {  get; set; }
    [Column("summa")]
    public decimal Summa {  get; set; }
    [Column("year")]
    public int Year { get; set; }
    [Column("region_id")]
    public int RegionId { get; set; }

    [Column("district_id")]
    public int DistrictId { get; set; }

}
