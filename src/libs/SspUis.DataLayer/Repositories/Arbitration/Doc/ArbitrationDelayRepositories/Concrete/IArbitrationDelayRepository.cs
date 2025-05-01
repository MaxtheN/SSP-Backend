using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IArbitrationDelayRepository :
	IBaseEntityRepository<long,
		ArbitrationDelay,
		CreateArbitrationDelayDlDto,
		UpdateArbitrationDelayDlDto,
		UpdateStatusArbitrationDelayDlDto>
  
{

}

