using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_prtn_reject_reason", Schema = "partner")]
    public partial class PrtnRejectReason : IHaveIdProp<int>, IHaveStateId
    {
        public PrtnRejectReason()
        {
            Translates = new HashSet<PrtnRejectReasonTranslate>();
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
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("prtn_contract_type_id")]
        public int? PrtnContractTypeId { get; set; }
        [Column("prtn_contract_type_table_id")]
        public int? PrtnContractTypeTableId { get; set; }
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

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [ForeignKey(nameof(PrtnContractTypeId))]
        public virtual PrtnContractType PrtnContractType { get; set; }

        [ForeignKey(nameof(PrtnContractTypeTableId))]
        public virtual PrtnContractTypeTable PrtnContractTypeTable { get; set; }

        [InverseProperty(nameof(PrtnRejectReasonTranslate.Owner))]
        public virtual ICollection<PrtnRejectReasonTranslate> Translates { get; set; }
    }
}
