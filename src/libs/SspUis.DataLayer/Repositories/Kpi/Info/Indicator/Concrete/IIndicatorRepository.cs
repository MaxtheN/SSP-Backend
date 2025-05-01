using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IIndicatorRepository : 
	IBaseEntityRepository<int, Indicator,
		CreateIndicatorDlDto, UpdateIndicatorDlDto>
{

}

