using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface INeedChamberServiceGroupRepository 
        : IBaseEntityRepository<int, NeedChamberServiceGroup, CreateNeedChamberServiceGroupDlDto, UpdateNeedChamberServiceGroupDlDto>
    { }
}
