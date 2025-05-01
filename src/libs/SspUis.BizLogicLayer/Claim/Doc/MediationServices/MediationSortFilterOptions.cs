namespace SspUis.BizLogicLayer;

public class MediationSortFilterOptions : DocumentSortFilterOptions
{
    public int? MediationResultId { get; set; }
    public int? ClaimNeedCourtId { get; set; }
    public long? ContractorId { get; set; }
    public int? StatusId { get; set; }
    public bool IsEmployee { get; set; } = false;
}
