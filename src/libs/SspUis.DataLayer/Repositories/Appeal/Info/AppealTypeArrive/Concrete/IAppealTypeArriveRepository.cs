using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IAppealTypeArriveRepository :
        IBaseEntityRepository<int, 
            AppealTypeArrive, 
            CreateAppealTypeArriveDlDto,
            UpdateAppealTypeArriveDlDto>
    {
    }
}
