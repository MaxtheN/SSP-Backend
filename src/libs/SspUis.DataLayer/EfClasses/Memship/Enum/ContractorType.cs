using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_contractor_type", Schema = "memship")]
public partial class ContractorType
{
    public ContractorType()
    {
        Translates = new HashSet<ContractorTypeTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_code")]
    [StringLength(50)]
    public string OrderCode { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(300)]
    public string FullName { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty(nameof(ContractorTypeTranslate.Owner))]
    public virtual ICollection<ContractorTypeTranslate> Translates { get; set; }
   
}
