using SspUis.DataLayer.EfClasses.DualEdu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_dual_contract", Schema = "dual_edu")]
public partial class DualContract : IHaveIdProp<long>, IHaveStatusId
{
    public DualContract()
    {
        Signs = new HashSet<DualContractSign>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("application_id")]
    public long ApplicationId { get; set; }
    [Column("level_id")]
    public int LevelId { get; set; }
    [Column("edu_type_id")]
    public int EduTypeId { get; set; }
    [Column("id_2")]
    public Guid Id2 { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(20)]
    public string DocNumber { get; set; }
    [Column("doc_date")]
    public DateOnly DocDate { get; set; }
    [Column("passport_expiration")]
    public DateOnly? PassportExpiration { get; set; }
    [Column("postal")]
    [StringLength(6)]
    public string Postal { get; set; }
    [Column("organization_phone_number")]
    [StringLength(25)]
    public string OrganizationPhoneNumber { get; set; }
    [Column("institute_id")]
    public int InstituteId { get; set; }
    [Column("speciality_id")]
    public int SpecialityId { get; set; }
    [Required]
    [Column("pinfl")]
    [StringLength(14)]
    public string Pinfl { get; set; }
    [Required]
    [Column("rektor_name")]
    [StringLength(250)]
    public string RektorName { get; set; }
    [Required]
    [Column("student_fullname")]
    public string StudentFullname { get; set; }
    [Column("edu_year")]
    public int EduYear { get; set; }
    [Required]
    [Column("passport_seria")]
    [StringLength(2)]
    public string PassportSeria { get; set; }
    [Required]
    [Column("passport_number")]
    [StringLength(7)]
    public string PassportNumber {  get; set; }
    [Column("date_of_brithday")]
    public DateOnly DateOfBrithday { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("external_id")]
    public int? ExternalId { get; set; }
    [Column("dual_education_type_id")]
    public int? DualEducationTypeId { get; set; }
    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_date", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }
    [Column("web_imzo_secret_key")]
    public string? WebImzoSecretKey { get; set; }
    [Column("web_imzo_request_id")]
    public Guid? WebImzoRequestId { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }
    [ForeignKey(nameof(ApplicationId))]
    public virtual Application Application { get; set; }
    [ForeignKey(nameof(EduTypeId))]
    public virtual EduType EduType { get; set; }
    [ForeignKey(nameof(InstituteId))]
    public virtual InstituteBilling Institute { get; set; }
    [ForeignKey(nameof(SpecialityId))]
    public virtual SpecialtyBilling Speciality { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [ForeignKey(nameof(DualEducationTypeId))]
    public virtual DualEducationType DualEducationType { get; set; }
    [InverseProperty(nameof(DualContractSign.Owner))]
    public virtual ICollection<DualContractSign> Signs { get; set; }
}