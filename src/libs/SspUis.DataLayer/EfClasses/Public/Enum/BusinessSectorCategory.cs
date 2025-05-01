using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_business_sector_category")]
    public partial class BusinessSectorCategory : IHaveStateId, IHaveIdProp<int>
    {
        public BusinessSectorCategory()
        {
            BusinessActivityTypes = new HashSet<BusinessActivityType>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
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
        [InverseProperty(nameof(BusinessActivityType.BusinessCtor))]
        public virtual ICollection<BusinessActivityType> BusinessActivityTypes { get; set; }
    }
}
