using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE;
using WEBASE.Models;
using WEBASE.Utility;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_businessman_user", Schema = "my")]
    [Index(nameof(UserName), Name = "sys_businessman_unique_index_user_name", IsUnique = true)]
    public partial class BusinessmanUser : IHaveIdProp<int>, IHaveStateId
    {
        public BusinessmanUser()
        {
            DeviceLogs = new HashSet<BusinessmanUserDeviceLog>();
            TrustedDevices = new HashSet<BusinessmanUserTrustedDevice>();
            BusinessmanUserInContractors = new HashSet<BusinessmanUserInContractor>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("user_name")]
        [StringLength(250)]
        public string UserName { get; set; }
        [Required]
        [Column("password_hash")]
        [StringLength(250)]
        public string PasswordHash { get; set; }
        [Required]
        [Column("password_salt")]
        [StringLength(250)]
        public string PasswordSalt { get; set; }
        [Column("email")]
        [StringLength(250)]
        public string Email { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        //[Required]
        [Column("inn")]
        [StringLength(50)]
        public string Inn { get; set; }
        [Column("pinfl")]
        [StringLength(50)]
        public string Pinfl { get; set; }
        [Column("e_sign_certificate_number")]
        [StringLength(100)]
        public string ESignCertificateNumber { get; set; }
        [Column("sign_data_file")]
        public Guid? SignDataFile { get; set; }
        [Column("is_pinfl")]
        public bool IsPinfl { get; set; }

        [Column("language_id")]
        public int? LanguageId { get; set; }
        [Column("last_access_time", TypeName = "timestamp without time zone")]
        public DateTime? LastAccessTime { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [InverseProperty(nameof(BusinessmanUserDeviceLog.BusinessmanUser))]
        public virtual ICollection<BusinessmanUserDeviceLog> DeviceLogs { get; set; }
        [InverseProperty(nameof(BusinessmanUserTrustedDevice.BusinessmanUser))]
        public virtual ICollection<BusinessmanUserTrustedDevice> TrustedDevices { get; set; }
        [InverseProperty(nameof(BusinessmanUserInContractor.BusinessmanUser))]
        public virtual ICollection<BusinessmanUserInContractor> BusinessmanUserInContractors { get; set; }

        public bool IsValidPassword(string password)
        {
            return !(password.NullOrEmpty() || new CustomPaswordHasher().HashPassword(password, PasswordSalt) != PasswordHash);
        }

        public void SetPassword(string password, bool isNewEntity = false)
        {
            if (isNewEntity && password.NullOrEmpty())
                throw new ArgumentException("password is required for new user entity", nameof(password));

            if (isNewEntity || !password.NullOrEmpty())
            {
                PasswordSalt = HashHelper.CreateRandomSalt();
                PasswordHash = new CustomPaswordHasher().HashPassword(password, PasswordSalt);
            }
        }

        public static string GetCorrectUserName(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace("+", "").Replace("-", "").Replace(" ", "");
            if (!phoneNumber.StartsWith("998") && phoneNumber.Length != 12)
                phoneNumber = $"998{phoneNumber}";
            return phoneNumber;
        }
    }
}
