using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_contract_group", Schema = "srv")]
    public class ServiceContractGroup : IHaveIdProp<long>
    {
        public ServiceContractGroup()
        {
            Tables = new HashSet<ServiceContractTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Column("group_id")]
        public int? GroupId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ServiceContract.Groups))]
        public virtual ServiceContract Owner { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual NeedChamberServiceGroup NeedChamberServiceGroup { get; set; }

        [InverseProperty(nameof(ServiceContractTable.Owner))]
        public virtual ICollection<ServiceContractTable> Tables { get; set; }
    }
}
