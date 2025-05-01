using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_prtn_contract_type_table", Schema = "partner")]
    public partial class PrtnContractTypeTable : IHaveIdProp<int>, IHaveStateId
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("order_number")]
        public int OrderNumber { get; set; }
        [Column("sign_organization_type_id")]
        public int SignOrganizationTypeId { get; set; }
        [Column("position_id")]
        public int? PositionId { get; set; }
        [Column("organization_id")]
        public int? OrganizationId { get; set; }
        [Column("region_id")]
        public int? RegionId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual PrtnContractType Owner { get; set; }
        [ForeignKey(nameof(SignOrganizationTypeId))]
        public virtual SignOrganizationType SignOrganizationType { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual Position Position { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
    }
}
