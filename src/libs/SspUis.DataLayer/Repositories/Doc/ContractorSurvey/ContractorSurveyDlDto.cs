using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ContractorSurveyDlDto<TDto> : EntityDto<TDto, ContractorSurvey>
    where TDto : ContractorSurveyDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string DocNumber { get; set; }
    //[LocalizedRequired]
    //public DateOnly StartOn { get; set; }
    //[LocalizedRequired]
    //public DateOnly StartEnd { get; set; }
    //[LocalizedRequired]
    //public DateOnly RealStartOn { get; set; }
    //[LocalizedRequired]
    //public DateOnly RealStartEnd { get; set; }
    [LocalizedRequired]
    public long QuestionnaireId { get; set; }
    [LocalizedRequired]
    public int InspectionTypeId { get; set; }
    public int? InspectionOrganizationId { get; set; }
    public string InspectionOrganization { get; set; }
    //public List<ContractorSurveyTableDlDto> Tables { get; set; } = new();
    public List<ContractorSurveyGroupDlDto> Groups { get; set; } = new();

    protected override Action<IMappingExpression<TDto, ContractorSurvey>> AlterMapping => cfg => cfg
        .ForMember(x => x.Groups, x => x.Ignore());

    public override ContractorSurvey CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        entity.DocOn = DateOnly.FromDateTime(DateTime.Now);
        Groups.AddTo(entity.Groups);

        return entity;
    }

    public override void UpdateEntity(ContractorSurvey entity)
    {
        base.UpdateEntity(entity);
        Groups.ApplyChangesTo<long, ContractorSurveyGroupDlDto, ContractorSurveyGroup>(entity.Groups);
    }
}
