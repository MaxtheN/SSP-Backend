using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_organizational_structure")]
public partial class OrganizationalStructure : IHaveStateId, IHaveIdProp<int>
{
    public OrganizationalStructure()
    {
        Translates = new HashSet<OrganizationalStructureTranslate>();
        Organizations = new HashSet<Organization>();
        StructureCalculationKind = new HashSet<OrganizationalStructureCalculationKind>();
        StructureStaffingIndicator = new HashSet<OrganizationalStructureStaffingIndicator>();
        StructurePosition = new HashSet<OrganizationalStructurePosition>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_code")]
    public int? OrderCode { get; set; }
    [Required]
    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(500)]
    public string FullName { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("corr_coef")]
    [Precision(18, 4)]
    public decimal? CorrCoef { get; set; }
    [Column("is_parent")]
    public bool? IsParent { get; set; }
    [Column("structure_type")]
    public int StructureType { get; set; }
    [Column("code_number")]
    public int CodeNumber { get; set; }
    [Required]
    [Column("code_symbol")]
    [StringLength(1)]
    public string CodeSymbol { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }


    [InverseProperty(nameof(OrganizationalStructureTranslate.Owner))]
    public virtual ICollection<OrganizationalStructureTranslate> Translates { get; set; }
    [InverseProperty(nameof(Organization.OrganizationalStructure))]
    public virtual ICollection<Organization> Organizations { get; set; }

    [InverseProperty(nameof(OrganizationalStructureCalculationKind.Owner))]
    public virtual ICollection<OrganizationalStructureCalculationKind> StructureCalculationKind { get; set; }
    [InverseProperty(nameof(OrganizationalStructurePosition.Owner))]
    public virtual ICollection<OrganizationalStructurePosition> StructurePosition { get; set; }
    [InverseProperty(nameof(OrganizationalStructureStaffingIndicator.Owner))]
    public virtual ICollection<OrganizationalStructureStaffingIndicator> StructureStaffingIndicator { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
}
