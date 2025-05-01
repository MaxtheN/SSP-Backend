using DocumentFormat.OpenXml.Spreadsheet;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DualContractDlDto<TDto> : EntityDto<TDto, DualContract>
    where TDto : DualContractDlDto<TDto>
{
    [Column("application_id")]
    public long ApplicationId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EduTypeId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int LevelId { get; set; }
    public Guid Id2 { get; set; }
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string RektorName { get; set; }
    [LocalizedStringLength(9)]
    public string Postal { get; set; }
    [LocalizedStringLength(25)]
    public string OrganizationPhoneNumber { get; set; }
    public DateOnly? PassportExpiration { get; set; }
    public DateOnly DocDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int InstituteId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int SpecialityId { get; set; }
    [LocalizedRequired]
    public string Pinfl { get; set; }
    [LocalizedRequired]
    public string StudentFullname { get; set; }
    public int EduYear { get; set; }
    [LocalizedRequired]
    public string PassportSeria { get; set; }
    [LocalizedRequired]
    public string PassportNumber { get; set; }
    public DateOnly DateOfBrithday { get; set; }
    public int? DualEducationTypeId { get; set; }
    [LocalizedRequired]
    public int ExternalId { get; set; }
    public override DualContract CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.SIGNING;
        return entity;
    }

    public override void UpdateEntity(DualContract entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}