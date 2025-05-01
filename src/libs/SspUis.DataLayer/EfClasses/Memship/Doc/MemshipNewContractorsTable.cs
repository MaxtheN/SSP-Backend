using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_new_contractors_table", Schema = "memship")]
    [Index(nameof(OwnerId), Name = "ux_doc_memship_new_contractors_table__owner")]
    public partial class MemshipNewContractorsTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
        [Column("legal_count")]
        public int LegalCount { get; set; }
        [Column("physical_count")]
        public int PhysicalCount { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(MemshipNewContractor.Tables))]
        public virtual MemshipNewContractor Owner { get; set; }
    }
}
