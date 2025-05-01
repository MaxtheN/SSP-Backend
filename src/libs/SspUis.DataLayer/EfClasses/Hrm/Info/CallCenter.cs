using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SspUis.DataLayer.EfClasses;

[Table("info_call_center", Schema = "hrm")]
public class CallCenter
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("date", TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }

    [Column("call_count")]
    public int CallCount { get; set; }

}   
