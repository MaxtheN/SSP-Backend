using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public class ContractorSurveyListDto : ILinkToEntity<ContractorSurvey>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public DateOnly DocOn { get; set; }
    public string DocNumber { get; set; }
    public DateOnly StartOn { get; set; }
    public DateOnly StartEnd { get; set; }
    public DateOnly RealStartOn { get; set; }
    public DateOnly RealStartEnd { get; set; }
    public string InspectionOrganization { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public string Questionnaire { get; set; }
    public string InspectionType { get; set; }
}
