using GenericServices;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories.Corruption;

namespace SspUis.BizLogicLayer.Corruption;

public class JoinAntiCorruptionResultTableDto : JoinAntiCorruptionResultTableDlDto, ILinkToEntity<JoinAntiCorruptionResultTable>
{
    public string Application { get; set; } = null!;
    public string ApplicationDocNumber { get; set; } = null!;
    public string JoinAntiCorruptionResultType { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
}
