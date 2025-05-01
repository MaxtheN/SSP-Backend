namespace SspUis.BizLogicLayer;

public class ApplicationForCourtSortFilterOptions : DocumentSortFilterOptions
{
    public int? ClaimOrganizationId { get; set; }
    public long? ContractorId { get; set; }
    public string? ContractorInn { get; set; }
    public int? StatusId { get; set; }
    public int? StepId { get; set; }
    public bool IsEmployee { get; set; } = false;
}
