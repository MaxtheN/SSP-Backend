using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_prtn_contract_sign", Schema = "partner")]
    public partial class PrtnContractSign : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("prtn_contract_type_table_id")]
        public int PrtnContractTypeTableId { get; set; }
        [Column("organization_sign_id")]
        public int? OrganizationSignId { get; set; }
        [Column("organization_id")]
        public int? OrganizationId { get; set; }
        [Column("status_id")]
        public int? StatusId { get; set; }
        [Column("sign_file")]
        public Guid SignFile { get; set; }
        [Column("data_file")]
        public Guid DataFile { get; set; }
        [Column("signed_user_info")]
        public string SignedUserInfo { get; set; }
        [Column("is_signed")]
        public bool IsSigned { get; set; }
        [Column("signed_at", TypeName = "timestamp without time zone")]
        public DateTime? SignedAt { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OrganizationSignId))]
        public virtual OrganizationSign OrganizationSign { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(PrtnContract.Signs))]
        public virtual PrtnContract Owner { get; set; }
        [ForeignKey(nameof(PrtnContractTypeTableId))]
        public virtual PrtnContractTypeTable PrtnContractTypeTable { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
    }
}
