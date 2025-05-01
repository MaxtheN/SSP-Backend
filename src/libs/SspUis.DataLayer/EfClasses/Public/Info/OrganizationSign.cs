using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;
using WEBASE.Utility;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_organization_sign")]
    public partial class OrganizationSign : IHaveIdProp<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("prtn_contract_type_table_id")]
        public int PrtnContractTypeTableId { get; set; }
        [Required]
        [Column("pinfl")]
        [StringLength(14)]
        public string Pinfl { get; set; }
        [Required]
        [Column("first_name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [Column("middle_name")]
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(300)]
        public string FullName { get; set; }
        [Required]
        [Column("passport_seria")]
        [StringLength(50)]
        public string PassportSeria { get; set; }
        [Required]
        [Column("passport_number")]
        [StringLength(50)]
        public string PassportNumber { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("birth_on")]
        public DateOnly BirthOn { get; set; }
        [Column("expire_on")]
        public DateOnly? ExpireOn { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(EfClasses.Organization.Signs))]
        public virtual Organization Owner { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        [ForeignKey(nameof(PrtnContractTypeTableId))]
        public virtual PrtnContractTypeTable PrtnContractTypeTable { get; set; }


        public void SetFIO()
        {
            FullName = StringUtility.GetFullFIO(LastName, FirstName, MiddleName);
            ShortName = StringUtility.GetFIO(FullName);
        }
    }
}
