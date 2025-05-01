using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IIndicatorDepartmentRepository : 
	IBaseEntityRepository<int, IndicatorDepartment,
		CreateIndicatorDepartmentDlDto, UpdateIndicatorDepartmentDlDto>
{

}

