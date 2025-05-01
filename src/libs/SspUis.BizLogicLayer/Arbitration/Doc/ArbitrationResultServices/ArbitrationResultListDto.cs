using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ArbitrationResultListDto :
    DocumentListDto<long>,
    ILinkToEntity<ArbitrationResult>
{
    public string Status { get; set; }
    public string DocNumber { get; set; }
    public decimal Amount { get; set; }
    public decimal PayedAmount { get; set; }
    public long ArbitrationCourtApplicationId { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public long ResponsibleContractorId { get; set; }
    public string ResponsibleContractor { get; set; }
}
