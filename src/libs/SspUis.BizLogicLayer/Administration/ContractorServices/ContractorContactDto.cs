using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.ContractorServices;

public class ContractorContactDto : ContractorContactDlDto, ILinkToEntity<ContractorContact>
{
    public string ContactType { get; set; }
    public long OwnerId { get; set; }

}
