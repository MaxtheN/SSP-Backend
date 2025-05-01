using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;
[Table("info_specialty_billing", Schema = "dual_edu")]
[Index(nameof(Id), Name = "idx_info_specialty_billing_id", IsUnique = true)]
public class SpecialtyBilling : IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("sequence_number")]
    public int? SequenceNumber { get; set; }
    [Column("edu_are_name")]
    public string EduAreName { get; set; }
    [Column("organization")]
    public string Organization { get; set; }
    [Column("faculty")]
    public string Faculty { get; set; }
    [Column("faculty_code")]
    public string FacultyCode { get; set; }
    [Column("edu_form")]
    public string EduForm { get; set; }
    [Column("edu_speciality_classifier")]
    public string EduSpecialityClassifier { get; set; }
    [Column("edu_speciality_classifier_code")]
    public string EduSpecialityClassifierCode { get; set; }
    [Column("edu_type")]
    public string EduType { get; set; }
    [Column("code")]
    public string Code { get; set; }
    [Column("short_name")]
    public string ShortName { get; set; }
    [Column("full_name")]
    public string FullName { get; set; }
    [Column("organization_id")]
    public int? OrganizationId { get; set; }
    [Column("faculty_id")]
    public int? FacultyId { get; set; }
    [Column("edu_area_id")]
    public int? EduAreaId { get; set; }
    [Column("edu_speciality_classifier_id")]
    public int? EduSpecialityClassifierId { get; set; }
    [Column("edu_type_id")]
    public int? EduTypeId { get; set; }
    [Column("edu_period")] 
    public int? EduPeriod { get; set; }
    [Column("edu_form_id")]
    public int? EduFormId { get; set; }
    [Column("hemis_external_code")]
    public Guid HemisExternalCode { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }
    //[InverseProperty(nameof(DualApplicationTable.Specialty))]
    //public virtual ICollection<DualApplicationTable> DualApplications { get; set; }
}