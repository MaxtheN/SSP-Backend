namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;

public class ArbitrationCourtApplicationSortFilterOptions : DocumentSortFilterOptions
{
    public int? ContractorResponsibleTypeId { get; set; }
    public int? CurrencyId { get; set; }
    public int? ClaimResponsibleTypeId { get; set; }

}
