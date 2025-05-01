using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("info_institute_billing", Schema = "dual_edu")]
[Index(nameof(Id), Name = "idx_info_institute_billing_id", IsUnique = true)]
public partial class InstituteBilling : IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [Column("country")]
    public string Country { get; set; }
    [Required]
    [Column("region")]
    public string Region { get; set; }
    [Required]
    [Column("district")]
    public string District { get; set; }
    [Column("director")]
    public string Director { get; set; }
    [Column("short_name")]
    public string ShortName { get; set; }
    [Column("full_name")]
    public string FullName { get; set; }
    [Column("inn")]
    public string Inn { get; set; }
    [Column("order_code")]
    public string OrderCode { get; set; }
    [Column("hemis_external_code")]
    public string HemisExternalCode { get; set; }
    [Column("address")]
    public string Address { get; set; }
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("faks")]
    public string Faks { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(5);
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }
    //[InverseProperty(nameof(DualApplicationTable.Institute))]
    //public virtual ICollection<DualApplicationTable> DualApplications { get; set; }
}