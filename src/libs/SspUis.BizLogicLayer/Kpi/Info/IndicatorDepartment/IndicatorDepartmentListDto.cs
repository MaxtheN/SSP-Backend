using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class IndicatorDepartmentListDto : ILinkToEntity<IndicatorDepartment>, IHaveIdProp<int>
{
	public int Id { get; set; }
	public string Code { get; set; } = null!;
	public string ShortName { get; set; } = null!;
	public string FullName { get; set; } = null!;
	public string State { get; set; } = null!;
}

