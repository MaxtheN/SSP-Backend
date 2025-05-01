using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Memship.Doc.MemshipPaymentOrder;

namespace SspUis.BizLogicLayer.Memship.Doc.MemshipPaymentOrderServices;

public class MemshipPaymentOrderTableDto : MemshipPaymentOrderTableDlDto,
	ILinkToEntity<MemshipPaymentOrderTable>
{
	public string ApplicationType { get; set; }
	public string NeedChamberService { get; set; }
}
