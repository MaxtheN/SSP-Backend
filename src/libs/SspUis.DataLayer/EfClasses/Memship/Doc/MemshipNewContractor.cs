using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_new_contractors", Schema = "memship")]
    public partial class MemshipNewContractor : IHaveIdProp<long>, IHaveStatusId
    {
        public MemshipNewContractor()
        {
            Tables = new HashSet<MemshipNewContractorsTable>();
            Files = new HashSet<MemshipNewContractorsFile>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Column("from_date")]
        public DateOnly FromDate { get; set; }
        [Column("to_date")]
        public DateOnly ToDate { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("total_legal_count")]
        public int TotalLegalCount { get; set; }
        [Column("total_physical_count")]
        public int TotalPhysicalCount { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(MemshipNewContractorsFile.Owner))]
        public virtual ICollection<MemshipNewContractorsFile> Files { get; set; }
        [InverseProperty(nameof(MemshipNewContractorsTable.Owner))]
        public virtual ICollection<MemshipNewContractorsTable> Tables { get; set; }
    }
}
