using AutoMapper;

namespace SspUis.DataLayer.Repositories.Appeal;

public class CreateAppealApplicationDlDto : AppealApplicationDlDto<CreateAppealApplicationDlDto>
{
    [IgnoreMap]
    public CreatePersonDlDto Person { get; set; } = new();
    [IgnoreMap]
    public string? ContractorInn { get; set; }
}
