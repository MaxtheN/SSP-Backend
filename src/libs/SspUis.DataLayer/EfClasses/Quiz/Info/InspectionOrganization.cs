using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfClasses;

[Table("info_inspection_organization", Schema = "quiz")]
[Index(nameof(Inn), Name = "info_inspection_organization_unique_index_inn", IsUnique = true)]
public partial class InspectionOrganization
{
    public InspectionOrganization()
    {
        ContractorSurveys = new HashSet<ContractorSurvey>();
        Translates = new HashSet<InspectionOrganizationTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_code")]
    [StringLength(50)]
    public string OrderCode { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(300)]
    public string FullName { get; set; }
    [Column("inn")]
    [StringLength(9)]
    public string Inn { get; set; }
    [Column("address")]
    [StringLength(500)]
    public string Address { get; set; }
    [Column("phone_number")]
    [StringLength(250)]
    public string PhoneNumber { get; set; }
    [Column("country_id")]
    public int CountryId { get; set; }
    [Column("region_id")]
    public int RegionId { get; set; }
    [Column("district_id")]
    public int? DistrictId { get; set; }
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

    [ForeignKey(nameof(CountryId))]
    public virtual Country Country { get; set; }
    [ForeignKey(nameof(DistrictId))]
    public virtual District District { get; set; }
    [ForeignKey(nameof(RegionId))]
    public virtual Region Region { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(ContractorSurvey.InspectionOrganizationNavigation))]
    public virtual ICollection<ContractorSurvey> ContractorSurveys { get; set; }
    [InverseProperty(nameof(InspectionOrganizationTranslate.Owner))]
    public virtual ICollection<InspectionOrganizationTranslate> Translates { get; set; }
}
