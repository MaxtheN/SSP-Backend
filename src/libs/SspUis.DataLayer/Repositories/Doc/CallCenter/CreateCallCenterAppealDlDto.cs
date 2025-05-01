using AutoMapper;

namespace SspUis.DataLayer.Repositories;

public class CreateCallCenterAppealDlDto : CallCenterAppealDlDto<CreateCallCenterAppealDlDto>
{
    [IgnoreMap]
    public CreatePersonDlDto Person { get; set; } = new();
    [IgnoreMap]
    public string? ContractorInn { get; set; }
}
