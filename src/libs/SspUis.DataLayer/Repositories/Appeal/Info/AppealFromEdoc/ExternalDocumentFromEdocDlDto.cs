using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ExternalDocumentFromEdocDlDto<TDto> : EntityDto<TDto, ExternalDocumentFromEdoc>
        where TDto : ExternalDocumentFromEdocDlDto<TDto>
{
    public DateTime? TermExecution { get; set; }
    public string? Assignment { get; set; }
    public int? OrganizationId { get; set; }
    public int? ProcessId { get; set; }
    public long? AppealApplicationId { get; set; }
    public long? CallCenterAppealId { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string RegNumber { get; set; }
    public DateTime? RegDate { get; set; }
    public DateTime? OutgoingDocCreatedData { get; set; }

    public override void UpdateEntity(ExternalDocumentFromEdoc entity)
    {
        entity.ModifiedAt = DateTime.UtcNow;
        base.UpdateEntity(entity);
    }
}
