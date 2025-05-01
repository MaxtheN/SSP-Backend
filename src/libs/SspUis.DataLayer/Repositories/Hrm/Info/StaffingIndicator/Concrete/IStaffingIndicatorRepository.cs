

using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IStaffingIndicatorRepository : IBaseEntityRepository<int, StaffingIndicator, CreateStaffingIndicatorDlDto, UpdateStaffingIndicatorDlDto>
    {
    }
}
