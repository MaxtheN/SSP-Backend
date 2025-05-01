using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;
using WEBASE.EF;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_contractor_oked", Schema = "my")]
    public partial class ContractorOked : IHaveIdProp<int>, IHaveSingleUniqueForeignKey<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("oked_id")]
        public int OkedId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(OkedId))]
        public virtual Oked Oked { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Contractor.Okeds))]
        public virtual Contractor Owner { get; set; }

        public object GetUniqueForeignKey() => OkedId;

        public void SetUniqueForeignKey(int foreignKey) => OkedId = foreignKey;
    }
}
