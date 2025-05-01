using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_join_anti_corruption_application_employee", Schema = "corruption")]
    public partial class JoinAntiCorruptionApplicationEmployee : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("person")]
        [StringLength(250)]
        public string Person { get; set; }
        [Column("position")]
        [StringLength(250)]
        public string Position { get; set; }
        [Required]
        [Column("phone_number")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [Column("email")]
        [StringLength(250)]
        public string Email { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(JoinAntiCorruptionApplication.Employees))]
        public virtual JoinAntiCorruptionApplication Owner { get; set; }
    }
}
