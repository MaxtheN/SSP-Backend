using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_organizational_structure_calculation_kind")]
//[Index(nameof(OwnerId), Name = "ix_info_organizational_structure_calculation_kind__owner")]
public partial class OrganizationalStructureCalculationKind : IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
    [Column("calculation_kind_id")]
    public int CalculationKindId { get; set; }
    [Column("percentage")]
    [Precision(18, 2)]
    public decimal Percentage { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(CalculationKindId))]
    public virtual CalculationKind CalculationKind { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual OrganizationalStructure Owner { get; set; }
}
