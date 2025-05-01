using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Appeal;

public interface IAppealApplicationRepository
    :IBaseEntityRepository<long,
        AppealApplication,
        CreateAppealApplicationDlDto,
        UpdateAppealApplicationDlDto,
        UpdateStatusAppealApplicationDlDto>
{
}
