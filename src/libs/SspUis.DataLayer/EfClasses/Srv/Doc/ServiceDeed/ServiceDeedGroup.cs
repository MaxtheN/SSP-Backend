using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_deed_group", Schema = "srv")]
    public partial class ServiceDeedGroup : IHaveIdProp<long>
    {
        public ServiceDeedGroup()
        {
            Tables = new HashSet<ServiceDeedTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
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
        public virtual ServiceDeed Owner { get; set; }
        [ForeignKey(nameof(GroupId))]
        public virtual NeedChamberServiceGroup NeedChamberServiceGroup { get; set; }
        [InverseProperty(nameof(ServiceDeedTable.Owner))]
        public virtual ICollection<ServiceDeedTable> Tables { get; set; }
    }
}
