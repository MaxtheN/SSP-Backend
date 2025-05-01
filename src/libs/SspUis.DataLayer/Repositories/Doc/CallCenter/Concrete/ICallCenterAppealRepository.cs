using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface ICallCenterAppealRepository
    :IBaseEntityRepository<long,
        CallCenterAppeal,
        CreateCallCenterAppealDlDto,
        UpdateCallCenterAppealDlDto,
        UpdateStatusCallCenterAppealDlDto>
{
}
