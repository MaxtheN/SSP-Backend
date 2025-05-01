using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_businessman_user_device_log", Schema = "my")]
    [Index(nameof(UniqueKey), Name = "sys_businessman_user_device_log_unique_index_unique_key", IsUnique = true)]
    public partial class BusinessmanUserDeviceLog
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("businessman_user_id")]
        public int BusinessmanUserId { get; set; }
        [Required]
        [Column("unique_key")]
        [StringLength(50)]
        public string UniqueKey { get; set; }
        [Column("ip_adress")]
        [StringLength(200)]
        public string IpAdress { get; set; }
        [Column("user_agent", TypeName = "character varying")]
        public string UserAgent { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("date_of_expire", TypeName = "timestamp without time zone")]
        public DateTime DateOfExpire { get; set; }
        [Column("mobile_number")]
        [StringLength(15)]
        public string MobileNumber { get; set; }
        [Column("sms_code")]
        [StringLength(10)]
        public string SmsCode { get; set; }

        [ForeignKey(nameof(BusinessmanUserId))]
        [InverseProperty(nameof(EfClasses.BusinessmanUser.DeviceLogs))]
        public virtual BusinessmanUser BusinessmanUser { get; set; }
    }
}
