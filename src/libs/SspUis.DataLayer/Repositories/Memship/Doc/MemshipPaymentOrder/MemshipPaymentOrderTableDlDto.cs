using SspUis.DataLayer.EfClasses;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Memship.Doc.MemshipPaymentOrder;

public class MemshipPaymentOrderTableDlDto :
	EntityDto<MemshipPaymentOrderTableDlDto,
		MemshipPaymentOrderTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public decimal Amount { get; set; }
	public int ApplicationTypeId { get; set; }
	public int NeedChamberServiceId { get; set; }
}
