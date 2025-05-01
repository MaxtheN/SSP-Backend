using SspUis.DataLayer.EfClasses.DualEdu;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class SpecialtyBillingDlDto<TDto> : EntityDto<TDto, SpecialtyBilling>
    where TDto : SpecialtyBillingDlDto<TDto>
{
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string Code { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string ShortName { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(500)]
    public string FullName { get; set; }
    public int? SequenceNumber { get; set; }
    public string EduAreName { get; set; }
    public int? EduAreaId { get; set; }
    public string Organization { get; set; }
    public int? OrganizationId { get; set; }
    public int? FacultyId { get; set; }
    public string Faculty { get; set; }
    public string FacultyCode { get; set; }
    public string EduForm { get; set; }
    public Guid HemisExternalCode { get; set; }
    public string EduSpecialityClassifier { get; set; }
    public string EduSpecialityClassifierCode { get; set; }
    public int? EduSpecialityClassifierId { get; set; }

    public override SpecialtyBilling CreateEntity()
    {
        var entity = base.CreateEntity();
        return entity;
    }

    public override void UpdateEntity(SpecialtyBilling entity)
    {
        base.UpdateEntity(entity);
    }
}