using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_sign_organization_type")]
    public partial class SignOrganizationType : IHaveIdProp<int>
    {
        public SignOrganizationType()
        {
            Translates = new HashSet<SignOrganizationTypeTranslate>();
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
        [StringLength(300)]
        public string FullName { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [InverseProperty(nameof(SignOrganizationTypeTranslate.Owner))]
        public virtual ICollection<SignOrganizationTypeTranslate> Translates { get; set; }
    }
}
