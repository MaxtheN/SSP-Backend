using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_erp_user_device_log", Schema = "public")]
    [Index(nameof(UniqueKey), Name = "sys_user_device_log_unique_index_unique_key", IsUnique = true)]
    public class UserDeviceLog : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("unique_key")]
        [StringLength(100)]
        public string UniqueKey { get; set; }

        [Column("ip_address")]
        [StringLength(200)]
        public string IpAddress { get; set; }

        [Column("user_agent", TypeName = "character varying")]
        public string UserAgent { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("date_of_expire", TypeName = "timestamp without time zone")]
        public DateTime DateOfExpire { get; set; }

        [Column("phone_number")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Column("sms_code")]
        [StringLength(10)]
        public string SmsCode { get; set; }

        [ForeignKey(nameof(UserId))]
        //[InverseProperty(nameof(User.DeviceLogs))]
        public virtual User User { get; set; }
    }
}