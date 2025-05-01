using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Memship;

public class DebtTableDto : DebtTableDlDto, ILinkToEntity<DebtTable>
{
    public string ContractorInn { get; set; }
    public string Contractor { get; set; }
    public string ApplicationType { get; set; }
    public string ContractorPinfl { get; set; }
}
