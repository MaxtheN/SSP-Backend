using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IAppealDescriptionRepository : IBaseEntityRepository<int, AppealDescription, CreateAppealDescriptionDlDto, UpdateAppealDescriptionDlDto>
    {
    }
}
