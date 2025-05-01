using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IArbitrationResultRepository :
	IBaseEntityRepository<long,
		ArbitrationResult,
		CreateArbitrationResultDlDto,
		UpdateArbitrationResultDlDto,
		UpdateStatusArbitrationResultDlDto>
  
{

}

