using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface INeedChamberServiceRepository : IBaseEntityRepository<int, NeedChamberService, CreateNeedChamberServiceDlDto, UpdateNeedChamberServiceDlDto>
{
}
