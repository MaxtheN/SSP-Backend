using SspUis.DataLayer.EfClasses.DualEdu;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;
public class InstituteBillingDlDto<TDto> : EntityDto<TDto, InstituteBilling>
    where TDto : InstituteBillingDlDto<TDto>
{
    public int Id { get; set; }
    public string Country { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string OrderCode { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(20)]
    public string Inn { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public string Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Faks { get; set; }
    public string HemisExternalCode { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public string Director { get; set; }

    public override InstituteBilling CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.CreatedAt = DateTime.UtcNow.AddHours(5);
        return entity;
    }

    public override void UpdateEntity(InstituteBilling entity)
    {
        entity.ModifiedAt = DateTime.UtcNow.AddHours(5);
        base.UpdateEntity(entity);
    }
}