using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_business_sector")]
    [Index(nameof(Code), Name = "uc_bs_code", IsUnique = true)]
    public partial class BusinessSector : IHaveStateId, IHaveIdProp<int>
    {
        public BusinessSector()
        {
            Translates = new HashSet<BusinessSectorTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(BusinessSectorTranslate.Owner))]
        public virtual ICollection<BusinessSectorTranslate> Translates { get; set; }
    }
}
