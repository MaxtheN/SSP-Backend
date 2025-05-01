using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("hl_employee_relative", Schema = "hrm")]
[Index(nameof(OwnerId), Name = "ix_hl_employee_relative__employee")]
public partial class EmployeeRelative : IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
    [Column("on_date")]
    public DateOnly OnDate { get; set; }
    [Column("relative_degree_id")]
    public int RelativeDegreeId { get; set; }
    [Required]
    [Column("family_name")]
    [StringLength(100)]
    public string FamilyName { get; set; }
    [Required]
    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; set; }
    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(200)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(300)]
    public string FullName { get; set; }
    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }
    [Column("pinfl")]
    [StringLength(14)]
    public string Pinfl { get; set; }
    [Column("has_died")]
    public bool HasDied { get; set; }
    [Column("date_of_death")]
    public DateOnly? DateOfDeath { get; set; }
    [Column("country_id")]
    public int? CountryId { get; set; }
    [Column("region_id")]
    public int? RegionId { get; set; }
    [Column("district_id")]
    public int? DistrictId { get; set; }
    [Column("address")]
    [StringLength(300)]
    public string Address { get; set; }
    [Column("phone_number")]
    [StringLength(30)]
    public string PhoneNumber { get; set; }
    [Column("identity_document_id")]
    public int? IdentityDocumentId { get; set; }
    [Column("document_series")]
    [StringLength(5)]
    public string DocumentSeries { get; set; }
    [Column("document_number")]
    [StringLength(10)]
    public string DocumentNumber { get; set; }
    [Column("date_of_issue")]
    public DateOnly? DateOfIssue { get; set; }
    [Column("date_of_expire")]
    public DateOnly? DateOfExpire { get; set; }
    [Column("issue_organization")]
    [StringLength(100)]
    public string IssueOrganization { get; set; }
    [Column("nationality_id")]
    public int? NationalityId { get; set; }
    [Column("citizenship_id")]
    public int? CitizenshipId { get; set; }
    [Column("relative_work_place")]
    [StringLength(100)]
    public string? RelativeWorkPlace { get; set; }
    [Column("relative_work_place_position")]
    [StringLength(100)]
    public string? RelativeWorkPlacePosition { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(CitizenshipId))]
    public virtual Citizenship? Citizenship { get; set; }
    [ForeignKey(nameof(CountryId))]
    public virtual Country? Country { get; set; }
    [ForeignKey(nameof(DistrictId))]
    public virtual District? District { get; set; }
    [ForeignKey(nameof(IdentityDocumentId))]
    public virtual IdentityDocument IdentityDocument { get; set; }
    [ForeignKey(nameof(NationalityId))]
    public virtual Nationality? Nationality { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual Employee Owner { get; set; }
    [ForeignKey(nameof(RegionId))]
    public virtual Region? Region { get; set; }
    [ForeignKey(nameof(RelativeDegreeId))]
    public virtual RelativeDegree RelativeDegree { get; set; }
}
