using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;


namespace SspUis.BizLogicLayer.Memship;

public class MemshipNewContractorTableDto : MemshipNewContractorTableDlDto, ILinkToEntity<MemshipNewContractorsTable>
{
    public string District { get; set; }
}
