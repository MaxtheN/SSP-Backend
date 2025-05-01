using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_table", Schema = "public")]
    public class Table : IHaveIdProp<int>
    {
        public Table() { }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("short_name")]
        [StringLength(50)]
        public string ShortName { get; set; }
        [Column("full_name")]
        [StringLength(80)]
        public string FullName { get; set; }
        [Column("db_schema_name")]
        [StringLength(50)]
        public string DbSchemaName { get; set; }
        [Column("db_table_name")]
        [StringLength(50)]
        public string DbTableName { get; set; }
    }
}
