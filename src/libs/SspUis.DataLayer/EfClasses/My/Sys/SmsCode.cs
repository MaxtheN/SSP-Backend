using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_sms_code", Schema = "my")]
    [Index(nameof(RequestId), Name = "sys_sms_code_unique_index_request_id", IsUnique = true)]
    public partial class SmsCode
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("phone_number")]
        [StringLength(12)]
        public string PhoneNumber { get; set; }
        [Required]
        [Column("code")]
        [StringLength(10)]
        public string Code { get; set; }
        [Column("expire_date", TypeName = "timestamp without time zone")]
        public DateTime? ExpireDate { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("password_hash")]
        [StringLength(100)]
        public string PasswordHash { get; set; }
        [Required]
        [Column("password_salt")]
        [StringLength(100)]
        public string PasswordSalt { get; set; }
        [Required]
        [Column("request_id")]
        [StringLength(50)]
        public string RequestId { get; set; }
    }
}
