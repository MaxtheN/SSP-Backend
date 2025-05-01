using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_application_chamber_service", Schema = "memship")]
    [Index(nameof(OwnerId), nameof(NeedChamberServiceId), Name = "uc_need_chamber_service", IsUnique = true)]
    public partial class MemshipApplicationChamberService :IHaveIdProp<long>, IHaveSingleUniqueForeignKey<int>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("need_chamber_service_id")]
        public int NeedChamberServiceId { get; set; }

        [ForeignKey(nameof(NeedChamberServiceId))]
        public virtual NeedChamberService NeedChamberService { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MemshipApplication.ChamberServices))]
        public virtual MemshipApplication Owner { get; set; }
        public object GetUniqueForeignKey() => NeedChamberServiceId;

        public void SetUniqueForeignKey(int foreignKey) => NeedChamberServiceId = foreignKey;
    }
}
