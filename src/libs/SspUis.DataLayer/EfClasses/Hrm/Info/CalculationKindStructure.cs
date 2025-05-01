using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_calculation_kind_structure", Schema = "hrm")]
    [Index(nameof(OwnerId), nameof(OrganizationalStructureId), Name = "ux_info_calculation_kind_structure__owner", IsUnique = true)]
    public class CalculationKindStructure: IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("organizational_structure_id")]
        public int OrganizationalStructureId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }


        [ForeignKey(nameof(OwnerId))]
        public virtual CalculationKind Owner { get; set; } = null!;
        [ForeignKey(nameof(OrganizationalStructureId))]
        public virtual OrganizationalStructure OrganizationStructure { get; set; } = null!;

    }
}
