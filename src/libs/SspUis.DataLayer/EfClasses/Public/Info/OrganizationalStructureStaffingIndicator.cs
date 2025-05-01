using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_organizational_structure_staffing_indicator")]
//[Index(nameof(OwnerId), Name = "ix_info_organizational_structure_staffing_indicator__owner")]
public partial class OrganizationalStructureStaffingIndicator : IHaveIdProp<int>
{
    public OrganizationalStructureStaffingIndicator()
    {
        Tables = new HashSet<OrganizationalStructureStaffingIndicatorTable>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
    [Column("staffing_indicator_id")]
    public int StaffingIndicatorId { get; set; }
    [Column("percentage")]
    [Precision(18, 2)]
    public decimal Percentage { get; set; }
    [Column("calc_order_code")]
    public int CalcOrderCode { get; set; }
    [Column("is_total")]
    public bool IsTotal { get; set; }
    [Column("display_order_code")]
    public int DisplayOrderCode { get; set; }
    [Column("is_calculation_kind_total")]
    public bool IsCalculationKindTotal { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual OrganizationalStructure Owner { get; set; }
    [ForeignKey(nameof(StaffingIndicatorId))]
    public virtual StaffingIndicator StaffingIndicator { get; set; }
    [InverseProperty(nameof(OrganizationalStructureStaffingIndicatorTable.Owner))]
    public virtual ICollection<OrganizationalStructureStaffingIndicatorTable> Tables { get; set; }
}
