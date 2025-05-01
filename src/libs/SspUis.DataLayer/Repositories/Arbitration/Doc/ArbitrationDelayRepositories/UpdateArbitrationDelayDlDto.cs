using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateArbitrationDelayDlDto :
	ArbitrationDelayDlDto<UpdateArbitrationDelayDlDto>,
	IHaveIdProp<long>
{
	[LocalizedRequired]
	[LocalizedRange(1, long.MaxValue)]
	public long Id { get; set; }
}
