using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE;
using WEBASE.Models;
using WEBASE.Utility;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_user")]
    [Index(nameof(OrganizationId), nameof(StateId), Name = "sys_user_index_organization_id_state_id")]
    [Index(nameof(UserName), Name = "sys_user_unique_index_user_name", IsUnique = true)]
    [Index(nameof(PhoneNumber), Name = "idx_unique_phone_number", IsUnique = true)]
    public partial class User : IHaveIdProp<int>, IHaveStateId
    {
        public User()
        {
            RoleModules = new HashSet<RoleModule>();
            UserRoles = new HashSet<UserRole>();
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
        [Column("phone_number")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("person_id")]
        public int PersonId { get; set; }
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
        [Column("employeemanageid")]
        public long? EmployeeManageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(EfClasses.Organization.Users))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(EfClasses.Person.Users))]
        public virtual Person Person { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage? EmployeeManage { get; set; }
        [InverseProperty(nameof(RoleModule.CreatedUser))]
        public virtual ICollection<RoleModule> RoleModules { get; set; }
        [InverseProperty(nameof(UserRole.User))]
        public virtual ICollection<UserRole> UserRoles { get; set; }

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

        //public static Expression<Func<User, string>> GetFullName()
        //{
        //    return a => a.Person.SurnameLatin + " " + a.Person.NameLatin + " " + a.Person.PatronymLatin;
        //}

        //public static Expression<Func<User, string>> ShortName()
        //{
        //    return a => a.Person.NameLatin[0] + ". " + (a.Person.PatronymLatin != null ? a.Person.PatronymLatin[0] : "") + ". " + a.Person.SurnameLatin;
        //}
    }
}
