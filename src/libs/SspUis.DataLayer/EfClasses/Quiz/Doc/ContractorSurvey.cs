using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_contractor_survey", Schema = "quiz")]
[Index(nameof(ContractorId), nameof(StatusId), nameof(DocOn), Name = "ux_doc_contractor_survey_qa", IsUnique = true)]
public partial class ContractorSurvey : IHaveIdProp<long>
{
    public ContractorSurvey()
    {
        //Tables = new HashSet<ContractorSurveyTable>();
        Groups = new HashSet<ContractorSurveyGroup>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("doc_on")]
    public DateOnly DocOn { get; set; }

    [Required]
    [Column("doc_number")]
    [StringLength(50)]
    public string DocNumber { get; set; }

    //[Column("start_on")]
    //public DateOnly StartOn { get; set; }

    //[Column("start_end")]
    //public DateOnly StartEnd { get; set; }

    //[Column("real_start_on")]
    //public DateOnly RealStartOn { get; set; }

    //[Column("real_start_end")]
    //public DateOnly RealStartEnd { get; set; }

    [Column("questionnaire_id")]
    public long QuestionnaireId { get; set; }

    [Column("inspection_type_id")]
    public int InspectionTypeId { get; set; }

    [Column("inspection_organization_id")]
    public int? InspectionOrganizationId { get; set; }

    [Column("inspection_organization")]
    [StringLength(250)]
    public string InspectionOrganization { get; set; }

    [Column("contractor_id")]
    public long ContractorId { get; set; }

    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }

    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(InspectionOrganizationId))]
    public virtual InspectionOrganization InspectionOrganizationNavigation { get; set; }

    [ForeignKey(nameof(InspectionTypeId))]
    public virtual InspectionType InspectionType { get; set; }

    [ForeignKey(nameof(ContractorId))]
    public virtual Contractor Contractor { get; set; }

    [ForeignKey(nameof(QuestionnaireId))]
    public virtual Questionnaire Questionnaire { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }

    //[InverseProperty(nameof(ContractorSurveyTable.Owner))]
    //public virtual ICollection<ContractorSurveyTable> Tables { get; set; }

    [InverseProperty(nameof(ContractorSurveyGroup.Owner))]
    public virtual ICollection<ContractorSurveyGroup> Groups { get; set; }
}
