using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_organizational_structure_staffing_indicator_table")]
//[Index(nameof(OwnerId), Name = "ix_info_organizational_structure_staffing_indicator_table__owne")]
public partial class OrganizationalStructureStaffingIndicatorTable : IHaveSingleUniqueForeignKey<int>, IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
    [Column("staffing_indicator_id")]
    public int StaffingIndicatorId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(OrganizationalStructureStaffingIndicator.Tables))]
    public virtual OrganizationalStructureStaffingIndicator Owner { get; set; }
    [ForeignKey(nameof(StaffingIndicatorId))]
    public virtual StaffingIndicator StaffingIndicator { get; set; }
    public object GetUniqueForeignKey() => StaffingIndicatorId;
    public void SetUniqueForeignKey(int foreignKey) => StaffingIndicatorId = foreignKey;
}
