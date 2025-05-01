using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public interface IWorkDayOffRepository
    :IBaseEntityRepository<long,WorkDayOff,CreateWorkDayOffDlDto,UpdateWorkDayOffDlDto,UpdateStatusWorkDayOffDlDto>
{
}
