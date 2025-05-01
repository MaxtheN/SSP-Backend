using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Public.Hl
{
    [Table("hl_person_log")]
    public partial class PersonLog : IHaveIdProp<int> 
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("pinfl")]
        [StringLength(14)]
        public string Pinfl { get; set; }
        [Required]
        [Column("passport_seria")]
        [StringLength(50)]
        public string PassportSeria { get; set; }
        [Required]
        [Column("passport_number")]
        [StringLength(50)]
        public string PassportNumber { get; set; }
        [Column("passport_date", TypeName = "timestamp without time zone")]
        public DateTime? PassportDate { get; set; }
        [Column("passport_expiration", TypeName = "timestamp without time zone")]
        public DateTime? PassportExpiration { get; set; }
        [Column("passport_div_name")]
        [StringLength(500)]
        public string PassportDivName { get; set; }
        [Column("person_id")]
        public int PersonId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }
    }
}
