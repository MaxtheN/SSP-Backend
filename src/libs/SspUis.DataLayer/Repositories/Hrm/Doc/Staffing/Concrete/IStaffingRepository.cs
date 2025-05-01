using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public interface IStaffingRepository : IBaseEntityRepository<long, Staffing, CreateStaffingDlDto, UpdateStaffingDlDto, UpdateStatusStaffingDlDto>
    {
    }
}
