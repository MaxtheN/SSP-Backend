using GenericServices;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.QuestionGroupService;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public class ContractorSurveyDto : UpdateContractorSurveyDlDto, ILinkToEntity<ContractorSurvey>
{
    public DateOnly DocOn { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public string Questionnaire { get; set; }
    public string InspectionType { get; set; }
    [JsonIgnore]
    public List<ContractorSurveyTableDto> Tables { get; set; } = new();
    public List<QuestionGroupDto> Group { get; set; } = new();
}
